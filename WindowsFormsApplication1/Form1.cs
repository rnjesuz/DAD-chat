using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace Chat
{
    public partial class Form1 : Form
    {

        ServerInterface server;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int port = int.Parse(textBox1.Text);
            TcpChannel channel = new TcpChannel(port);
            ChannelServices.RegisterChannel(channel, false);

            RemoteClient rc = new RemoteClient();
            RemotingServices.Marshal(rc, "ChatClient", typeof(RemoteClient));

            server = (ServerInterface)Activator.GetObject(
                typeof(ServerInterface),
                "tcp://localhost:8086/ChatServer");


            try
            {
                server.Connect(port);
            }
            catch (SocketException)
            {
                System.Console.WriteLine("Could not locate server");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Send_Click(object sender, EventArgs e)
        {
            if (server == null)
                return;
            try
            {
                server.SendMessage(textBox2.Text);
                UpdateChat(textBox2.Text);
            }
            catch (SocketException)
            {
                System.Console.WriteLine("Could not locate server");
            }
        }
    public void UpdateChat(string msg)
        {
            textBox3.Text += msg + "\r\n";
        }
    }

    delegate void DelegateMsg(string message);

    public class RemoteClient : MarshalByRefObject, ClientInterface
    {
        public static Form1 form = Client.form;
        public void ReceiveMessage(string message)
        {
            //form.Invoke(new DelegateMsg(form.UpdateChat), message);
            form.UpdateChat(message);
        }
    }
}
