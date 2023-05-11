using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    
    public class AES : CryptographicTechnique
    {

        string[,] RCON =
        {
            { "01","02","04","08","10","20","40","80","1b","36"},
            { "00","00","00","00","00","00","00","00","00","00"},
            { "00","00","00","00","00","00","00","00","00","00"},
            { "00","00","00","00","00","00","00","00","00","00"}
        };
        string[,] M =
        {
            {"02", "03", "01", "01"},
            {"01", "02", "03", "01"},
            {"01", "01", "02", "03"},
            {"03", "01", "01", "02"}
        };
        string[,] M_D = {
                {"0E","0B","0D","09"},
                {"09","0E","0B","0D"},
                { "0D","09","0E","0B"},
                { "0B","0D","09","0E"}
            };
        string[,] Box = {
            { "63", "7c", "77", "7b", "f2", "6b", "6f", "c5", "30", "01", "67", "2b", "fe", "d7", "ab", "76" },
            { "ca", "82", "c9", "7d", "fa", "59", "47", "f0", "ad", "d4", "a2", "af", "9c", "a4", "72", "c0" },
            { "b7", "fd", "93", "26", "36", "3f", "f7", "cc", "34", "a5", "e5", "f1", "71", "d8", "31", "15" },
            { "04", "c7", "23", "c3", "18", "96", "05", "9a", "07", "12", "80", "e2", "eb", "27", "b2", "75" },
            { "09", "83", "2c", "1a", "1b", "6e", "5a", "a0", "52", "3b", "d6", "b3", "29", "e3", "2f", "84" },
            { "53", "d1", "00", "ed", "20", "fc", "b1", "5b", "6a", "cb", "be", "39", "4a", "4c", "58", "cf" },
            { "d0", "ef", "aa", "fb", "43", "4d", "33", "85", "45", "f9", "02", "7f", "50", "3c", "9f", "a8" },
            { "51", "a3", "40", "8f", "92", "9d", "38", "f5", "bc", "b6", "da", "21", "10", "ff", "f3", "d2" },
            { "cd", "0c", "13", "ec", "5f", "97", "44", "17", "c4", "a7", "7e", "3d", "64", "5d", "19", "73" },
            { "60", "81", "4f", "dc", "22", "2a", "90", "88", "46", "ee", "b8", "14", "de", "5e", "0b", "db" },
            { "e0", "32", "3a", "0a", "49", "06", "24", "5c", "c2", "d3", "ac", "62", "91", "95", "e4", "79" },
            { "e7", "c8", "37", "6d", "8d", "d5", "4e", "a9", "6c", "56", "f4", "ea", "65", "7a", "ae", "08" },
            { "ba", "78", "25", "2e", "1c", "a6", "b4", "c6", "e8", "dd", "74", "1f", "4b", "bd", "8b", "8a" },
            { "70", "3e", "b5", "66", "48", "03", "f6", "0e", "61", "35", "57", "b9", "86", "c1", "1d", "9e" },
            { "e1", "f8", "98", "11", "69", "d9", "8e", "94", "9b", "1e", "87", "e9", "ce", "55", "28", "df" },
            { "8c", "a1", "89", "0d", "bf", "e6", "42", "68", "41", "99", "2d", "0f", "b0", "54", "bb", "16" }
        };
        string[,] Box_D = {
              {"52", "09", "6a", "d5", "30", "36", "a5", "38", "bf", "40", "a3", "9e", "81", "f3", "d7", "fb"},
              {"7c", "e3", "39", "82", "9b", "2f", "ff", "87", "34", "8e", "43", "44", "c4", "de", "e9", "cb"},
              {"54", "7b", "94", "32", "a6", "c2", "23", "3d", "ee", "4c", "95", "0b", "42", "fa", "c3", "4e"},
              {"08", "2e", "a1", "66", "28", "d9", "24", "b2", "76", "5b", "a2", "49", "6d", "8b", "d1", "25"},
              {"72", "f8", "f6", "64", "86", "68", "98", "16", "d4", "a4", "5c", "cc", "5d", "65", "b6", "92"},
              {"6c", "70", "48", "50", "fd", "ed", "b9", "da", "5e", "15", "46", "57", "a7", "8d", "9d", "84"},
              {"90", "d8", "ab", "00", "8c", "bc", "d3", "0a", "f7", "e4", "58", "05", "b8", "b3", "45", "06"},
              {"d0", "2c", "1e", "8f", "ca", "3f", "0f", "02", "c1", "af", "bd", "03", "01", "13", "8a", "6b"},
              {"3a", "91", "11", "41", "4f", "67", "dc", "ea", "97", "f2", "cf", "ce", "f0", "b4", "e6", "73"},
              {"96", "ac", "74", "22", "e7", "ad", "35", "85", "e2", "f9", "37", "e8", "1c", "75", "df", "6e"},
              {"47", "f1", "1a", "71", "1d", "29", "c5", "89", "6f", "b7", "62", "0e", "aa", "18", "be", "1b"},
              {"fc", "56", "3e", "4b", "c6", "d2", "79", "20", "9a", "db", "c0", "fe", "78", "cd", "5a", "f4"},
              {"1f", "dd", "a8", "33", "88", "07", "c7", "31", "b1", "12", "10", "59", "27", "80", "ec", "5f"},
              {"60", "51", "7f", "a9", "19", "b5", "4a", "0d", "2d", "e5", "7a", "9f", "93", "c9", "9c", "ef"},
              {"a0", "e0", "3b", "4d", "ae", "2a", "f5", "b0", "c8", "eb", "bb", "3c", "83", "53", "99", "61"},
              {"17", "2b", "04", "7e", "ba", "77", "d6", "26", "e1", "69", "14", "63", "55", "21", "0c", "7d"}
        };
        Dictionary<char, string> Hex_Bin = new Dictionary<char, string>();
       

        public string[,] toint(string s)
        {
            string[,] ans = new string[4, 4];
            int c = 2;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ans[j, i] = Convert.ToString(s[c]);
                    ans[j, i] += s[c + 1];
                    c += 2;
                }
            }
            return ans;
        }
        public string tostring(string[,] s)
        {
            string ans = "0x";
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    ans += s[j, i];
            ans = ans.ToUpper();
            return ans;
        }
        public int[,]StToarr(string[,] s)
        {
            int[,] ans = new int[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < s.GetLength(1); j++)
                {
                  
                    ans[i, j] = Convert.ToInt32(s[i, j], 16);
                   
                }
            }
            return ans;
        } 
        public int[]StToarr(string[] s)
        {
            int[] ans = new int[4];
            for (int i = 0; i < 4; i++)
            {
                  ans[i] = Convert.ToInt32(s[i], 16);   
            }
            return ans;
        }
        public string[,] arrToSt(int[,] matrix)
        {
            string[,] ans = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (matrix[i, j] == 0)
                        ans[i, j] = "00";
                    
                    else if (matrix[i, j] < 16)
                        ans[i, j] = "0" + Convert.ToString(matrix[i, j], 16);
                        
                    else
                        ans[i, j] = Convert.ToString(matrix[i, j], 16);

                }
            }
            return ans;
        }
        public static string[,] AddRoundKey(string[,] txt, string[,] key)
        {
            int[,] arr1 = new int[txt.GetLength(0), txt.GetLength(1)];
            int[,] arr2 = new int[key.GetLength(0), key.GetLength(1)];
            int rows = txt.GetLength(0);
            int cols = txt.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    arr1[i, j] = Convert.ToInt32(txt[i, j], 16);
                    arr2[i, j] = Convert.ToInt32(key[i, j], 16);
                }

            }
            string[,] res = new string[txt.GetLength(0), txt.GetLength(1)];
            int x;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    x = arr1[i, j] ^ arr2[i, j];
                    if (x < 16)
                    {
                        res[i, j] += "0";
                    }
                    res[i, j] += Convert.ToString(x, 16);
                }
            }
            return res;
        }
        public string[,] SubBytes(string[,] s1 , int x)
        {
            string[,] ans = new string[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if(x == 0)
                        ans[i, j] = Box[Convert.ToInt32(Convert.ToString(s1[i, j][0]), 16), Convert.ToInt32(Convert.ToString(s1[i, j][1]), 16)];
                    else
                        ans[i, j] = Box_D[Convert.ToInt32(Convert.ToString(s1[i, j][0]), 16), Convert.ToInt32(Convert.ToString(s1[i, j][1]), 16)];

                }
            }
            
            return ans;
        }
        public string[,] SubBytes_colmn(string [] s)
        {
            string[,] ans = new string[4, 1];
            for (int j = 0; j < 4; j++)
                ans[j, 0] = Box[Convert.ToInt32(Convert.ToString(s[j][0]), 16), Convert.ToInt32(Convert.ToString(s[j][1]), 16)];
            
            return ans;
        }
        public static string[,] ShiftRows(string[,] matrix)
        {
            string[,] res_matrix = new string[4, 4];
            int index, count;
            for (int i = 0; i < 4; i++)
            {
                count = 0;
                index = i;
                for (int j = index; j < 4; j++)
                {
                    res_matrix[i, count] = matrix[i, j];
                    count++;
                }
                for (int j = 0; j < index; j++)
                {
                    res_matrix[i, count] = matrix[i, j];
                    count++;
                }

            }
            return res_matrix;
        }
        public static string[,] ShiftRows_dec(string[,] matrix)
        {
            string[,] res_matrix = new string[4, 4];
            int index, count;
            for (int i = 0; i < 4; i++)
            {
                count = 0;
                index = 4 - i;
                for (int j = index; j < 4; j++)
                {
                    res_matrix[i, count] = matrix[i, j];
                    count++;
                }
                for (int j = 0; j < index; j++)
                {
                    res_matrix[i, count] = matrix[i, j];
                    count++;
                }

            }
            return res_matrix;
        }
        public int Multi_By_2(int x)
        {
           
            x = x << 1;
            if(x > 255 )
            {
                x -= 256;
                x ^= 27;
            }
            return x;
        }
        public int Multi_By_3(int x)
        {
            int y = Multi_By_2(x);
            return (int)y ^ (int)x;
        }
        public int Multi_By_9(int x)
        {
            //8
            int y = Multi_By_2(Multi_By_2(Multi_By_2(x)));
            //9
            return (int)y ^ (int)x;
        }
        public int Multi_By_11(int x)
        {
            //8
            int y = Multi_By_2(Multi_By_2(Multi_By_2(x)));
            int z = Multi_By_2(x);
            //10
            y = (int) y ^ (int) z;
            //11
            return (int)y ^ (int)x;
        } 
        public int Multi_By_13(int x)
        {
            //8
            int y = Multi_By_2(Multi_By_2(Multi_By_2(x)));
            int z = Multi_By_2( Multi_By_2(x));
            //12
            y = (int) y ^ (int) z;
            //13
            return (int)y ^ (int)x;
        } 
        public int Multi_By_15(int x)
        {
            //8
            int y = Multi_By_2(Multi_By_2(Multi_By_2(x)));
            int z = Multi_By_2(x);
            int xx = Multi_By_2(Multi_By_2(x));
            //10
            y = (int) y ^ (int) z;
            //14
            y = (int) y ^ (int) xx;
           
            return y;
        }
        public string[,] MixColumns(string[,] s)
        {
            int[,] Mixed = new int[4, 4];
            int[,] mx =StToarr(M);
            int[,] S =StToarr(s);
            for(int i = 0; i < 4; i ++)
            {
                for ( int j = 0; j < 4; j ++)
                {
                    int x = 0;
                    for(int jj = 0; jj < 4; jj++)
                    {  
                        if (mx[i, jj] == 3)
                        {
                            x = x ^ Multi_By_3(S[jj, j]);

                        }
                        else if (mx[i, jj] == 2)
                        {
                            x = x ^ Multi_By_2(S[jj, j]);
                        }
                        else
                        {
                            x = x ^ S[jj, j];
                        }
                       
                    }
                    Mixed[i, j] ^= x;
                }
            }
            return arrToSt(Mixed);
        }
        public string[,] MixColumns_Dec(string[,] s)
        {
            int[,] Mixed = new int[4, 4];
            int[,] mx =StToarr(M_D);
            int[,] S =StToarr(s);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int x = 0;
                    for (int jj = 0; jj < 4; jj++)
                    {
                        if (mx[i, jj] == 0x09)
                        {
                            x = x ^ Multi_By_9(S[jj, j]);
                        }
                        else if (mx[i, jj] == 0x0b)
                        {
                            x = x ^ Multi_By_11(S[jj, j]);
                        }
                        else if (mx[i, jj] == 0x0d)
                        {
                            x = x ^ Multi_By_13(S[jj, j]);
                        }
                        else if (mx[i, jj] == 0x0e)
                        {
                            x = x ^ Multi_By_15(S[jj, j]);
                        }
                    }
                    Mixed[i, j] ^= x;
                }
            }
            return arrToSt(Mixed);
        }
        public string[,] KeySchedule(string[,] s, int index)
        {
            int[,] ans = new int[4, 4];
            string[] Rcon = new string[4];
            string[] Last_column = new string[4];
           //fill last colmn
            for (int i = 0; i < 4; i++)
                Last_column[i] = s[(i + 1) % 4 , 3];
            
            int[,] sint =StToarr(s);
           
            //Subbyte
            string[,] sub = SubBytes_colmn(Last_column);

            int[,] sub2 =StToarr(sub);
            
           
            for (int i = 0; i < 4; i++)
                Rcon[i] =Convert.ToString( RCON[i, index]);

            int[] rcon =StToarr(Rcon);

            for (int i = 0; i < 4; i++)
            {
                ans[i, 0] = sint[i, 0] ^ sub2[i, 0] ^ rcon[i];
                ans[i, 1] = sint[i, 1] ^ ans[i, 0];
                ans[i, 2] = sint[i, 2] ^ ans[i, 1];
                ans[i, 3] = sint[i, 3] ^ ans[i, 2];
            }
            
           

            return arrToSt(ans);
        }
        public override string Decrypt(string cipherText, string key)
        {
            Hex_Bin.Add('0', "0000");
            Hex_Bin.Add('1', "0001");
            Hex_Bin.Add('2', "0010");
            Hex_Bin.Add('3', "0011");
            Hex_Bin.Add('4', "0100");
            Hex_Bin.Add('5', "0101");
            Hex_Bin.Add('6', "0110");
            Hex_Bin.Add('7', "0111");
            Hex_Bin.Add('8', "1000");
            Hex_Bin.Add('9', "1001");
            Hex_Bin.Add('a', "1010");
            Hex_Bin.Add('b', "1011");
            Hex_Bin.Add('c', "1100");
            Hex_Bin.Add('d', "1101");
            Hex_Bin.Add('e', "1110");
            Hex_Bin.Add('f', "1111");

            string[,] newCipher = toint(cipherText);
            string[,] newKey = toint(key);
            string[] newKeys = new string[11];
            newKeys[0] = tostring(newKey);

            for (int i = 1; i < 11; i++)
            {
                newKey = toint(newKeys[i - 1]);
                newKeys[i] = tostring(KeySchedule(newKey, i - 1));
            }

            string[,] plainText = AddRoundKey(newCipher, toint(newKeys[10]));

            for (int i = 9; i >= 0; i--)
            {
                if (i == 9)
                {
                    plainText = ShiftRows_dec(plainText);
                    plainText = SubBytes(plainText, 1);
                }

                newKey = toint(newKeys[i]);
                plainText = AddRoundKey(plainText, newKey);

                if (i > 0)
                {
                    plainText = MixColumns_Dec(plainText);
                    plainText = ShiftRows_dec(plainText);
                    plainText = SubBytes(plainText, 1);
                }
            }
            return tostring(plainText);
        }

        public override string Encrypt(string plainText, string key)
        {
            Hex_Bin.Add('0', "0000");
            Hex_Bin.Add('1', "0001");
            Hex_Bin.Add('2', "0010");
            Hex_Bin.Add('3', "0011");
            Hex_Bin.Add('4', "0100");
            Hex_Bin.Add('5', "0101");
            Hex_Bin.Add('6', "0110");
            Hex_Bin.Add('7', "0111");
            Hex_Bin.Add('8', "1000");
            Hex_Bin.Add('9', "1001");
            Hex_Bin.Add('a', "1010");
            Hex_Bin.Add('b', "1011");
            Hex_Bin.Add('c', "1100");
            Hex_Bin.Add('d', "1101");
            Hex_Bin.Add('e', "1110");
            Hex_Bin.Add('f', "1111");
            plainText = plainText.ToLower();
            key = key.ToLower();
           
            string[,] plain = toint(plainText);
            string[,] KEY = toint(key);
            string[,] cipher = AddRoundKey(plain, KEY);
            for (int i = 0; i < 10; i++)
            {
                KEY = KeySchedule(KEY, i);
                cipher = SubBytes(cipher, 0);
                cipher = ShiftRows(cipher);
                if (i < 9)
                    cipher = MixColumns(cipher);

                cipher = AddRoundKey(cipher, KEY);
            }
           return tostring(cipher);

        }
    }
}