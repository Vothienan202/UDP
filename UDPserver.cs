using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public partial class UDPserver : Form
    {
        UdpClient server;
        IPEndPoint endPoint;
        public UDPserver()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UDPclient client = new UDPclient();
            client.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server = new UdpClient(int.Parse(txtPort.Text));
            endPoint = new IPEndPoint(IPAddress.Any, 0);

            Thread thr = new Thread(new ThreadStart(ServerStart));
            thr.Start();
            btnStartServer.Enabled = false;
            btnNewClient.Enabled = true;

            WriteLog("Action : Server Started Successfully...");
        }

        private void ServerStart()
        {
            while (true)
            {
                byte[] buffer = server.Receive(ref endPoint);

                string[] msg = Encoding.Unicode.GetString(buffer).Split('.');
                int clientPort = int.Parse(msg[0]);
                string clientHostName = msg[1];
                string request = msg[2];

                WriteLog(string.Format("Client at Port: {0} at: {1} message {2}.", clientPort, clientHostName, request));

                string response = string.Format("client send message to server at: {0}", DateTime.Now.ToLocalTime());

                buffer = Encoding.Unicode.GetBytes(response);
                server.Send(buffer, buffer.Length, clientHostName, clientPort);
            }
        }

        private void WriteLog(string msg)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                txtLog.Text += string.Format("{0}.{1}", msg, Environment.NewLine);
            });

            this.BeginInvoke(invoker);
        }
    }
}
