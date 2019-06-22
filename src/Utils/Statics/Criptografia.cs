using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MPSC.PlenoSoft.Core.Utils.Statics
{
	public static class Criptografia
	{
		private static readonly Encoding _encoding = Encoding.UTF8;
		private const String cKey = "p2eS9hDBNZCxmyOdeGyDgQ==";
		private const String cIV = "IVJjFZBCW9w=";

		public static String Encripta(this String mensagem)
		{
			return Encripta(mensagem, cKey, cIV);
		}

		public static String Decripta(this String mensagem)
		{
			return Decripta(mensagem, cKey, cIV);
		}

		public static String Criptografar(this String mensagem)
		{
			var stringBuilder = new StringBuilder();
			var md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			var bytes = md5CryptoServiceProvider.ComputeHash(_encoding.GetBytes(mensagem));

			foreach (var vByte in bytes)
				stringBuilder.Append(vByte.ToString("x2"));

			return stringBuilder.ToString().ToLower();
		}

		public static String Encripta(this String mensagem, String key, String vetor)
		{
			var bytes = _encoding.GetBytes(mensagem);
			var rc2CryptoServiceProvider = new RC2CryptoServiceProvider();
			var iCryptoTransform = rc2CryptoServiceProvider.CreateEncryptor(Convert.FromBase64String(key), Convert.FromBase64String(vetor));
			var memoryStream = new MemoryStream();

			var cryptoStream = new CryptoStream(memoryStream, iCryptoTransform, CryptoStreamMode.Write);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			cryptoStream.Close();

			return Convert.ToBase64String(memoryStream.ToArray());
		}

		public static String Decripta(this String mensagem, String key, String vetor)
		{
			var buffer = new Byte[256];
			var stringBuilder = new StringBuilder();
			var rc2CryptoServiceProvider = new RC2CryptoServiceProvider();
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