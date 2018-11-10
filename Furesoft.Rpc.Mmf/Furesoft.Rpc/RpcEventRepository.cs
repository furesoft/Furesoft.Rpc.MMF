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
                //ToDo: renew event system to call on server and recieve on client
                events.Add(name, new RpcEvent(name));

                return events[name];
            }
        }
    }
}