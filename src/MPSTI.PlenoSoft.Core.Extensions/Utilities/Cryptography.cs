using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MPSTI.PlenoSoft.Core.Extensions.Utilities
{
    public static class Cryptography
	{
        private static readonly Encoding _encoding = Encoding.UTF8;
        private const string cKey = "p2eS9hDBNZCxmyOdeGyDgQ==";
        private const string cIV = "IVJjFZBCW9w=";

        public static string Encripta(this string mensagem)
        {
            return mensagem.Encripta(cKey, cIV);
        }

        public static string Decripta(this string mensagem)
        {
            return mensagem.Decripta(cKey, cIV);
        }

        public static string Criptografar(this string mensagem)
        {
            var stringBuilder = new StringBuilder();
            var md5CryptoServiceProvider = MD5.Create();
            var bytes = md5CryptoServiceProvider.ComputeHash(_encoding.GetBytes(mensagem));

            foreach (var vByte in bytes)
                stringBuilder.Append(vByte.ToString("x2"));

            return stringBuilder.ToString().ToLower();
        }

        public static string Encripta(this string mensagem, string key, string vetor)
        {
            var bytes = _encoding.GetBytes(mensagem);
            var rc2CryptoServiceProvider = RC2.Create();
            var iCryptoTransform = rc2CryptoServiceProvider.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(vetor));
            var memoryStream = new MemoryStream();

            var cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();

            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public static string Decripta(this string mensagem, string key, string vetor)
        {
            var buffer = new byte[256];
            var stringBuilder = new StringBuilder();
            var rc2CryptoServiceProvider = RC2.Create();
            var iCryptoTransform = rc2CryptoServiceProvider.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(vetor));
            var memoryStream = new MemoryStream(Convert.FromBase64String(mensagem));

            var cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Read);
            var bytesLidos = cryptoStream.Read(buffer, 0, buffer.Length);
            while (bytesLidos == buffer.Length)
            {
                stringBuilder.Append(_encoding.GetString(buffer, 0, bytesLidos));
                bytesLidos = cryptoStream.Read(buffer, 0, buffer.Length);
            }
            cryptoStream.Close();

            return stringBuilder.ToString();
        }
    }
}