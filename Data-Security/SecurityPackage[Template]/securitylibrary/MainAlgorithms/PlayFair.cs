using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        static char[,] playfair_arr = new char[5, 5];
        static int col1, col2;
        static int row1, row2;
        static void Playfair(string keyword)
        {
            HashSet<char> matrix = new HashSet<char>();
            foreach (char i in keyword)
            {
                if (i == 'j') matrix.Add('i');
                else matrix.Add(i);
            }
            for (char i = 'a'; i <= 'z'; i++)
            {
                if (i == 'j') matrix.Add('i');
                else matrix.Add(i);
            }
            int c = 0;
            foreach (char i in matrix)
            {
                playfair_arr[c / 5, c % 5] = i;
                c++;
            }
        }
        static void search(char ch)
        {
            int p = 0;
            for (int i = 0; i < 5; i++)
            {
                if (p == 1) break;
                for (int j = 0; j < 5; j++)
                {
                    if (ch == playfair_arr[i, j])
                    {
                        p = 1;
                        row2 = i;
                        col2 = j;
                        break;
                    }
                }
            }
        }
        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToLower();
            key = key.ToLower();
            string answer = "";
            Playfair(key);
            string plain_text = "";
            for (int i = 0; i < cipherText.Length; i += 2)
            {
                char a = (char)cipherText[i];
                char b = (char)cipherText[i + 1];
                if (a == 'j') a = 'i';
                if (b == 'j') b = 'i';
                search(a);
                row1 = row2;
                col1 = col2;
                search(b);
                //   Console.WriteLine(a + " " + row1 + " " + col1 + "      " + b + " " + row2 + " " + col2);
                if (row1 == row2)
                {
                    if (col1 == 0)
                    {
                        plain_text += playfair_arr[row1, 4];
                        if (col2 >= 1) plain_text += playfair_arr[row1, (col2 - 1) % 5];


                    }
                    else if (col2 == 0)
                    {
                        plain_text += playfair_arr[row1, col1 - 1];
                        plain_text += playfair_arr[row1, 4];
                    }
                    else
                    {
                        plain_text += playfair_arr[row1, col1 - 1];
                        plain_text += playfair_arr[row1, col2 - 1];
                    }
                }
                else if (col1 == col2)
                {
                    if (row1 == 0)
                    {
                        plain_text += playfair_arr[4, col1];
                        plain_text += playfair_arr[row2 - 1, col2];
                    }
                    else if (row2 == 0)
                    {
                        plain_text += playfair_arr[row1 - 1, col1];
                        plain_text += playfair_arr[4, col2];
                    }
                    else
                    {
                        plain_text += playfair_arr[row1 - 1, col1];
                        plain_text += playfair_arr[row2 - 1, col2];
                    }
                }
                else
                {
                    plain_text += playfair_arr[row1, col2];
                    plain_text += playfair_arr[row2, col1];
                }
            }
            // Console.WriteLine(plain_text);
            string ans = "";
            if (plain_text[plain_text.Length - 1] == 'x') plain_text =  plain_text.Remove(plain_text.Length - 1 , 1);
            for(int i = 0; i < plain_text.Length; i ++ )
            {
                if (plain_text[i] == 'x' && (i % 2) == 1)
                {
                    if( i != plain_text.Length - 1 )
                    {
                        if (i != 0)
                        {
                            if (plain_text[i + 1] == plain_text[i - 1])
                            {
                                continue;
                            }
                        }
                    }
                }
                ans += plain_text[i];
            }

            return ans;
        }
        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            key = key.ToLower();
            key = key.Replace("j", "i");
            string Alphabit = "abcdefghiklmnopqrstuvwxyz";
            string KeyWord = new string((key + Alphabit).Distinct().ToArray()).ToLower();
            KeyWord = Regex.Replace(KeyWord, "[^a-z]", "");
            Console.WriteLine(KeyWord);

            // Generate the Matrix
            char[,] Matrix = new char[5, 5];
            int c = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Matrix[i, j] = KeyWord[c];
                    c++;
                }
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(Matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

            // Prepare plaintext 
            string PlainText = plainText.ToLower();
            plainText = plainText.Replace("j", "i");
            string FinalPlainText = "";
            FinalPlainText += PlainText;
            FinalPlainText = Regex.Replace(FinalPlainText, "[^a-z]", "");

            Console.WriteLine(FinalPlainText);

            //Encrption
            int r1 = 0, c1 = 0, r2 = 0, c2 = 0;
            string EncryptedText = "";
            for (int i = 0; i < FinalPlainText.Length; i += 2)
            {
                if (i < FinalPlainText.Length - 1)
                {
                    char first = FinalPlainText[i];
                    char second = FinalPlainText[i + 1];
                    if (first == second)
                    {
                        FinalPlainText = FinalPlainText.Insert(i, "x");
                        second = FinalPlainText[i];
                    }
                    Console.WriteLine(first + " " + second);
                    // Get the first Character index
                    for (int row = 0; row < 5; row++)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            if (Matrix[row, col] == (first))
                            {
                                r1 = row;
                                c1 = col;
                                break;
                            }
                        }
                    }
                    // Get the Second Character index
                    for (int row = 0; row < 5; row++)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            if (Matrix[row, col] == (second))
                            {
                                r2 = row;
                                c2 = col;
                                break;
                            }
                        }
                    }

                    // Encrypt Rules

                    //same row
                    if (r1 == r2)
                    {
                        EncryptedText += Matrix[r1, ((c1 + 1) % 5)].ToString() + Matrix[r2, ((c2 + 1) % 5)].ToString();
                    }
                    //same col
                    else if (c1 == c2)
                    {
                        EncryptedText += Matrix[((r1 + 1) % 5), c1].ToString() + Matrix[((r2 + 1) % 5), c2].ToString();
                    }
                    //Square
                    else
                    {
                        EncryptedText += Matrix[r1, c2].ToString() + Matrix[r2, c1].ToString();
                    }
                }
                else
                {
                    char first = FinalPlainText[i];
                    char second = 'x';
                    Console.WriteLine(first + " " + second);
                    // Get the first Character index
                    for (int row = 0; row < 5; row++)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            if (Matrix[row, col] == (first))
                            {
                                r1 = row;
                                c1 = col;
                                break;
                            }
                        }
                    }
                    // Get the Second Character index
                    for (int row = 0; row < 5; row++)
                    {
                        for (int col = 0; col < 5; col++)
                        {
                            if (Matrix[row, col] == (second))
                            {
                                r2 = row;
                                c2 = col;
                                break;
                            }
                        }
                    }

                    // Encrypt Rules

                    //same row
                    if (r1 == r2)
                    {
                        EncryptedText += Matrix[r1, ((c1 + 1) % 5)].ToString() + Matrix[r2, ((c2 + 1) % 5)].ToString();
                    }
                    //same col
                    else if (c1 == c2)
                    {
                        EncryptedText += Matrix[((r1 + 1) % 5), c1].ToString() + Matrix[((r2 + 1) % 5), c2].ToString();
                    }
                    //Square
                    else
                    {
                        EncryptedText += Matrix[r1, c2].ToString() + Matrix[r2, c1].ToString();
                    }
                }
            }
            return EncryptedText;
        }
    }
}