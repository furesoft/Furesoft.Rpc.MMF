namespace Furesoft.Rpc.Mmf.Messages
{
    public class RpcExceptionMessage : RpcMethod
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