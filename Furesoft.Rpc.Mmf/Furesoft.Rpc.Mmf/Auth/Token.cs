using System;
using System.Collections.Generic;
using System.Linq;

namespace Furesoft.Rpc.Mmf.Auth
{
    public class Token
    {
        public static TokenValidation ValidateToken(string reason, string token, Guid SecurityStamp, Guid Id)
        {
            var result = new TokenValidation();
            byte[] data = Convert.FromBase64String(token);
            byte[] _time = data.Take(8).ToArray();
            byte[] _key = data.Skip(8).Take(16).ToArray();
            byte[] _reason = data.Skip(24).Take(4).ToArray();
            byte[] _Id = data.Skip(28).ToArray();

            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(_time, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                result.Errors.Add(TokenValidationStatus.Expired);
            }

            Guid gKey = new Guid(_key);
            if (gKey.ToString() != SecurityStamp.ToString())
            {
                result.Errors.Add(TokenValidationStatus.WrongGuid);
            }

            if (reason != GetString(_reason))
            {
                result.Errors.Add(TokenValidationStatus.WrongPurpose);
            }

            if (Id.ToString() != GetString(_Id))
            {
                result.Errors.Add(TokenValidationStatus.WrongUser);
            }

            return result;
        }

        private static string GetString(byte[] data)
        {
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public static string GenerateToken(string reason, Guid SecurityStamp, Guid Id)
        {
            byte[] _time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] _key = SecurityStamp.ToByteArray();
            byte[] _Id = GetBytes(Id.ToString());
            byte[] _reason = GetBytes(reason);
            byte[] data = new byte[_time.Length + _key.Length + _reason.Length + _Id.Length];

            System.Buffer.BlockCopy(_time, 0, data, 0, _time.Length);
            System.Buffer.BlockCopy(_key, 0, data, _time.Length, _key.Length);
            System.Buffer.BlockCopy(_reason, 0, data, _time.Length + _key.Length, _reason.Length);
            System.Buffer.BlockCopy(_Id, 0, data, _time.Length + _key.Length + _reason.Length, _Id.Length);

            return Convert.ToBase64String(data.ToArray());
        }

        private static byte[] GetBytes(string raw)
        {
            return System.Text.Encoding.ASCII.GetBytes(raw);
        }

        public class TokenValidation
        {
            public bool Validated { get { return Errors.Count == 0; } }
            public readonly List<TokenValidationStatus> Errors = new List<TokenValidationStatus>();
        }

        public enum TokenValidationStatus
        {
            Expired,
            WrongUser,
            WrongPurpose,
            WrongGuid
        }
    }
}