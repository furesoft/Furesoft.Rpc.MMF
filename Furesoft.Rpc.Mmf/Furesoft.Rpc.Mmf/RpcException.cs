using System;
using System.Runtime.Serialization;

namespace Furesoft.Rpc.Mmf
{
    [Serializable]
    public class RpcException : Exception
    {
        public string Inteface { get; }
        public string Method { get; }

        public RpcException()
        {
        }

        public RpcException(string inteface, string method, Exception ex)
           : base($"Exception thrown at: {inteface} from {method}", ex)
        {
            Inteface = inteface;
            Method = method;
        }

        public RpcException(string message) : base(message)
        {
        }

        public RpcException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RpcException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}