using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Threading;

namespace TCPClient
{
    public partial class Cliente_inicio : Form
    {

        public enum Origem
        {
            Servidor,
            Cliente,
            Erro,
            Saida,
            Proprio
        }


        //código para actualizar componentes do UI a partir de outras threads
        public void UpdateStatus(string text, Origem origem)
        {
            // Set the textbox text.
            switch (origem)
            {
                case Origem.Servidor:
                    txt_status.SelectionColor = Color.Green;
                    break;
                case Origem.Cliente:
                    txt_status.SelectionColor = Color.Black;
                    break;
                case Origem.Erro:
                    txt_status.SelectionColor = Color.Red;
                    break;
                case Origem.Saida:
                    txt_status.SelectionColor = Color.Blue;
                    break;
                case Origem.Proprio:
                    txt_status.SelectionColor = Color.DarkViolet;
                    break;
                default:
                    break;
            }
            txt_status.AppendText(text);
            txt_status.AppendText("\n");
            txt_status.ScrollToCaret();
        }
        public delegate void UpdateStatusCallback(string text, Origem origem);

        public void UpdateClientes(string accao,string text)
        {
            // Set the textbox text.
            if (accao == "entrar")
            {
                if (text != null && text != "")
                {
                    txt_clientes.Items.Add(text);
                }
            }
            if (accao == "sair")
            {
                if (text != null && text != "")
                {
                    txt_clientes.Items.Remove(text);
                }
            }
        }
        public delegate void UpdateClientesCallback(string accao, string text);

        public void UpdateNick(string text)
        {
            txt_nick.Text = text;
        }
        public delegate void UpdateNickCallback(string text);

        public void FocusNick()
        {
            txt_nick.Focus();
        }
        public delegate void FocusNickCallback();


        public void RefreshStatus()
        {
            txt_status.Refresh();
        }
        public delegate void RefreshStatusCallback();

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

        public void Txt_nick(object acc)
        {

            string accao = (string)acc;

            if (accao == "activar")
            {
                txt_nick.Enabled = true;
            }
            if (accao == "desactivar")
            {
                txt_nick.Enabled = false;
            }

        }
        public delegate void Txt_nickCallback(object acc);

        public void BotaoComando(object acc)
        {

            string accao = (string)acc;

            if (accao == "activar")
            {
                btn_comando.Enabled = true;
            }
            if (accao == "desactivar")
            {
                btn_comando.Enabled = false;
            }

        }
        public delegate void BotaoComandoCallback(object acc);

        public void Txt_servidor(object acc)
        {

            string accao = (string)acc;

            if (accao == "activar")
            {
                txt_servidor.Enabled = true;
            }
            if (accao == "desactivar")
            {
                txt_servidor.Enabled = false;
            }

        }
        public delegate void Txt_servidorCallback(object acc);

        public void Txt_porto(object acc)
        {

            string accao = (string)acc;

            if (accao == "activar")
            {
                txt_porto.Enabled = true;
            }
            if (accao == "desactivar")
            {
                txt_porto.Enabled = false;
            }

        }
        public delegate void Txt_portoCallback(object acc);



        public void LimpaClientes()
        {
            // Set the textbox text.
            txt_clientes.Items.Clear();
        }
        public delegate void LimpaClientesCallback();

        public Cliente_inicio()
        {

            InitializeComponent();
            btn_comando.Enabled = false;
            
            
            //activar o Enter para enviar o comando
            txt_comando.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Return)
                {
                    btn_comando.PerformClick();
                }
            };

