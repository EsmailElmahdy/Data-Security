using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SecurityLibrary.RC4
{
	/// <summary>
	/// If the string starts with 0x.... then it's Hexadecimal not string
	/// </summary>
	public class RC4 : CryptographicTechnique
	{
		public override string Decrypt(string cipherText, string key)
		{
			bool found = false;
			if (cipherText[0] == '0' && cipherText[1] == 'x')
			{
				found = true;
				string Temp = "";
				for (int i = 2; i < cipherText.Length; i += 2)
				{
					Temp += char.ConvertFromUtf32(Convert.ToInt32(cipherText.Substring(i, 2), 16));
				}
				cipherText = Temp;
			}

			if (key[0] == '0' && key[1] == 'x')
			{
				key = key.Substring(2); // remove "0x" prefix
				byte[] KeyArr = new byte[key.Length / 2];
				for (int i = 0; i < KeyArr.Length; i++)
				{
					KeyArr[i] = Convert.ToByte(key.Substring(i * 2, 2), 16);
				}
				key = Encoding.UTF8.GetString(KeyArr);
			}


			int[] S = Enumerable.Range(0, 256).ToArray();

			int[] T = Enumerable.Range(0, 256)
								.Select(i => (int)key[i % key.Length])
								.ToArray();

			int j = 0;
			Enumerable.Range(0, 256).ToList()
					  .ForEach(i =>
					  {
						  j = (j + S[i] + T[i]) % 256;
						  (S[i], S[j]) = (S[j], S[i]);
					  });

			string FinalText = "";
			int L = 0;
			Enumerable.Range(0, cipherText.Length).ToList()
					  .ForEach(i =>
					  {
						  char Te = cipherText[i];
						  int F = (i + 1) % 256;
						  L = (S[F] + L) % 256;
						  (S[F], S[L]) = (S[L], S[F]);
						  int K = S[(S[F] + S[L]) % 256];
						  FinalText += (char)(Te ^ K);
					  });

			if (found)
			{
				string hexString = string.Concat(FinalText.Select(c => ((int)c).ToString("x2")));
				FinalText = $"0x{hexString}";
			}

			return FinalText;


		}

		public override string Encrypt(string plainText, string key)
		{
			bool found = false;
			if (plainText[0] == '0' && plainText[1] == 'x')
			{
				found = true;
				byte[] byteArray = Enumerable.Range(2, plainText.Length - 2)
											  .Where(x => x % 2 == 0)
											  .Select(x => Convert.ToByte(plainText.Substring(x, 2), 16))
											  .ToArray();
				plainText = Encoding.UTF8.GetString(byteArray);
			}
			if (key[0] == '0' && key[1] == 'x')
			{
				key = key.Substring(2);
				byte[] keyBytes = Enumerable.Range(0, key.Length)
											 .Where(x => x % 2 == 0)
											 .Select(x => Convert.ToByte(key.Substring(x, 2), 16))
											 .ToArray();
				key = Encoding.UTF8.GetString(keyBytes);
			}

			int[] S = Enumerable.Range(0, 256).ToArray();

			int[] T = Enumerable.Range(0, 256)
								.Select(i => (int)key[i % key.Length])
								.ToArray();

			int j = 0;
			Enumerable.Range(0, 256).ToList()
					  .ForEach(i =>
					  {
						  j = (j + S[i] + T[i]) % 256;
						  (S[i], S[j]) = (S[j], S[i]);
					  });

			string FinalText = "";
				int L = 0;
			Enumerable.Range(0, plainText.Length).ToList()
					  .ForEach(i =>
					  {
						  char Te = plainText[i];
						  int F = (i + 1) % 256;
						  L = (S[F] + L) % 256;
						  (S[F], S[L]) = (S[L], S[F]);
						  int K = S[(S[F] + S[L]) % 256];
						  FinalText += (char)(Te ^ K);
					  });

			if (found)
			{
				string hexString = string.Concat(FinalText.Select(c => ((int)c).ToString("x2")));
				FinalText = $"0x{hexString}";
			}

			return FinalText;

		}
	}
}