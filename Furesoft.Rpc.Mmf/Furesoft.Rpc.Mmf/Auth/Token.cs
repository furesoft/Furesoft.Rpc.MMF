using System;
using System.Linq;

namespace Furesoft.Rpc.Mmf.Auth
{
    public class Token
    {
        public Guid AppID { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExpired { get; set; }

        public static Token Create(Guid id)
        {
            var t = new Token()
            {
                AppID = id,
                CreatedAt = DateTime.Now
            };

            return t;
        }

        public bool Validate(TimeSpan expireTime)
        {
            IsExpired = CreatedAt < DateTime.Now.Add(expireTime);

            return IsExpired;
        }

        public override string ToString()
        {
            byte[] time = BitConverter.GetBytes(CreatedAt.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            byte[] id = AppID.ToByteArray();

            byte[] raw = id.Concat(time).Concat(key).ToArray();

            return Convert.ToBase64String(raw);
        }

        public static Token Parse(string token)
        {
            var t = new Token();
            var raw = Convert.FromBase64String(token);

            t.AppID = new Guid(raw.Take(16).ToArray());
            t.CreatedAt = DateTime.FromBinary(BitConverter.ToInt64(raw, 0));

            return t;
        }
    }
}