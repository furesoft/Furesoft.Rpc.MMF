using Furesoft.Rpc.Mmf.Communicator;
using Furesoft.Rpc.Mmf.Messages;
using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf
{
    [Serializable]
    public class RpcEvent
    {
        public string Name { get; set; }

        private MemoryMappedFileCommunicator communicator;

        public static RpcEvent Register(string name)
        {
            return new RpcEvent(name);
        }

        internal RpcEvent(string name)
        {
            Name = name;
            communicator = new MemoryMappedFileCommunicator(Name + ".event", 5000);
            communicator.WritePosition = 0;
            communicator.ReadPosition = 0;
            communicator.DataReceived += Communicator_DataReceived;
        }

        private void Communicator_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
        {
            var r = (RpcEventCallMessage)RpcServices.Deserialize(e.Data);

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

            communicator.Write(RpcServices.Serialize(msg));
        }

        public void Start()
        {
            communicator.StartReader();
        }
    }
}