using Furesoft.Rpc.Mmf;
using Furesoft.Rpc.Mmf.InformationApi.Attributes;
using System;
using System.ComponentModel;

namespace Interface
{
    [Description("Test Interface")]
    public interface IMath
    {
        [Description("Raised when Index changed")]
        RpcEvent OnIndexChanged { get; set; }

        [Description("Get/Set Value based on index")]
        int this[int index] { get; set; }

        [Description("sum two values")]
        int Add(int x, int y);

        [Description("Adds a Point to the given coordinates")]
        [ThrowsException(typeof(ArgumentOutOfRangeException))]
        Point AddPosition(int x, int y);

        [ThrowsException(typeof(ArgumentNullException))]
        object MethodWithException();

        [Description("Translate the given Point into negative value")]
        Point TranslatePoint(Point input);
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