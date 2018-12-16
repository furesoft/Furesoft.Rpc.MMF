using System;

namespace Furesoft.Rpc.Mmf.Messages
{
    [Serializable]
    public class RpcExceptionMessage : RpcMessage
    {
        public RpcExceptionMessage(string @interface, string name, string message)
        {
            Interface = @interface;
            Name = name;
            Message = message;
        }

        public RpcExceptionMessage()
        {

        }

        public string Message { get; set; }
    }
}