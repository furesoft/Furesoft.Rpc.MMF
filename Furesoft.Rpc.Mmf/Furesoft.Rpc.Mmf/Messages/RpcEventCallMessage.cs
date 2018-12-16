﻿using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace Furesoft.Rpc.Mmf.Messages
{
    [ContentProperty("Arg")]
    [Serializable]
    public class RpcEventCallMessage : RpcMessage
    {
        public List<object> Args { get; set; } = new List<object>();
    }
}
