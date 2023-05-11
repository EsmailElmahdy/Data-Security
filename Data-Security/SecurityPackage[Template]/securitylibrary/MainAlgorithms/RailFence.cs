using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            string x;
            //throw new NotImplementedException();
            if (plainText == cipherText)
            {
                return 1;
            }
            else
            {
                for (int i = 2; i <= plainText.Length; i++)
                {
                    x = Encrypt(plainText, i);
                    if (x == cipherText)
                    {
                        return i;
                    }
                }
            }
            return 0;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            char[] answer = new char[cipherText.Length];
            string plainText = "";
            int Length = (int)Math.Ceiling(Convert.ToDouble(cipherText.Length) / key);
            for (int i = 0; i < Length; i++)
            {
                for (int j = i; j < cipherText.Length; j += Length)
                {
                    plainText += cipherText[j];
                }
            }
            plainText = plainText.ToUpper();
            return plainText;


        }

        public string Encrypt(string plainText, int key)
        {
            char[] answer = new char[plainText.Length];
            string cipherText = "";
            for (int i = 0; i < key; i++)
            {
                for (int j = i; j < plainText.Length; j += key)
                {
                    cipherText += plainText[j];
                }
            }
            cipherText = cipherText.ToUpper();
            return cipherText;
        }
    }
}

