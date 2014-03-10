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
using System.Threading;
using System.Linq.Expressions;
using System.Collections;

namespace TCPServer
{
    public partial class TCPServer : Form
    {

        public enum Origem
        {
            Servidor,
            Cliente,
            Saida,
            Erro
        }

        
        //código para actualizar componentes do UI a partir de outras threads
        public void UpdateStatus(string text, Origem origem)
        {
            // Set the textbox text.
            switch (origem)
            {
                case Origem.Servidor:
                    txt_status.SelectionColor = Color.Black;
                    break;
                case Origem.Cliente:
                    txt_status.SelectionColor = Color.Green;
                    break;
                case Origem.Saida:
                    txt_status.SelectionColor = Color.Blue;
                    break;
                case Origem.Erro:
                    txt_status.SelectionColor = Color.Red;
                    break;
                default:
                    break;
            }
            txt_status.AppendText(text);
            txt_status.AppendText("\n");
            txt_status.ScrollToCaret();
        }
        public delegate void UpdateStatusCallback(string text, Origem origem);


        public TCPServer()
        {
            InitializeComponent();
        }

        TcpListener tcpListener;
        Thread listenThread;
        Thread clientThread;
        Thread processar;
        Thread broadcast;
        Hashtable lista_clientes = new Hashtable();

        
        private void btn_iniciar_programa_Click(object sender, EventArgs e)
        {
            string prt = txt_porto.Text;
            int porto = Convert.ToInt16(prt);
            btn_iniciar_programa.Enabled = false;
            txt_porto.Enabled = false;
            txt_status.Clear();
            txt_status.AppendText("A escutar no porto "+porto+"\n");
            txt_status.AppendText("Servidor iniciado, a aguardar ligações..\n");

            tcpListener = new TcpListener(IPAddress.Any, porto);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();    
            
        }

        private void ListenForClients()
        {
            tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = new TcpClient();
                client = this.tcpListener.AcceptTcpClient();
                NetworkStream clientStream = client.GetStream();
                Enviar_mensagem("|7,nick please", client);

                //create a thread to handle communication
                //with connected client
                clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;

            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
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
                object objecto_mensagem = (object)mensagem;
                object objecto_cliente = (object)tcpClient;
                object[] parametros = { objecto_mensagem, objecto_cliente };
                object param = (object)parametros;

                processar = new Thread(new ParameterizedThreadStart(Processar_comando));
                try
                {
                    processar.Start(param);
                }
                catch
                {
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Comando perdido!", Origem.Erro });
                    processar.Join();
                    processar.Abort();
                }
               
                
            }

            tcpClient.Close();
        }

        private void Processar_comando(object param)
        {

            object[] parametros = (object[])param;
            string mensagem = (string)parametros[0];
            TcpClient cliente = (TcpClient)parametros[1];
            string temp = mensagem;
            string[] mensagem_separada = temp.Split(',');

            if (mensagem_separada[0] == "/")
            {

                string comando = mensagem_separada[1];

                //DEBUG - Ver todos os comandos enviados pelos clientes
                //txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                //new object[] { (string)lista_clientes[cliente] + ": " + comando, Origem.Cliente });

                if (comando.ToLower() == "clients_please")
                {
                    Enviar_lista(cliente);
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                new object[] { (string)lista_clientes[cliente] + ": " + comando, Origem.Cliente });
                }
                
                if (comando.ToLower() == "?" || comando.ToLower() == "help")
                {
                    Enviar_ajuda(cliente);
                }

                if (comando.ToLower() == "teste")
                {
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                new object[] { (string)lista_clientes[cliente] + ": " + comando, Origem.Cliente });
                    for (int i = 0; i < 10; i++)
                    {
                        Enviar_mensagem("|0,Teste de comunicacao..", cliente);
                        Thread.Sleep(2000);
                    }
                    Enviar_mensagem("|0,Teste completo!", cliente);
                }

