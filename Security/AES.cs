using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DroidLord.Core;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace DroidLord.Security
{
    // AES/CBC/PKCS7
    public class AES : ICipher
    {
        private byte[] InitialVector;
        private byte[] Key;

        public static string GenerateKey()
        {
            var aes = RijndaelManaged.Create();
            aes.BlockSize = 256;
            aes.KeySize = 256;
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;
            aes.GenerateKey();
            aes.GenerateIV();
            var key = Convert.ToBase64String(aes.Key);
            var iv = Convert.ToBase64String(aes.IV);
            return key + ":" + iv;
        }

        public string Decrypt(string data)
        {
            var buffer = Convert.FromBase64String(data);
            var aes = RijndaelManaged.Create();
            aes.BlockSize = 256;            
            aes.KeySize = 256;
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;                        
            using (var ms = new MemoryStream(buffer))
            {
                using (var encoder = new CryptoStream(ms, aes.CreateDecryptor(Key.Clone() as byte[], InitialVector.Clone() as byte[]), CryptoStreamMode.Read))
                {
                    using (var raw = new MemoryStream())
                    {
                        var readLen = 0;
                        var readBuf = new byte[102400];

                        while ((readLen = encoder.Read(readBuf, 0, 102400)) != 0)
                        {
                            raw.Write(readBuf, 0, readLen);
                        }
                        var decBuffer = raw.ToArray();
                        return Encoding.UTF8.GetString(decBuffer).Trim('\0');
                    }
                }
            }
        }

        public string Encrypt(string data)
        {
            var buffer = Encoding.UTF8.GetBytes(data);  
            var aes = RijndaelManaged.Create();
            aes.BlockSize = 256;
            aes.KeySize = 256;
            aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;
            using (var ms = new MemoryStream())
            {
                using (var encoder = new CryptoStream(ms, aes.CreateEncryptor(Key.Clone() as byte[], InitialVector.Clone() as byte[]), CryptoStreamMode.Write))
                {
                    encoder.Write(buffer, 0, buffer.Length);
                    encoder.FlushFinalBlock();
                }
                var encBuffer = ms.ToArray();
                return Convert.ToBase64String(encBuffer);               
            }
        }

        public bool Init(string key)
        {
            var group = key.Split(':');
            if (group.Length != 2)
            {
                return false;
            }
            Key = Convert.FromBase64String(group[0]);
            InitialVector = Convert.FromBase64String(group[1]);
            return true;
        }
    }
}
