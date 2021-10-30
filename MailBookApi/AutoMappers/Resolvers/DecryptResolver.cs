using System;
using System.Text;
using AutoMapper;
using System.Security.Cryptography;
using System.IO;
using MailBookApi.Configure;

namespace MailBookApi.AutoMappers.Resolves
{
    public class DecryptResolver : IValueResolver<object, object, int>
    {
        protected virtual string propertyName { get; }

        private byte[] key;
        private byte[] iv;

        public DecryptResolver(SecurityKeyConfigureOption securityKey)
        {
            this.key = Encoding.UTF8.GetBytes(securityKey.Key);
            this.iv = Encoding.UTF8.GetBytes(securityKey.IV);
        }

        public int Resolve(object source, object destination, int destMember, ResolutionContext context)
        {
            var encryptedIdText = source.GetType().GetProperty(this.propertyName).GetValue(source).ToString();
            var aes = Aes.Create();

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;

            var encryptedBytes = Convert.FromBase64String(encryptedIdText);
            var decryptor = aes.CreateDecryptor(key, iv);
            var ms = new MemoryStream(encryptedBytes);
            var clearIdText = string.Empty;

            using (var cryptoStream2 = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            {
                using (var sw2 = new StreamReader(cryptoStream2))
                {
                    clearIdText = sw2.ReadToEnd();
                }
            }

            var clearIdValue = clearIdText.Split('_')[0];
            var finalIdValue = Convert.ToInt32(clearIdValue);

            return finalIdValue;
        }
    }
}