                if (comando.ToLower() == "quit")
                {
                    string nick = (string)lista_clientes[cliente];
                    Enviar_mensagem("|6,Adeus " + (string)lista_clientes[cliente] + "!", cliente);
                    if (broadcast.IsAlive)
                    {
                        broadcast.Join();
                    }
                    try
                    {
                        lista_clientes.Remove(cliente);
                    }
                    catch
                    {
                        Thread.Sleep(50);
                        lista_clientes.Remove(cliente);
                    }
                    object message = (object)"|3," + nick + " bazou!";
                    broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                    try
                    {
                        broadcast.Start(message);
                    }
                    catch
                    {
                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                        broadcast.Join();
                        broadcast.Abort();
                    }
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { nick + " bazou.", Origem.Saida });
                    Broadcast_update_lista_clientes("sair", nick);
                }

                if (comando.ToLower().StartsWith("nick "))
                {

                    string[] comando_separado = comando.Split(' ');
                    string nick_requisitado = comando_separado[1].Trim().ToLower();
                    if (lista_clientes.ContainsValue(nick_requisitado))
                    {
                        Enviar_mensagem("|1,Nick " + nick_requisitado + " ja em utilizacao!", cliente);
                    }
                    else
                    {
                        string nick_actual = (string)lista_clientes[cliente];
                        if (broadcast.IsAlive)
                        {
                            broadcast.Join();
                        }
                        lista_clientes.Remove(cliente);
                        if (broadcast.IsAlive)
                        {
                            broadcast.Join();
                        }
                        lista_clientes.Add(cliente, nick_requisitado);
                        Broadcast_update_lista_clientes("sair", nick_actual);
                        Broadcast_update_lista_clientes("entrar", nick_requisitado);

                        Enviar_mensagem("|4," + (string)lista_clientes[cliente], cliente);

                        string mensagem_enviar = "|5," + nick_actual + " alterou o nick para " + nick_requisitado;
                        object message = (object)mensagem_enviar;
                        broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                        try
                        {
                            broadcast.Start(message);
                        }
                        catch
                        {
                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                            broadcast.Join();
                            broadcast.Abort();
                        }

                       

                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                new object[] { (string)lista_clientes[cliente] + ": " + comando, Origem.Cliente });
                    }

                }

                if (comando.ToLower().StartsWith("nick_please "))
                {
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                new object[] { (string)lista_clientes[cliente] + ": " + comando, Origem.Cliente });

                    string[] comando_separado = comando.Split(' ');

                    string nick_requisitado = comando_separado[1].ToLower();
                    string nick_final;
                    if (nick_requisitado == "[!*!!!!n0_n1ck_p1e4s3!!!!*!]")
                    {
                        nick_final = "anonimo" + DateTime.Now.Minute + DateTime.Now.Minute + DateTime.Now.Millisecond;
                    }
                    else
                    {
                        if (lista_clientes.ContainsValue(nick_requisitado))
                        {
                            nick_final = "anonimo" + lista_clientes.Count;
                            Enviar_mensagem("|1,Nick " + nick_requisitado + " ja em utilizacao!", cliente);
                        }
                        else
                        {
                            nick_final = nick_requisitado;
                        }
                    }

                    //Broadcast boas vindas
                    object message = (object)"|3,Ora vamos la dar as boas vindas ao ilustre " + nick_final + "!";
                    broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                    try
                    {
                        broadcast.Start(message);
                    }
                    catch
                    {
                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                        broadcast.Join();
                        broadcast.Abort();
                    }

                    //Broadcast da entrada deste cliente
                    Broadcast_update_lista_clientes("entrar", nick_final);

                    //Adicionar o cliente à lista
                    if (broadcast.IsAlive)
                    {
                        broadcast.Join();
                    }
                    lista_clientes.Add(cliente, nick_final);

                    Enviar_lista(cliente);

                    //Enviar o nick ao cliente
                    Enviar_mensagem("|4," + nick_final, cliente);

                    //enviar hello
                    Enviar_mensagem("|0,hello " + nick_final, cliente);
                    Enviar_ajuda(cliente);

                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { nick_final + " entrou.", Origem.Saida });

                }
            }
            else
            {
                //escreve a mensagem que veio do cliente

                object message = "|8,"+(object)(string)lista_clientes[cliente] + ": " + mensagem;
                broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                try
                {
                    broadcast.Start(message);
                }
                catch
                {
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                    broadcast.Join();
                    broadcast.Abort();
                }
                
            }

        }

        private void Enviar_ajuda(TcpClient cliente)
        {
            string mensagem = "\n|0,Comandos disponiveis:\n/? - Ajuda;\n/help - Ajuda;\n/teste - Testar a ligacao com o servidor;\n/nick <nickname> - alterar o nickname\n/quit - sair;\n";
            Enviar_mensagem(mensagem, cliente);
        }




        private void Enviar_mensagem(string mensagem, object client)
        {
            try
            {
                TcpClient tcpClient = (TcpClient)client;
                NetworkStream clientStream = tcpClient.GetStream();
                ASCIIEncoding encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes(mensagem);
                clientStream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                //Mensagem não enviada
            }
        }


  

        private void Broadcast_lista_clientes()
        {
            string mensagem = "|2,";
            foreach (DictionaryEntry cliente in lista_clientes)
            {
                mensagem = mensagem + "/" + cliente.Value + "/";
            }
            object message = (object)mensagem;
            broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
            try
            {
                broadcast.Start(message);
            }
            catch
            {
                txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                broadcast.Join();
                broadcast.Abort();
            }
        }


        private void Broadcast_update_lista_clientes(string accao, string nick)
        {
            if (accao == "entrar")
            {
                object message = (object)"|3.1," + nick;
                broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                try
                {
                    broadcast.Start(message);
                }
                catch
                {
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                    broadcast.Join();
                    broadcast.Abort();
                }
                
            }
            if (accao == "sair")
            {
                object message = (object)"|3.2," + nick;
                broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                try
                {
                    broadcast.Start(message);
                }
                catch
                {
                    txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                    broadcast.Join();
                    broadcast.Abort();
                }
            }

        }



        private void Broadcast_mensagem(object mens)
        {
            string mensagem = (string)mens;

            try
            {
                foreach (DictionaryEntry client in lista_clientes)
                {
                    Enviar_mensagem(mensagem, client.Key);
                }
            }
            catch
            {
                Thread.Sleep(50);
                Broadcast_mensagem(mens);
            }
        }

        private void Verificar_ligacoes_Tick(object sender, EventArgs e)
        {
            Hashtable clientes_remover = new Hashtable();

            try
            {
                foreach (DictionaryEntry cliente in lista_clientes)
                {
                    TcpClient ligacao = (TcpClient)cliente.Key;
                    //verificar se o cliente está activo
                    try
                    {
                        NetworkStream clientStream = ligacao.GetStream();
                        /*ASCIIEncoding encoder = new ASCIIEncoding();
                        byte[] buffer = encoder.GetBytes("|0,c");
                        clientStream.WriteTimeout = 500;
                        clientStream.Write(buffer, 0, buffer.Length);*/
                        clientStream.Flush();
                    }
                    catch
                    {
                        //não foi possivel ligar, adicionar aos clientes a remover
                        clientes_remover.Add(cliente.Key, cliente.Value);
                    }
                }

                //remover os clientes que estejam na lista
                if (clientes_remover.Count > 0)
                {
                    if (broadcast.IsAlive)
                    {
                        broadcast.Join();
                    }
                    foreach (DictionaryEntry cliente in clientes_remover)
                    {
                        string nick = (string)cliente.Value;
                        lista_clientes.Remove(cliente.Key);
                        object message = (object)"|3," + nick + " bazou!";
                        broadcast = new Thread(new ParameterizedThreadStart(Broadcast_mensagem));
                        try
                        {
                            broadcast.Start(message);
                        }
                        catch
                        {
                            txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                    new object[] { "Broadcast perdido!", Origem.Erro });
                            broadcast.Join();
                            broadcast.Abort();
                        }
                        txt_status.Invoke(new UpdateStatusCallback(this.UpdateStatus),
                        new object[] { nick + " bazou.", Origem.Saida });
                        Broadcast_update_lista_clientes("sair", nick);
                    }
                    clientes_remover.Clear();
                }
            }
            catch 
            { 
                // nada a fazer, salta-se esta verificacao
            }
            
        }

        private void Enviar_lista(TcpClient cliente)
        {
            //Enviar a lista de clientes inicial
            string mensagem_lista = "|2,";

            try
            {
                foreach (DictionaryEntry cli in lista_clientes)
                {
                    mensagem_lista = mensagem_lista + "/" + cli.Value + "/";
                }
                Enviar_mensagem(mensagem_lista, cliente);
            }
            catch
            {
                Thread.Sleep(50);
                foreach (DictionaryEntry cli in lista_clientes)
                {
                    mensagem_lista = mensagem_lista + "/" + cli.Value + "/";
                }
                Enviar_mensagem(mensagem_lista, cliente);
            }
        }

        private void n_clientes_Tick(object sender, EventArgs e)
        {
            lbl_n_clientes.Text = lista_clientes.Count.ToString();
        }

        public void Application_ApplicationExit(object sender, EventArgs e)
        {
            listenThread.Abort();
            clientThread.Abort();
            processar.Abort();
            tcpListener.Stop();
            Environment.Exit(0);
        }


    }
}
