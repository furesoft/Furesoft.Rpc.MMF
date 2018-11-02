using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf
{
    public class RpcEvent
    {
        public string Name { get; set; }

        public static RpcEvent Register(RpcServer server, string name)
        {
            return new RpcEvent(server, name);
        }

        internal RpcEvent(RpcServer s, string name)
        {
            Name = name;
            this.s = s;
        }

        private List<EventHandler> handlers = new List<EventHandler>();
        private readonly RpcServer s;

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

        public void Invoke(object sender, EventArgs e)
        {
            s?.CallEvent(Name, sender, e);
        }
    }
}