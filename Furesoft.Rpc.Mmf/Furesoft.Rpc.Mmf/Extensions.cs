using System;
using System.Linq;

namespace Furesoft.Rpc.Mmf
{
    public static class Extensions
    {
        public static string RemoveLast(this string target, int count)
        {
            return target.Remove(target.Length - count);
        }

        public static string InsertLast(this string target, string input)
        {
            return target.Insert(target.Length, input);
        }

        public static bool IsSimpleType(this Type type)
        {
            return
                !type.IsValueType ||
                type.IsPrimitive ||
                new Type[] {
                typeof(String),
                typeof(RpcEvent),
                typeof(Decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}