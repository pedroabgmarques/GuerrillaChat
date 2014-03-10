using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;

namespace Test_client.cs
{
    public partial class Test_client : Form
    {

        public void BotaoLigar(object acc)
        {

            string accao = (string)acc;

            if (accao == "activar")
            {
                btn_ligar.Enabled = true;
            }
            if (accao == "desactivar")
            {
                btn_ligar.Enabled = false;
            }
            
        }
        public delegate void BotaoLigarCallback(object acc);

        public Test_client()
        {
            InitializeComponent();
        }

        private void btn_ligar_Click(object sender, EventArgs e)
        {
           
            btn_ligar.Enabled = false;
            try
            {
                Iniciar_ligacao("localhost", 3000);
                
                //thread que fica a aguardar comunicações iniciadas pelo servidor
                aguardar = new Thread(new ParameterizedThreadStart(Aguardar_resposta));
                aguardar.Start(clientStream);
                
            }
            catch
            {
                btn_ligar.Enabled = true;
            }

        }

        TcpClient client;
        string servidor;
        int porto;
        NetworkStream clientStream;
        string nick;
        Thread processar;
        Thread aguardar;

        private void Iniciar_ligacao(string servidor, int porto)
        {
            client = new TcpClient();
            client.Connect(servidor, porto);
            clientStream = client.GetStream();
        }

        private void Enviar_comando(string comando)
        {
            try
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(comando);

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }
            catch
            {
                btn_ligar.Enabled = true;
            }
        }

        private void Aguardar_resposta(object clientStream)
        {

            while (true)
            {
                NetworkStream stream = (NetworkStream)clientStream;
                byte[] message = new byte[4096];
                int bytesRead;

                while (true)
                {
                    bytesRead = 0;

                    try
                    {
                        //blocks until a client sends a message
                        bytesRead = stream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        //a socket error has occured
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        break;
                    }

                    //message has successfully been received
                    ASCIIEncoding encoder = new ASCIIEncoding();

                    string mensagem = encoder.GetString(message, 0, bytesRead);

                    //DEBUG
                    /*txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Servidor: " + mensagem, Origem.Servidor }); */

                    //cria nova thread para processar o conteudo da mensagem
                    //thread que fica a aguardar comunicações iniciadas pelo servidor

                    processar = new Thread(new ParameterizedThreadStart(Processamento_mensagens));
                    processar.Start(mensagem);

                }
            }
        }

        private void Processamento_mensagens(object mens)
        {

            string message = (string)mens;

            string[] mensagem_individual = message.Split('|');

            //DEBUG - ver mensagens enviadas pelo servidor
            /*txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
            new object[] { "Servidor: " + mensagem, Origem.Erro });*/

            foreach (string mensagem in mensagem_individual)
            {
                if (mensagem != "")
                {

                    string[] mensagem_separada = mensagem.Split(',');



                    //lista de clientes
                    if (mensagem_separada[0] == "2")
                    {

                        string[] blocos = mensagem_separada[1].Split('/');
                        int i = 0;
                        while (i < blocos.Length)
                        {
                            if (blocos[i] != null)
                            {
                                //escrever lista de clientes, não se aplica
                            }
                            i++;
                        }

                    }


                    //nick deste cliente
                    else if (mensagem_separada[0] == "4")
                    {

                        if (mensagem_separada[1] != "")
                        {
                            nick = mensagem_separada[1];
                        }

                        bool siga = true;
                        while (siga)
                        {

                            Random gerador = new Random();
                            Thread.Sleep(gerador.Next(30000));
                            Enviar_comando("ola!");
                            Thread.Sleep(gerador.Next(30000));
                            Enviar_comando("tudo bem contigo?");
                            Thread.Sleep(gerador.Next(30000));
                            Enviar_comando("sim, vai-se andando");
                            Thread.Sleep(gerador.Next(30000));
                            Enviar_comando("e entao que tens feito?");
                            Thread.Sleep(gerador.Next(30000));
                            Enviar_comando("novidades so no continente =P");
                            Thread.Sleep(gerador.Next(30000));
                            Enviar_comando("/,nick anonimo" + DateTime.Now.Minute + DateTime.Now.Minute+DateTime.Now.Millisecond);
                            siga = false;
                        }

                    }


                    //kill order do servidor
                    else if (mensagem_separada[0] == "6")
                    {
                        btn_ligar.Invoke(new BotaoLigarCallback(this.BotaoLigar),
                        new object[] { "activar" });
                        client.Close();
                        processar.Abort();
                        aguardar.Abort();

                    }

                    //pedido inicial de nick
                    else if (mensagem_separada[0] == "7")
                    {
                        string nick_requisitado = "";
                        if (nick_requisitado == "")
                        {
                            Enviar_comando("/,nick_please [!*!!!!n0_n1ck_p1e4s3!!!!*!]");
                        }
                        else
                        {
                            Enviar_comando("/,nick_please " + nick_requisitado);
                        }
                    }
                }
            }
        }

        public void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (processar.IsAlive)
            {
                processar.Abort();
            }
            if (aguardar.IsAlive)
            {
                aguardar.Abort();
            }
            Enviar_comando("/,quit"); 
            client.Close(); 
            
            Environment.Exit(0);
        }

        private void pedir_lista_Tick(object sender, EventArgs e)
        {
            Enviar_comando("/,clients_please");
        }

        }



    }
