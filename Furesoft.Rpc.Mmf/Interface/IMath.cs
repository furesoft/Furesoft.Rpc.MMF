using Furesoft.Rpc.Mmf;
using Furesoft.Rpc.Mmf.Auth;
using Furesoft.Rpc.Mmf.InformationApi.Attributes;
using System;
using System.ComponentModel;

namespace Interface
{
    [Description("Test Interface")]
    public interface IMath
    {
        [Description("sum two values")]
        int Add(int x, int y);

        [Auth]
        [ThrowsException(typeof(ArgumentNullException))]
        object MethodWithException();

        [Description("Adds a Point to the given coordinates")]
        [ThrowsException(typeof(ArgumentOutOfRangeException))]
        Point AddPosition(int x, int y);

        [Description("Get/Set Value based on index")]
        int this[int index] { get; set; }

        [Description("Raised when Index changed")]
        RpcEvent OnIndexChanged { get; set; }
    }

    [Description("2D Point Coordinate")]
    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int GetX()
        {
            return X;
        }
    }
}