# Furesoft.Rpc.MMF
A RPC Framework for IPC with Memory Mapped Files

Example:
Interface:
```C#
public interface IMath {
     int Add(int x, int y);
     object MethodWithException();
     object this[int index] {get;set;}
}
```

Interface Implementation in Server:
```C#
public class MathImpl : IMath {
     public int Add(int x, int y) {
         return x + y;
     }
     
     public object MethodWithException() {
          return new Exception("Exception was trown");
     }
     
     int mul = 1;
     public int this[int index] {
          get {
               return index * mul;
          }
          set {
                mul = index + value;
          }
     }
}
```

Client:
```C#
using Furesoft.Rpc.MMF

public class Program {
      public static void Main() {
          var rpc = new RpcClient("ExampleChannel");
          rpc.Start();
          
          var math = rpc.Bind<IMath>();
          
          Console.WriteLine(math.Add(10, 5);
          math[15] = 22;
          
          Console.WriteLine(math[15]);
          
          math.MethodWithException();
      }
}
```

Server
```C#
using Furesoft.Rpc.MMF

public class Program {
      public static void Main() {
          var rpc = new RpcServer("ExampleChannel");
             
          rpc.Bind<IMath>(new MathImpl());
          rpc.Start();
      }
}
```
