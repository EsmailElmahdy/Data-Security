using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            List<int> key = new List<int>();
            cipherText = cipherText.ToLower();
            plainText = plainText.ToLower();
            if (plainText.Equals(cipherText))
            {
                key.Add(1);
                return key;
            }
            else
            {
                int Count = plainText.Length, rows = 1;
                Dictionary<int, string> dic = new Dictionary<int, string>();
                for (int i = 2; i < plainText.Length; i++)
                {
                    int col = i, row = (int)Math.Ceiling(Convert.ToDouble(cipherText.Length) / i);
                    if (!(col * row == cipherText.Length))
                    {
                        continue;
                    }

                    List<List<char>> arr = new List<List<char>>();
                    int c = 0;
                    for (int i2 = 0; i2 < row; i2++)
                    {
                        arr.Add(new List<char>());
                        for (int j = 0; j < col; j++)
                        {
                            if (c == plainText.Length)
                            {
                                arr[i2].Add('x');
                            }
                            else
                            {
                                arr[i2].Add(plainText[c]);
                                c++;
                            }
                        }
                    }
                    for (int i2 = 0; i2 < col; i2++)
                    {
                        string x = "";
                        for (int j = 0; j < row; j++)
                        {
                            x += arr[j][i2];
                        }
                        dic[i2] = x;
                    }
                    bool ok = true, found_length = true;
                    for (int i2 = 0; i2 < col; i2++)
                    {
                        string x = dic[i2];
                        for (int j = 0; j < cipherText.Length; j += row)
                        {
                            ok = true;
                            if (cipherText[j] == x[0])
                            {
                                for (int k = 0; k < row; k++)
                                {
                                    if (cipherText[j + k] != x[k])
                                        ok = false;
                                }
                                if (ok == true)
                                    break;
                            }
                            else
                                ok = false;
                        }
                        if (ok == false)
                        {
                            found_length = false;
                            break;
                        }
                    }
                    if (found_length == false)
                    {
                        dic.Clear();
                        continue;
                    }
                    Count = i;
                    rows = row;
                    break;
                }
                for (int i = 0; i < Count; i++)
                    key.Add(0);

                if (dic.Count > 0)
                {
                    for (int i = 0; i < Count; i++)
                    {
                        double indx = cipherText.IndexOf(dic[i]);
                        key[i] = (int)(Math.Ceiling(indx / Convert.ToDouble(rows)) + 1);
                    }
                }
            }
            return key;
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            for (int i = 0; i < cipherText.Length % key.Count; i++)
                cipherText += 'x';
            cipherText = cipherText.ToLower();
            string plainText = "";
            int col = key.Count, row = (int)Math.Ceiling(Convert.ToDouble(cipherText.Length) / key.Count);
            int c = 0;
            Dictionary<int, string> dic = new Dictionary<int, string>();
            for (int i = 0; i < col; i++)
            {
                string x = "";
                for (int j = 0; j < row; j++)
                {
                    x += cipherText[c];
                    c++;
                }
                dic[i + 1] = x;
            }
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    plainText += dic[key[j]][i];
                }
            }

            return plainText;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            string cipherText = "";
            int col = key.Count, row = (int)Math.Ceiling(Convert.ToDouble(plainText.Length) / key.Count);
            List<List<char>> arr = new List<List<char>>();
            int c = 0;
            for (int i = 0; i < row; i++)
            {
                arr.Add(new List<char>());
                for (int j = 0; j < col; j++)
                {
                    if (c == plainText.Length)
                    {
                        arr[i].Add('x');
                    }
                    else
                    {
                        arr[i].Add(plainText[c]);
                        c++;
                    }
                }
            }
            Dictionary<int, string> dic = new Dictionary<int, string>();
            for (int i = 0; i < col; i++)
            {
                string x = "";
                for (int j = 0; j < row; j++)
                {
                    x += arr[j][i];
                }
                dic[key[i]] = x;
            }

            for (int i = 1; i <= col; i++)
            {
                cipherText += dic[i];
            }

            return cipherText;
        }
    }
}
