using System;
using System.Collections.Generic;

namespace Furesoft.Rpc.Mmf
{
    public class RpcEventRepository
    {
        private static Dictionary<string, RpcEvent> events = new Dictionary<string, RpcEvent>();

        public static RpcEvent Get(string name)
        {
            if(events.ContainsKey(name))
            {
                return events[name];
            }
            else
            {
                events.Add(name, new RpcEvent(null, name));

                return events[name];
            }
        }
    }
}