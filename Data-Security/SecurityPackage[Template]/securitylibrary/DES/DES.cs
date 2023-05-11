using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DES
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    /// ME
    public class DES : CryptographicTechnique
    {
        public List<string> keys = new List<string>();
        int[,] s1 = new int[4, 16] {
                { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
                { 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
                { 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
                { 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 } };
        int[,] s2 = new int[4, 16] {
                { 15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
                { 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
                { 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
                { 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 } };
        int[,] s3 = new int[4, 16] {
                { 10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
                { 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
                { 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
                { 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 } };
        int[,] s4 = new int[4, 16] {
                { 7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
                { 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
                { 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
                { 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 } };
        int[,] s5 = new int[4, 16] {
                { 2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
                { 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
                { 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
                { 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 } };
        int[,] s6 = new int[4, 16] {
                { 12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
                { 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
                { 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
                { 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 } };
        int[,] s7 = new int[4, 16] {
                { 4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
                { 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
                { 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
                { 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 } };
        int[,] s8 = new int[4, 16] {
                { 13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
                { 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
                { 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
                { 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 } };

        public static string HexToBinary(string hex)
        {
            string hexDigits = "0123456789ABCDEF";
            string binary = "";
            for (int i = 2; i < hex.Length; i++)
            {
                int hexDigit = hexDigits.IndexOf(char.ToUpper(hex[i]));
                binary += Convert.ToString(hexDigit, 2).PadLeft(4, '0');
            }
            return binary;
        }
        public static string XOR(string binary1, string binary2)
        {
            string res = "";
            for (int i = 0; i < binary1.Length; i++)
            {
                if (binary1[i] != binary2[i]) res += '1';
                else res += '0';
            }
            return res;
        }
        public static string ToHexa(string n)
        {
            StringBuilder hex = new StringBuilder();
            for (int i = 0; i < n.Length; i += 4)
            {
                string nibble = n.Substring(i, 4);
                int value = Convert.ToInt32(nibble, 2);
                hex.Append(value.ToString("X"));
            }
            return hex.ToString();
        }

        public static string Shift(char LR, int N, string key)
        {
            if (LR == 'L')
            {
                if (N == 1)
                {
                    char c = key[0];
                    string res = "";
                    for (int i = 1; i < key.Length; i++) res += key[i];
                    res += c;
                    return res;
                }
                else if (N == 2)
                {
                    char c1 = key[0];
                    char c2 = key[1];
                    string res = "";
                    for (int i = 2; i < key.Length; i++) res += key[i];
                    res += c1;
                    res += c2;
                    return res;
                }
            }
            else if (LR == 'R')
            {
                if (N == 1)
                {
                    char c = key[key.Length - 1];
                    string res = "";
                    res += c;
                    for (int i = 0; i < key.Length - 1; i++) res += key[i];
                    return res;
                }
                else if (N == 2)
                {
                    char c1 = key[key.Length - 2];
                    char c2 = key[key.Length - 1];
                    string res = "";
                    res += c1;
                    res += c2;
                    for (int i = 0; i < key.Length - 2; i++) res += key[i];
                    return res;
                }
            }
            return key;
        }
        public string GetR(string L, string R, string k)
        {
            string ER = Replace(R, "R");
            string tmp_f = XOR(ER, k);
            List<string> blocks = new List<string>();
            for (int i = 0; i < 48; i += 6)
            {
                blocks.Add(tmp_f.Substring(i, 6));
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < blocks.Count; i++)
            {
                int row = Convert.ToInt32(blocks[i][0].ToString() + blocks[i][5].ToString(), 2);
                int col = Convert.ToInt32(blocks[i].Substring(1, 4), 2);
                int value = 0;
                switch (i)
                {
                    case 0: value = s1[row, col]; break;
                    case 1: value = s2[row, col]; break;
                    case 2: value = s3[row, col]; break;
                    case 3: value = s4[row, col]; break;
                    case 4: value = s5[row, col]; break;
                    case 5: value = s6[row, col]; break;
                    case 6: value = s7[row, col]; break;
                    case 7: value = s8[row, col]; break;
                }
                sb.Append(Convert.ToString(value, 2).PadLeft(4, '0'));
            }
            string f = sb.ToString();
            int[] P = new int[32] { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };
            sb.Clear();
            for (int i = 0; i < 32; i++)
            {
                sb.Append(f[P[i] - 1]);
            }
            string new_R = XOR(L, sb.ToString());
            return new_R;
        }
        public int[] All(string n)
        {
            int[] PC1 = new int[56] { 57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4 };
            int[] PC2 = new int[48] { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };
            int[] IP = new int[64] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };
            int[] IP_1 = new int[64] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };
            int[] arr = new int[48] { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };

            if (n == "PC1")
            {
                return PC1;
            }
            else if (n == "PC2")
            {
                return PC2;
            }
            else if (n == "IP")
            {
                return IP;
            }
            else if (n == "IP_1")
            {
                return IP_1;
            }
            else return arr;
        }
        public string Replace(string key, string n)
        {
            int[] arr = All(n);
            string New_key = "";
            for (int i = 0; i < arr.Length; i++)
                New_key += key[arr[i] - 1];
            return New_key;
        }
        public void Start(string key)
        {
            string nkey = HexToBinary(key);
            nkey = Replace(nkey, "PC1");
            string c = nkey.Substring(0, 28);
            string d = nkey.Substring(28, 28);
            keys.Add(Replace(c + d, "PC2"));

            for (int i = 1; i <= 16; i++)
            {
                if (i == 1 || i == 2 || i == 9 || i == 16)
                {
                    c = Shift('L', 1, c);
                    d = Shift('L', 1, d);
                }
                else
                {
                    c = Shift('L', 2, c);
                    d = Shift('L', 2, d);
                }

                keys.Add(Replace(c + d, "PC2"));
            }
        }
        public string FinalEncrypt(string pt, char ED)
        {
            string ip = Replace(HexToBinary(pt), "IP");
            string l = ip.Substring(0, 32);
            string r = ip.Substring(32, 32);

            if (ED == 'E')
            {
                for (int i = 1; i <= 16; i++)
                {
                    string newl = r;
                    r = GetR(l, r, keys[i]);
                    l = newl;

                }
            }
            else
            {

                for (int i = 1; i <= 16; i++)
                {
                    string newl = r;
                    r = GetR(l, r, keys[keys.Count - i]);
                    l = newl;

                }
            }

            r = r + l;
            string final = Replace(r, "IP_1");
            if (ED == 'E')
                return ("0x" + Convert.ToInt64(final, 2).ToString("X"));
            else
            {
                string ans = "0x";
                for (int i = 0; i < 64; i += 4)
                {
                    string temp = ToHexa(final.Substring(i, 4));
                    ans += temp;
                }
                return ans;
            }
        }

        public override string Decrypt(string cipherText, string key)
        {
            Start(key);
            return FinalEncrypt(cipherText, 'D');

        }
        public override string Encrypt(string plainText, string key)
        {
            Start(key);
            return FinalEncrypt(plainText, 'E');
        }
    }
}