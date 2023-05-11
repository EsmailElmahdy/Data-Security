using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            string key = "";
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();
            Dictionary<char, int> map = new Dictionary<char, int>();
            char cr = 'a';
            for (char c = 'a'; c <= 'z'; c++)
            {
                map[c] = 0;
                //  Console.Write(c);
            }
            //Console.WriteLine();
            char[] arr = new char[26];
            for (int i = 0; i < plainText.Length; i++)
            {
                arr[plainText[i] - 'a'] = cipherText[i];
                map[cipherText[i]] = 1;
            }
            cr = 'a';

            for (int i = 0; i < 26; i++)
            {
                if (arr[i] > 'z' || arr[i] < 'a')
                {
                    if (map[cr] == 0)
                    {
                        arr[i] = cr;
                        map[cr]++;
                        cr++;
                    }
                    else
                    {
                        while (true)
                        {
                            cr++;
                            if (map[cr] == 0)
                            {
                                arr[i] = cr;
                                map[cr]++;
                                cr++;
                                break;
                            }
                        }
                    }
                }
                key += arr[i];
            }

            Console.WriteLine(key);
            return key;
            //throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, string key)
        {
            // throw new NotImplementedException();
            string answer = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            for (int i = 0; i < cipherText.Length; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (cipherText[i] == key[j])
                    {
                        answer += chars[j];
                        break;
                    }
                }
            }
            return answer;
            //throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            // throw new NotImplementedException();
            string answer = "";
            string chars = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < plainText.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (plainText[i] == chars[j])
                    {
                        answer += key[j];
                        break;
                    }
                }
            }
            return answer;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
      
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            string freq = "zqjxkvbywgpfmucdlhrsnioate";            
            freq = freq.ToLower();
            cipher = cipher.ToLower();
            string k = "";
            Dictionary<char, int> map = new Dictionary<char, int>();
            Dictionary<char, char> dic = new Dictionary<char, char>();
            for (char c = 'a'; c <= 'z'; c++) map[c] = 0;
            foreach (char c in cipher) map[c]++;
            foreach (var c in map.OrderBy(x => x.Value))
            {
                k += c.Key;
            }
           /* char[] chars = new char[26];
            for(int i = 0; i < 26; i ++ )
            {

                chars[(int)(freq[i] - 'a')] = k[i];
            }*/
           for(int i = 0; i < 26; i ++)
            {
                dic[k[i]] = freq[i];
            }
            string ans = "";
            foreach(char c in cipher)
            {
                ans+=dic[c];
            }
            return ans;
            //throw new NotImplementedException();
        }
    }
}