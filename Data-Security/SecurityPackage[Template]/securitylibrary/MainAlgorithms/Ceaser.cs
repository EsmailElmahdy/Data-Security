using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        char[] alphabet = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public string Encrypt(string plainText, int key)
        {
            char[] answer = new char[plainText.Length];
            // char[] arr_plainText=plainText.ToCharArray();

            //bool check = false;
            int index = 0;
            plainText = plainText.ToLower();
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == alphabet[j])
                    {
                        index = (j + key) % 26;
                        break;
                    }
                }
                answer[i] = (char)(answer[i] + alphabet[index]);
            }
            // string cipherText = answer.ToString().ToUpper();
            string cipherText = "";
            for (int i = 0; i < answer.Length; i++)
            {
                cipherText += answer[i];
            }
            cipherText = cipherText.ToUpper();
            return cipherText;
            // throw new NotImplementedException();

        }
        public string Decrypt(string cipherText, int key)
        {
            
            string ans = "";
            int index = 0;
            cipherText = cipherText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (cipherText[i] == alphabet[j])
                    {
                        index = j - key;
                        if (index < 0)
                            index += 26;

                      
                        ans += alphabet[index];
                        break;
                    }
                    //   throw new NotImplementedException();
                }
            }
            return ans;
        }
        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            int ans = 0;
            if (plainText[0] == cipherText[0])
                ans = 0;
            else if (plainText[0] < cipherText[0])
            {
                ans = cipherText[0] - plainText[0];
            }
            else
            {
                int x = cipherText[0] + 26;
                ans = x - plainText[0];
            }
            return ans;
            //throw new NotImplementedException();
        }
      
    }
}