using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            string Key = "";
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            string Letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < cipherText.Length; i++)
            {
                int Pos = (Letters.IndexOf((cipherText[i])) - Letters.IndexOf((plainText[i])));
                Pos = ((Pos % 26) + 26) % 26;
                Key += Letters[Pos];
            }

            int Temp = 0;
            for (int i = 0; i < Key.Length; i++)
            {
                if (plainText[0] == Key[i])
                {
                    Temp = i;
                    break;
                }
            }
            string Newkey = "";
            for (int i = 0; i < Temp; i++)
            {
                Newkey += Key[i];
            }
            return Newkey;

        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            string Plain = "";
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string Letters = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < key.Length; i++)
            {
                int Pos = (Letters.IndexOf((cipherText[i])) - Letters.IndexOf((key[i])));
                Pos = ((Pos % 26) + 26) % 26;
                Plain += Letters[Pos];
            }
            int C = 0;
            for (int i = key.Length; i < cipherText.Length; i++)
            {
                int Pos = (Letters.IndexOf((cipherText[i])) - Letters.IndexOf((Plain[C])));
                C++;
                Pos = ((Pos % 26) + 26) % 26;
                Plain += Letters[Pos];
            }

            return Plain;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            plainText = plainText.ToLower();
            key = key.ToLower();
            string Cipher = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            int Counter = 0;
            while (key.Length < plainText.Length)
            {
                key += plainText[Counter];
                Counter++;
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                int Pos = (letters.IndexOf((plainText[i])) + letters.IndexOf((key[i]))) % 26;
                Cipher += letters[Pos];
            }

            return Cipher;
        }
    }
}