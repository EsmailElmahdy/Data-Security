using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
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
            for (int i = 1; i < Key.Length; i++)
            {
                if (Key[0] == Key[i] && Key[1] == Key[i + 1] && Key[2] == Key[i + 2])
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
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string Plain = "";
            string letters = "abcdefghijklmnopqrstuvwxyz";
            int Counter = 0;

            while (key.Length < cipherText.Length)
            {
                key += key[Counter];
                Counter++;
            }

            for (int i = 0; i < cipherText.Length; i++)
            {
                int Pos = (letters.IndexOf((cipherText[i])) - letters.IndexOf((key[i]))) % 26;
                Pos = ((Pos % 26) + 26) % 26;
                Plain += letters[Pos];
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
                key += key[Counter];
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