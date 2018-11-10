using Furesoft.Rpc.Mmf.Messages;
using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf
{
    public class RpcEvent
    {
        public string Name { get; set; }

        private IRpcChannel communicator;

        public static RpcEvent Register(string name, IRpcChannel channel)
        {
            return new RpcEvent(name, channel);
        }

        internal RpcEvent(string name, IRpcChannel channel)
        {
            Name = name;
            communicator = channel;
            communicator.MessageRecieved += Communicator_DataReceived;
        }

        private void Communicator_DataReceived(byte[] data)
        {
            var r = (RpcEventCallMessage)RpcServices.Deserialize(data);

            if(r.Name == Name)
            {
                foreach (var h in handlers)
                {
                    h(r.Args[0], (EventArgs)r.Args[1]);
                }
            }
        }

        private List<EventHandler> handlers = new List<EventHandler>();

        public static RpcEvent operator +(RpcEvent e, EventHandler handler)
        {
            e.handlers.Add(handler);

            return e;
        }
        public static RpcEvent operator -(RpcEvent e, EventHandler handler)
        {
            e.handlers.Remove(handler);

            return e;
        }

        public Action this[object sender, EventArgs e]
        {
            get
            {
                Invoke(sender, e);
                return new Action(() => { });
            }
        }

        public void Invoke(object sender, EventArgs e)
        {
            var msg = new RpcEventCallMessage
            {
                Name = Name,
                Args = new List<object> { sender, e }
            };

            communicator.SendMessage(RpcServices.Serialize(msg));
        }

        public void Start()
        {
            communicator.Start();
        }
    }
}