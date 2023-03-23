﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Nu_te_supara_frate
{
    //server
    public partial class Tabla : Form
    {
        public TcpListener listener;// el e server
        public String listener_data;
        Thread t;
        NetworkStream stream_server;
        bool work_thread;
        private static Tabla server_form;
        public string enemy_name;
        public string my_name;



        Form inrg;
        Zar[] zar;
        Drum drum1;
        Jucator[] j1 = new Jucator[4];
        Jucator[] j2 = new Jucator[4];
        Random r;
        Castig castig;


        public Tabla(Inreg form)
        {
            InitializeComponent();

            my_name = form.get_name();


            listener = new TcpListener(System.Net.IPAddress.Any, 5000);
            listener.Start();
            t = new Thread(new ThreadStart(asculta_server));
            work_thread = true;
            t.Start();

            inrg = form;
            castig = new Castig(this);
            inrg.Hide();
            rand2.Hide();

            drum1 = new Drum(this);
            drum1.getDrum();

            r = new Random();
            zar = new Zar[2];

            zar[0] = new Zar(r, Z1);
            zar[1] = new Zar(r, Z2);

            j1[0] = new Rosu(this, "nume1", zar[0], R1, -1, "rosu");
            j1[1] = new Rosu(this, "nume2", zar[0], R2, -1, "rosu");
            j1[2] = new Rosu(this, "nume3", zar[0], R3, -1, "rosu");
            j1[3] = new Rosu(this, "nume4", zar[0], R4, -1, "rosu");

            j2[0] = new Galben(this, "nume1", zar[1], G1, -1, "galben");
            j2[1] = new Galben(this, "nume2", zar[1], G2, -1, "galben");
            j2[2] = new Galben(this, "nume3", zar[1], G3, -1, "galben");
            j2[3] = new Galben(this, "nume4", zar[1], G4, -1, "galben");

            server_form = this;
            this.J1.Text = "";
        }

        public void asculta_server()
        {
            while (work_thread)
            {
                Socket socketServer = listener.AcceptSocket();
                try
                {
                    stream_server = new NetworkStream(socketServer);
                    StreamReader citireServer = new StreamReader(stream_server);

                    while (work_thread)
                    {

                        string dateServer = citireServer.ReadLine();
                        
                        MethodInvoker m = new MethodInvoker(() =>
                        {
                           
                            if (dateServer[0] == 'z')
                            {
                                zar[0].SetFata(Convert.ToInt32(dateServer[1] + ""));
                                set_turn(Convert.ToInt32(dateServer[1] + ""));
                               
                            }
                            else if (dateServer[0] == 'n')
                            {
                                for (int i = 1; i < dateServer.Length; i++)
                                {
                                    this.J1.Text += dateServer[i];
                                }
                            }
                            else if (dateServer[0] == 'p')
                            {
                                switch (Convert.ToInt32(dateServer[1] + ""))
                                {
                                    case 1:
                                        this.R1_Click();
                                        break;
                                    case 2:
                                        this.R2_Click();
                                        break;
                                    case 3:
                                        this.R3_Click();
                                        break;
                                    case 4:
                                        this.R4_Click();
                                        break;
                                }
                            }
                           
                        });
                        server_form.Invoke(m);

                    }
                    stream_server.Close();
                }
                catch (Exception e)
                {
#if LOG
                    Console.WriteLine(e.Message);
#endif
                }
                socketServer.Close();
            }

        }

        public Tabla()
        {
            InitializeComponent();

        }

        public void set_turn(int r)
        {
            if (r < 6)
            {
                rand1.Hide();
                rand2.Show();
            }
            else
            {
                rand1.Show();
            }
        }

        private void bX_Click(object sender, EventArgs e)
        {
            this.Hide();
            inrg.Show();

        }
        public void Roll2_Click(object sender, EventArgs e)
        {
            zar[1].Roll();

            if (zar[1].getFata() < 6)
            {
                rand2.Hide();
                rand1.Show();
            }
            else
            {
                rand2.Show();
            }

            StreamWriter scriere = new StreamWriter(stream_server);
            scriere.AutoFlush = true;
            scriere.WriteLine("z" + Convert.ToString(zar[1].getFata()));        
                                                                                

        }



        private void R1_Click()
        {
            if (j1[0].getPozitie() == -1 && zar[0].getFata() == 6)
            {
                j1[0].getPion().Location = drum1.getDrum()[0].Location;
                j1[0].setPozite(1);
                zar[0].SetFata(0);
            }
            else
            {
                if (j1[0].getPozitie() != -1)
                {
                    j1[0].muta(zar[0], drum1, this);

                    if (j1[0].verificare(drum1, j1) == true)
                    {
                        castig.NumeCastigator.Text = J1.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
        }
        private void R2_Click()
        {
            if (j1[1].getPozitie() == -1 && zar[0].getFata() == 6)
            {
                j1[1].getPion().Location = drum1.getDrum()[0].Location;
                j1[1].setPozite(1);
                zar[0].SetFata(0);
            }
            else
            {
                if (j1[1].getPozitie() != -1)
                {
                    j1[1].muta(zar[0], drum1, this);

                    if (j1[1].verificare(drum1, j1) == true)
                    {
                        castig.NumeCastigator.Text = J1.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
        }
        private void R3_Click()
        {
            if (j1[2].getPozitie() == -1 && zar[0].getFata() == 6)
            {
                j1[2].getPion().Location = drum1.getDrum()[0].Location;
                j1[2].setPozite(1);
                zar[0].SetFata(0);
            }
            else
            {
                if (j1[2].getPozitie() != -1)
                {
                    j1[2].muta(zar[0], drum1, this);

                    if (j1[2].verificare(drum1, j1) == true)
                    {
                        castig.NumeCastigator.Text = J1.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
        }
        private void R4_Click()
        {
            if (j1[3].getPozitie() == -1 && zar[0].getFata() == 6)
            {
                j1[3].getPion().Location = drum1.getDrum()[0].Location;
                j1[3].setPozite(1);
                zar[0].SetFata(0);
            }
            else
            {
                if (j1[3].getPozitie() != -1)
                {
                    j1[3].muta(zar[0], drum1, this);

                    if (j1[3].verificare(drum1, j1) == true)
                    {
                        castig.NumeCastigator.Text = J1.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
        }




        private void G1_Click(object sender, EventArgs e)
        {
            if (j2[0].getPozitie() == -1 && zar[1].getFata() == 6)
            {
                j2[0].getPion().Location = drum1.getDrum()[20].Location;
                j2[0].setPozite(1);
                zar[1].SetFata(0);
            }
            else
            {
                if (j2[0].getPozitie() != -1)
                {
                    j2[0].muta(zar[1], drum1, this);
                    if (j2[0].verificare(drum1, j2) == true)
                    {
                        castig.NumeCastigator.Text = J2.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
            StreamWriter scriere = new StreamWriter(stream_server);
            scriere.AutoFlush= true;
            scriere.WriteLine("p1");
        }
        private void G2_Click(object sender, EventArgs e)
        {
            if (j2[1].getPozitie() == -1 && zar[1].getFata() == 6)
            {
                j2[1].getPion().Location = drum1.getDrum()[20].Location;
                j2[1].setPozite(1);
                zar[1].SetFata(0);
            }
            else
            {
                if (j2[1].getPozitie() != -1)
                {
                    if (j2[1].getPozitie() != -1)
                    {
                        j2[1].muta(zar[1], drum1, this);
                        if (j2[1].verificare(drum1, j2) == true)
                        {
                            castig.NumeCastigator.Text = J2.Text;
                            castig.Show();
                            this.Hide();
                        }
                    }
                }
            }
            StreamWriter scriere = new StreamWriter(stream_server);
            scriere.AutoFlush = true;
            scriere.WriteLine("p2");
        }
        private void G3_Click(object sender, EventArgs e)
        {
            if (j2[2].getPozitie() == -1 && zar[1].getFata() == 6)
            {
                j2[2].getPion().Location = drum1.getDrum()[20].Location;
                j2[2].setPozite(1);
                zar[1].SetFata(0);
            }
            else
            {
                if (j2[2].getPozitie() != -1)
                {
                    j2[2].muta(zar[1], drum1, this);
                    if (j2[2].verificare(drum1, j2) == true)
                    {
                        castig.NumeCastigator.Text = J2.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
            StreamWriter scriere = new StreamWriter(stream_server);
            scriere.AutoFlush = true;
            scriere.WriteLine("p3");
        }
        private void G4_Click(object sender, EventArgs e)
        {
            if (j2[3].getPozitie() == -1 && zar[1].getFata() == 6)
            {
                j2[3].getPion().Location = drum1.getDrum()[20].Location;
                j2[3].setPozite(1);
                zar[1].SetFata(0);
            }
            else
            {
                if (j2[3].getPozitie() != -1)
                {
                    j2[3].muta(zar[1], drum1, this);
                    if (j2[3].verificare(drum1, j2) == true)
                    {
                        castig.NumeCastigator.Text = J2.Text;
                        castig.Show();
                        this.Hide();
                    }
                }
            }
            StreamWriter scriere = new StreamWriter(stream_server);
            scriere.AutoFlush = true;
            scriere.WriteLine("p4");
        }

        private void Tabla_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            while (true)
            {
                try
                {
                    StreamWriter scriere = new StreamWriter(stream_server);
                    scriere.AutoFlush = true; 
                    scriere.WriteLine("n" + my_name);
                    break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Waiting for other player to join");


                }
            }
                
            
            
        }
    }

}
