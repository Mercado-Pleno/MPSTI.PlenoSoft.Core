using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace MPSTI.PlenoSoft.Core.CrossProject.Security
{
	public class Crypto
	{
		private const char Splitter = '$';
		private const string Algorithm = "Rfc2898DeriveBytes";

		public static string CriarHash(string password)
		{
			var crypto = new Crypto();
			var hash = crypto.GetSHA2_512Str(password);
			return crypto.Compute(hash);
		}

		public static bool VerificarHash(string password, string passwordHashString, bool throwsExceptionOnError = true)
		{
			var crypto = new Crypto();
			var hash = crypto.GetSHA2_512Str(password);
			return crypto.Verify(hash, passwordHashString, throwsExceptionOnError);
		}

		public TimeSpan Estimate(string password, int iterations)
		{
			var watch = new Stopwatch();
			watch.Start();
			Compute(password, iterations);
			watch.Stop();
			return watch.Elapsed;
		}

		public string Compute(string password, int iterations = 40000, int saltSize = 16, int hashSize = 32)
		{
			using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltSize, iterations);
			var hash = rfc2898DeriveBytes.GetBytes(hashSize);
			return CreateHashString(hash, rfc2898DeriveBytes.Salt, iterations);
		}

		public bool Verify(string password, string passwordHashString, bool throwsExceptionOnError)
		{
			try
			{
				var parameters = new SimpleHashParameters(passwordHashString);
				var hashSize = Convert.FromBase64String(parameters.PasswordHash).Length;
				var newPasswordHash = ComputeHash(password, parameters.Salt, parameters.Iterations, hashSize);
				return parameters.PasswordHash == newPasswordHash;
			}
			catch
			{
				if (throwsExceptionOnError) throw;
			}
			return false;
		}

		private string CreateHashString(byte[] hash, byte[] salt, int iterations)
		{
			var i = iterations.ToString(CultureInfo.InvariantCulture);
			var saltString = Convert.ToBase64String(salt);
			var hashStringPart = Convert.ToBase64String(hash);
			return string.Join(Splitter, Algorithm, i, saltString, hashStringPart);
		}

		private string ComputeHash(string password, string salt, int iterations, int hashSize)
		{
			var saltBytes = Convert.FromBase64String(salt);
			using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, iterations);
			var hash = rfc2898DeriveBytes.GetBytes(hashSize);
			return Convert.ToBase64String(hash);
		}

		private string GetSHA2_512Str(string source)
		{
			var hashBytes = GetSHA2_512Hash(source);
			return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
		}

		private byte[] GetSHA2_512Hash(string source)
		{
			using var sha512Hash = SHA512.Create();
			var sourceBytes = Encoding.UTF8.GetBytes(source);
			return sha512Hash.ComputeHash(sourceBytes);
		}

		private class SimpleHashParameters
		{
			internal string Algorithm { get; private set; }
			internal int Iterations { get; private set; }
			internal string Salt { get; private set; }
			internal string PasswordHash { get; private set; }

			internal SimpleHashParameters(string passwordHashString)
			{
				var parameters = ParseParameters(passwordHashString);
				ProcessParameters(parameters);
			}

			private string[] ParseParameters(string passwordHashString)
			{
				var parameters = passwordHashString.Split(Splitter);

				if (parameters.Length != 4)
					throw new ArgumentException("Invalid password hash string format", nameof(passwordHashString));
				return parameters;
			}

			private void ProcessParameters(string[] parameters)
			{
				Algorithm = parameters[0];
				Iterations = int.Parse(parameters[1]);
				Salt = parameters[2];
				PasswordHash = parameters[3];
			}
		}
	}
}