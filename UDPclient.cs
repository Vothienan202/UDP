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

namespace WindowsFormsApp1
{
    public partial class UDPclient : Form
    {
        UdpClient client;
        IPEndPoint endPoint;
        public UDPclient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            int serverPort = int.Parse(txtServerPort.Text);
            int clientPort = int.Parse(txtClientPort.Text);
            string hostName = txtHostName.Text;

            string msg = string.Format("{0}.{1}.{2}", clientPort, hostName, txtMessage.Text.Trim());
            byte[] buffer = Encoding.Unicode.GetBytes(msg);

            client.Send(buffer, buffer.Length, hostName, serverPort);

            endPoint = new IPEndPoint(IPAddress.Any, 0);
            buffer = client.Receive(ref endPoint);

            WriteLog(msg);
        }

        private void WriteLog(string msg)
        {
            MethodInvoker invoker = new MethodInvoker(delegate
            {
                txtLog.Text += string.Format("Sever Responed : {0}.{1}", msg, Environment.NewLine);
                txtMessage.Text = string.Empty;
            });

            this.BeginInvoke(invoker);
        }

        private void btnCreateClient_Click(object sender, EventArgs e)
        {
            int clientPort = int.Parse(txtClientPort.Text);
            client = new UdpClient(clientPort);

            btnCreateClient.Enabled = txtClientPort.Enabled = txtHostName.Enabled = txtServerPort.Enabled = false;
            btnSend.Enabled = true;
        }
    }
}
