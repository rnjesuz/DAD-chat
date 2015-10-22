using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;

namespace Chat
{
    class Server
    {
        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel(8086);
            ChannelServices.RegisterChannel(channel, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(RemoteServer),
                "ChatServer",
                WellKnownObjectMode.Singleton);

            System.Console.WriteLine("Press <enter> to terminate chat server...");
            System.Console.ReadLine();
        }
    }

    public class RemoteServer : MarshalByRefObject, ServerInterface
    {

        List<ClientInterface> clients;
        List<string> messages;

        RemoteServer()
        {
            clients = new List<ClientInterface>();
            messages = new List<string>();
        }

        public void Connect(int port)
        {
            ClientInterface newClient = (ClientInterface)Activator.GetObject(typeof(ClientInterface), "tcp://localhost:" + port + "/ChatClient");
            Console.WriteLine("antes de adicionar");
            clients.Add(newClient);
            Console.WriteLine("depois de adicionar");
        }

        public void SendMessage(string message)
        {
            messages.Add(message);
            ThreadStart ts = new ThreadStart(this.BroadcastMessage);
            Thread t = new Thread(ts);
            t.Start();
        }

        public void BroadcastMessage()
        {
            string msg;
            msg = messages[messages.Count - 1];
            foreach (ClientInterface rc in clients)
            {
                Console.WriteLine("fiz");
                try
                {
                    rc.ReceiveMessage(msg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e + "fodeu");
                    //clients.Remove(rc);
                }
            }
        }
    }
}
