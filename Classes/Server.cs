﻿using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace TCPServer
{
    public class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 3000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
            
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
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
                System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));


                MainWindow.myWindow.Dispatcher.BeginInvoke(new Action(delegate() { MainWindow.myMainWindow.textBox1.Text = "some text"; }));
              
                
                
                clientStream = tcpClient.GetStream();
                encoder = new ASCIIEncoding();
                byte[] buffer = encoder.GetBytes("Servidor -> Cliente: Hello Client!");

                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }

            tcpClient.Close();
        }


    }
}
