namespace Furesoft.Rpc.Mmf
{
    public static class Extensions
    {
        public static string RemoveLast(this string target, int count)
        {
            return target.Remove(target.Length - count);
        }
    }
}