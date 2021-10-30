using System;
using System.Text;
using AutoMapper;
using System.Security.Cryptography;
using System.IO;
using MailBookApi.Configure;

namespace MailBookApi.AutoMappers.Resolves
{
    public class EncryptResolver : IValueResolver<object, object, string>
    {
        protected virtual string propertyName { get; }

        private byte[] key;
        private byte[] iv;

        public EncryptResolver(SecurityKeyConfigureOption securityKey)
        {
            this.key = Encoding.UTF8.GetBytes(securityKey.Key);
            this.iv = Encoding.UTF8.GetBytes(securityKey.IV);
        }

        public string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            var idPropertyInfo = source.GetType().GetProperty(propertyName);
            var idPropertyValue = idPropertyInfo.GetValue(source).ToString();
            var idConcatenate = $"{idPropertyValue}_{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            var idConcatenateInBytes = Encoding.UTF8.GetBytes(idConcatenate);
            var encryptedBytes = default(byte[]);
            var aes = Aes.Create();

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;
            
            var encryptor = aes.CreateEncryptor(key, iv);
            var ms = new MemoryStream();

            using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (var sw = new StreamWriter(cryptoStream))
                {
                    sw.Write(idConcatenate);
                    sw.Flush();

                    encryptedBytes = ms.ToArray();
                }
            }

            var encryptedText = Convert.ToBase64String(encryptedBytes);

            return encryptedText;
        }
    }
}