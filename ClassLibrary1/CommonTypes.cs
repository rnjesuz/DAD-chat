using System;
using System.Collections.Generic;
using System.Threading;

namespace Chat
{
    public interface ServerInterface
    {
        void Connect(int port);
        void SendMessage(string message);
        void BroadcastMessage();
    }
}

    public interface ClientInterface {
        void ReceiveMessage(string message);
}