            txt_nick.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == Keys.Return)
                {
                    btn_ligar.PerformClick();
                }
            };

            txt_nick.Focus();

            
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
                txt_status.SelectionColor = Color.Red;
                txt_status.AppendText("\nNão foi possível estabelecer uma ligação ao servidor! Verifique o IP do servidor e se o porto está aberto no servidor.");
                btn_ligar.Enabled = true;
                btn_comando.Enabled = false;
                txt_clientes.Items.Clear();
                txt_servidor.Enabled = true;
                txt_porto.Enabled = true;
                txt_nick.Enabled = true;
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

                    //resposta a comando
                    if (mensagem_separada[0] == "0")
                    {
                        if (mensagem_separada[1] != "c")
                        {
                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                            new object[] { "Servidor: " + mensagem_separada[1], Origem.Servidor });
                        }
                        else
                        {
                            txt_status.Invoke(new RefreshStatusCallback(this.RefreshStatus),
                            new object[] { });
                        }

                    }

                    //erro
                    else if (mensagem_separada[0] == "1")
                    {
                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                        new object[] { "Erro: " + mensagem_separada[1], Origem.Erro });
                    }

                    //lista de clientes
                    else if (mensagem_separada[0] == "2")
                    {

                        string[] blocos = mensagem_separada[1].Split('/');

                        //limpar a lista de clientes que vai ser renovada
                        txt_clientes.Invoke(new LimpaClientesCallback(this.LimpaClientes),
                        new object[] { });

                        int i = 0;
                        while (i < blocos.Length)
                        {
                            if (blocos[i] != null)
                            {
                                txt_clientes.Invoke(new UpdateClientesCallback(this.UpdateClientes),
                                new object[] { "entrar",blocos[i] });
                            }
                            i++;
                        }

                    }

                    //entrada e saida de clientes
                    else if (mensagem_separada[0].StartsWith("3"))
                    {
                        if (mensagem_separada[0] == "3.1")
                        {
                            //entrada
                            txt_clientes.Invoke(new UpdateClientesCallback(this.UpdateClientes),
                            new object[] { "entrar", mensagem_separada[1] });
                            
                        }
                        if (mensagem_separada[0] == "3.2")
                        {
                            //saida
                            txt_clientes.Invoke(new UpdateClientesCallback(this.UpdateClientes),
                            new object[] { "sair", mensagem_separada[1] });
                            
                        }

                        if (mensagem_separada[0] == "3")
                        {
                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                            new object[] { "Servidor: " + mensagem_separada[1], Origem.Saida });
                        }
                    }

                    //nick deste cliente
                    else if (mensagem_separada[0] == "4")
                    {

                        if (mensagem_separada[1] != "")
                        {
                            nick = mensagem_separada[1];
                            txt_nick.Invoke(new UpdateNickCallback(this.UpdateNick),
                            new object[] { nick });
                        }
                        else
                        {
                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                            new object[] { "Servidor: " + mensagem, Origem.Erro });
                        }

                    }


                    //broadcast de alteracao de nick
                    else if (mensagem_separada[0] == "5")
                    {

                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                        new object[] { "Servidor: " + mensagem_separada[1], Origem.Servidor });

                    }
                    //kill order do servidor
                    else if (mensagem_separada[0] == "6")
                    {

                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                        new object[] { "Servidor: " + mensagem_separada[1], Origem.Servidor });
                        txt_status.Invoke(new LimpaClientesCallback(this.LimpaClientes),
                        new object[] { });
                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                        new object[] { "Sessão encerrada.", Origem.Erro });
                        btn_ligar.Invoke(new BotaoLigarCallback(this.BotaoLigar),
                        new object[] { "activar" });
                        btn_comando.Invoke(new BotaoComandoCallback(this.BotaoComando),
                        new object[] { "desactivar" });
                        txt_servidor.Invoke(new Txt_servidorCallback(this.Txt_servidor),
                        new object[] { "activar" });
                        txt_porto.Invoke(new Txt_portoCallback(this.Txt_porto),
                        new object[] { "activar" });
                        txt_nick.Invoke(new Txt_nickCallback(this.Txt_nick),
                        new object[] { "activar" });
                        txt_nick.Invoke(new FocusNickCallback(this.FocusNick),
                        new object[] { });
                        client.Close();
                        processar.Abort();
                        aguardar.Abort();

                    }

                    //pedido inicial de nick
                    else if (mensagem_separada[0] == "7")
                    {
                        string nick_requisitado = txt_nick.Text;
                        if (nick_requisitado == "")
                        {
                            Enviar_comando("/,nick_please [!*!!!!n0_n1ck_p1e4s3!!!!*!]");
                        }
                        else
                        {
                            Enviar_comando("/,nick_please " + nick_requisitado);
                        }
                    }


                    //mensagem normal de outros clientes
                    else if(mensagem_separada[0]=="8")
                    {


                        string[] origem = mensagem.Split(':');
                        string codigo_e_nick = origem[0].TrimStart();
                        string[] codigo_nick = codigo_e_nick.Split(',');
                        string nick_origem = codigo_nick[1];

                        //a mensagem é do proprio cliente
                        if (nick_origem == nick)
                        {
                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                            new object[] { nick_origem+": "+origem[1], Origem.Proprio });
                        }
                        else
                        {

                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                            new object[] { nick_origem+ ": " + origem[1], Origem.Cliente });
                        }
                    }
                }
            }
        }

        private void btn_ligar_Click(object sender, EventArgs e)
        {
            servidor = txt_servidor.Text;
            porto = Convert.ToInt16(txt_porto.Text);
            
            txt_status.Clear();
            btn_ligar.Enabled = false;
            txt_nick.Enabled = false;
            txt_status.ForeColor = Color.Black;
            txt_status.AppendText("A ligar ao servidor "+servidor+"...\n");
            try
            {
                Iniciar_ligacao(servidor, porto);

                btn_comando.Enabled = true;
                txt_servidor.Enabled = false;
                txt_porto.Enabled = false;
                txt_comando.Focus();
                
                
                //thread que fica a aguardar comunicações iniciadas pelo servidor
                aguardar = new Thread(new ParameterizedThreadStart(Aguardar_resposta));
                aguardar.Start(clientStream);
                
            }
            catch
            {
                txt_status.SelectionColor = Color.Red;
                txt_status.AppendText("\nNão foi possível estabelecer uma ligação ao servidor! Verifique o IP do servidor e se o porto está aberto no servidor.");
                btn_ligar.Enabled = true;
                btn_comando.Enabled = false;
                txt_clientes.Items.Clear();
                txt_servidor.Enabled = true;
                txt_porto.Enabled = true;
                txt_nick.Enabled = true;
            }

        }

        private void btn_comando_Click(object sender, EventArgs e)
        {
            string comando = txt_comando.Text;
            if (comando.StartsWith("/"))
            {
                comando = comando.Replace("/", "");
                comando = "/," + comando;
            }
            txt_comando.Text = "";
            Enviar_comando(comando);
            txt_status.ScrollToCaret();   
        }

        private void Refresh_status_Tick(object sender, EventArgs e)
        {
            txt_status.Refresh();
        }


        
        public void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (processar.IsAlive) { processar.Abort(); }
            if (aguardar.IsAlive) { aguardar.Abort(); }
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

