using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<string, string>, ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            //throw new NotImplementedException();

            List<int> Enc = new List<int>();
            bool Found = false;
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    for (int k = 0; k < 26; k++)
                    {
                        for (int l = 0; l < 26; l++)
                        {
                            Enc = Encrypt(plainText, new List<int> { l, k, j, i });
                            Found = Enumerable.SequenceEqual(Enc, cipherText);
                            if (Found)
                            {
                                return new List<int> { l, k, j, i };
                            }
                        }
                    }
                }
            }
            if (!Found)
            {
                throw new InvalidAnlysisException();
            }
            return Enc;
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            List<int> anslist = new List<int>();
            int ksize = key.Count;
            int psize = cipherText.Count;
            int[,] keymatrix = new int[3, 3];
            int[,] rulematrix = new int[3, 3];
            int[,] rulematrix2 = new int[3, 3];
            int[,] plainmatrix = new int[20, 20];
            int[,] ciphermatrix = new int[20, 20];
            if (ksize == 9)
            {
                int i = 0, j = 0;
                //put key in matrix
                Console.WriteLine("Key");
                foreach (int c in key)
                {
                    keymatrix[i, j] = c;
                    j++;
                    if (j % 3 == 0)
                    {
                        i++;
                        j = 0;
                    }
                }
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        Console.Write(keymatrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                i = 0;
                j = 0;
                Console.WriteLine();
                Console.WriteLine();

                //put cipher in matrix
                Console.WriteLine("Cipher");
                foreach (int c in cipherText)
                {
                    ciphermatrix[i, j] = c;
                    i++;
                    if (i % 3 == 0)
                    {
                        j++;
                        i = 0;
                    }
                }
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < cipherText.Count / 3; j++)
                    {
                        Console.Write(ciphermatrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();

                //get det(key)
                int det = 0;
                det += (keymatrix[0, 0] * ((keymatrix[1, 1] * keymatrix[2, 2]) - keymatrix[1, 2] * keymatrix[2, 1]));
                det -= (keymatrix[0, 1] * ((keymatrix[1, 0] * keymatrix[2, 2]) - keymatrix[1, 2] * keymatrix[2, 0]));
                det += (keymatrix[0, 2] * ((keymatrix[1, 0] * keymatrix[2, 1]) - keymatrix[1, 1] * keymatrix[2, 0]));
                /*det += (keymatrix[1, 0] * ((keymatrix[0, 1] * keymatrix[2, 2]) - keymatrix[0, 2] * keymatrix[2, 1]));
                det -= (keymatrix[1, 1] * ((keymatrix[0, 0] * keymatrix[2, 2]) - keymatrix[0, 2] * keymatrix[2, 0]));
                det -= (keymatrix[1, 2] * ((keymatrix[0, 0] * keymatrix[2, 1]) - keymatrix[0, 1] * keymatrix[2, 0]));
                det += (keymatrix[2, 0] * ((keymatrix[0, 1] * keymatrix[1, 2]) - keymatrix[1, 1] * keymatrix[0, 2]));
                det -= (keymatrix[2, 1] * ((keymatrix[0, 0] * keymatrix[1, 2]) - keymatrix[0, 2] * keymatrix[1, 0]));
                det += (keymatrix[2, 2] * ((keymatrix[0, 0] * keymatrix[1, 1]) - keymatrix[0, 1] * keymatrix[1, 0]));*/
                det %= 26;
                det += 26;
                det %= 26;
                Console.WriteLine("Det = " + det);

                //Calculate B
                int b = 0;
                for (i = 0; i < 26; i++)
                {
                    if ((i * det) % 26 == 1)
                    {
                        b = i;
                        Console.WriteLine("B = " + i);
                        break;
                    }
                }


                //Console.WriteLine(5100 % 26);
                anslist.Add(b * ((((keymatrix[1, 1] * keymatrix[2, 2]) - keymatrix[1, 2] * keymatrix[2, 1]))));
                anslist.Add(b * -((((keymatrix[1, 0] * keymatrix[2, 2]) - keymatrix[1, 2] * keymatrix[2, 0]))));
                anslist.Add(b * ((((keymatrix[1, 0] * keymatrix[2, 1]) - keymatrix[1, 1] * keymatrix[2, 0]))));
                anslist.Add(b * -((((keymatrix[0, 1] * keymatrix[2, 2]) - keymatrix[0, 2] * keymatrix[2, 1]))));
                anslist.Add(b * ((((keymatrix[0, 0] * keymatrix[2, 2]) - keymatrix[0, 2] * keymatrix[2, 0]))));
                anslist.Add(b * -((((keymatrix[0, 0] * keymatrix[2, 1]) - keymatrix[0, 1] * keymatrix[2, 0]))));
                anslist.Add(b * ((((keymatrix[0, 1] * keymatrix[1, 2]) - keymatrix[1, 1] * keymatrix[0, 2]))));
                anslist.Add(b * -((((keymatrix[0, 0] * keymatrix[1, 2]) - keymatrix[0, 2] * keymatrix[1, 0]))));
                anslist.Add(b * ((((keymatrix[0, 0] * keymatrix[1, 1]) - keymatrix[0, 1] * keymatrix[1, 0]))));
                i = 0;
                j = 0;
                for (i = 0; i < anslist.Count; i++)
                {
                    anslist[i] %= 26;
                    anslist[i] += 26;
                    anslist[i] %= 26;
                }
                i = 0;
                j = 0;
                foreach (int c in anslist)
                {
                    rulematrix[i, j] = c;
                    j++;
                    if (j % 3 == 0)
                    {
                        i++;
                        j = 0;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("K matrix");
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        Console.Write(rulematrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }

                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        rulematrix2[i, j] = rulematrix[j, i];
                    }
                }
                Console.WriteLine();
                Console.WriteLine(431 % 26);
                Console.WriteLine("K matrix -1");
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        Console.Write(rulematrix2[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                anslist.Clear();
                for (int ii = 0; ii < (cipherText.Count / 3); ii++)
                {
                    int[] a = new int[3];
                    a[0] = ciphermatrix[0, ii];
                    a[1] = ciphermatrix[1, ii];
                    a[2] = ciphermatrix[2, ii];

                    for (i = 0; i < 3; i++)
                    {
                        int x = 0;
                        for (j = 0; j < 3; j++)
                        {
                            x += (rulematrix2[i, j] * a[j]);
                            //Console.WriteLine(rulematrix2[i, j]+ "     " + a[j]);
                        }
                        anslist.Add(x % 26);
                    }
                }
                i = 0;
                Console.WriteLine("Answer");
                foreach (int c in anslist)
                {
                    char cr = 'A';
                    for (i = 0; i < c; i++) cr++;
                    Console.Write(cr);

                    //  Console.Write(c + "  ");

                }
            }
            else
            {
                int i = 0, j = 0;
              
                foreach (int c in key)
                {
                    keymatrix[i, j] = c;
                    j++;
                    if (j % 2 == 0)
                    {
                        i++;
                        j = 0;
                    }
                }

                i = 0;
                j = 0;
                
                //put cipher in matrix

                foreach (int c in cipherText)
                {
                    ciphermatrix[i, j] = c;
                    i++;
                    if (i % 2 == 0)
                    {
                        j++;
                        i = 0;
                    }
                }

                //get det(key)
                double det = 0;
                det = (keymatrix[0, 0] * keymatrix[1, 1]) - (keymatrix[0, 1] * keymatrix[1, 0]);
                
                int num = 1 / (keymatrix[0, 0] * keymatrix[1, 1] - keymatrix[0, 1] * keymatrix[1, 0]);
                double num2 = 1 / (keymatrix[0, 0] * keymatrix[1, 1] - keymatrix[0, 1] * keymatrix[1, 0]);
                
                if(num != 1 && num != -1) throw new NotImplementedException();
                rulematrix[0, 0] = num * keymatrix[1, 1];
                rulematrix[0, 1] = num * -1 * keymatrix[0, 1];
                rulematrix[1, 0] = num * -1 * keymatrix[1, 0];
                rulematrix[1, 1] = num * keymatrix[0, 0];

                Console.WriteLine();
                Console.WriteLine("K matrix");
                for (i = 0; i < 2; i++)
                {
                    for (j = 0; j < 2; j++)
                    {
                        Console.Write(rulematrix[i, j] + " ");
                    }
                    Console.WriteLine();
                }
                anslist.Clear();

                for (int ii = 0; ii < (cipherText.Count / 2); ii++)
                {
                    int[] a = new int[2];
                    a[0] = ciphermatrix[0, ii];
                    a[1] = ciphermatrix[1, ii];

                    for (i = 0; i < 2; i++)
                    {
                        int x = 0;
                        for (j = 0; j < 2; j++)
                        {
                            x += (rulematrix[i, j] * a[j]);

                        }

                        while (x < 0) x += 26;

                        anslist.Add(x % 26);

                    }
                }
            }
            return anslist; 
            
        }

        public string Decrypt(string cipherText, string key)
        {
            throw new NotImplementedException();
        }

        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            List<int> anslist = new List<int>();
            //Encrypt_Hill(plain4, key4);
            int ksize = key.Count;
            int psize = plainText.Count;
            int[,] keymatrix = new int[3, 3];
            int[,] plainmatrix = new int[20, 20];
            int[,] ciphermatrix = new int[20, 20];
            if (ksize == 9)
            {
                int i = 0, j = 0;
                foreach (int c in key)
                {
                    keymatrix[i, j] = c;
                    j++;
                    if (j % 3 == 0)
                    {
                        i++;
                        j = 0;
                    }
                }
                i = 0;
                j = 0;
                foreach (int c in plainText)
                {
                    plainmatrix[i, j] = c;
                    i++;
                    if (i % 3 == 0)
                    {
                        j++;
                        i = 0;
                    }
                }
                Console.WriteLine("Plain Matrix");
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        Console.Write(plainmatrix[i, j]);
                        Console.Write("  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("Key Matrix");
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        Console.Write(keymatrix[i, j]);
                        Console.Write("  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                for (int ii = 0; ii < (psize / 3); ii++)
                {
                    int[] a = new int[3];
                    a[0] = plainmatrix[0, ii];
                    a[1] = plainmatrix[1, ii];
                    a[2] = plainmatrix[2, ii];
                    for (i = 0; i < 3; i++)
                    {
                        int x = 0;
                        for (j = 0; j < 3; j++)
                        {
                            x += (keymatrix[i, j] * a[j]);
                        }
                        anslist.Add(x % 26);
                    }
                }
                foreach (int c in anslist)
                {
                    Console.Write(c + " ");
                }
            }
            else
            {
                int i = 0, j = 0;
                foreach (int c in key)
                {
                    keymatrix[i, j] = c;
                    j++;
                    if (j % 2 == 0)
                    {
                        i++;
                        j = 0;
                    }
                }
                i = 0;
                j = 0;
                foreach (int c in plainText)
                {
                    plainmatrix[i, j] = c;
                    i++;
                    if (i % 2 == 0)
                    {
                        j++;
                        i = 0;
                    }
                }
                Console.WriteLine("Plain Matrix");
                for (i = 0; i < 2; i++)
                {
                    for (j = 0; j < plainText.Count / 2; j++)
                    {
                        Console.Write(plainmatrix[i, j]);
                        Console.Write("  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("Key Matrix");
                for (i = 0; i < 2; i++)
                {
                    for (j = 0; j < 2; j++)
                    {
                        Console.Write(keymatrix[i, j]);
                        Console.Write("  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                for (int ii = 0; ii < (plainText.Count / 2); ii++)
                {
                    int[] a = new int[2];
                    a[0] = plainmatrix[0, ii];
                    a[1] = plainmatrix[1, ii];

                    for (i = 0; i < 2; i++)
                    {
                        int x = 0;
                        for (j = 0; j < 2; j++)
                        {
                            x += (keymatrix[i, j] * a[j]);
                        }
                        anslist.Add(x % 26);
                    }
                }
                foreach (int c in anslist)
                {
                    Console.Write(c + " ");
                }
            }
            return anslist;

           // throw new NotImplementedException();
        }

        public string Encrypt(string plainText, string key)
        {
            List<int> plain = new List<int>();
            List<int> k = new List<int>();
            List<int> ans = new List<int>();
            plainText = plainText.ToLower();
            key = key.ToLower();
            foreach(char c in plainText)
            {
                plain.Add(c - 'a');
            }
            foreach(char c in key)
            {
                k.Add(c - 'a');
            }
            ans = Encrypt(plain, k);
            string s = "";
            char []arr =new char[26];
            int c1 = 0;
            for(char c = 'a'; c <= 'z'; c ++)
            {
                arr[c1] = c;
                c1++;
            }
            foreach( int i in ans)
            {
                s += arr[i];
            }
            return s;
        }

        public List<int> Analyse3By3Key(List<int> plain3, List<int> cipher3)
        {
            //throw new NotImplementedException();

            List<int> Result = new List<int>();
            List<List<int>> PlainMatrix = new List<List<int>>();
            List<List<int>> CipherMatrix = new List<List<int>>();
            int c = 0;
            //Make 2d matrix List to Fill the plain and cipher
            for (int i = 0; i < 3; i++)
            {
                PlainMatrix.Add(new List<int>());
                for (int j = 0; j < 3; j++)
                {
                    PlainMatrix[i].Add(0);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PlainMatrix[j][i] = plain3[c];
                    c++;
                }
            }
            c = 0;
            for (int i = 0; i < 3; i++)
            {
                CipherMatrix.Add(new List<int>());
                for (int j = 0; j < 3; j++)
                {
                    CipherMatrix[i].Add(0);
                }
            }
            c = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CipherMatrix[j][i] = cipher3[c];
                    c++;
                }
            }
            // Get the Determenate
            int Det = CalculateDet3by3(PlainMatrix);
            PlainMatrix = CalcMatrInver3by3(PlainMatrix, Det);

            for (int i2 = 0; i2 < 3; i2++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        sum += (CipherMatrix[i2][k] * PlainMatrix[k][j]);
                    }
                    Result.Add(sum % 26);
                }

            }
            return Result;


        }


        List<List<int>> CalcMatrInver3by3(List<List<int>> PlainMatrix, int Det)
        {
            /* https://www.youtube.com/watch?v=JK3ur6W4rvw Link this Methode to get the inverse */

            // make matrix with size 5*5
            List<List<int>> PlainMat = new List<List<int>>();
            for (int i = 0; i < 3; i++)
            {
                PlainMat.Add(new List<int>());
                for (int j = 0; j < 5; j++)
                {
                    PlainMat[i].Add(0);
                }
            }
            int c = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PlainMat[i][j] = PlainMatrix[i][j];
                    c++;
                    if (j == 2) continue;
                    PlainMat[i][j + 3] = PlainMatrix[i][j];
                }
            }
            PlainMat.Add(PlainMat[0]);
            PlainMat.Add(PlainMat[1]);

            // calculate the Adj
            // the second step in every line is to make all variable between 0 to 26

            PlainMatrix[0][0] = (PlainMat[1][1] * PlainMat[2][2]) - (PlainMat[2][1] * PlainMat[1][2]);
            PlainMatrix[0][0] = (26 + (PlainMatrix[0][0] % 26)) % 26;

            PlainMatrix[0][1] = PlainMat[2][1] * PlainMat[3][2] - PlainMat[3][1] * PlainMat[2][2];
            PlainMatrix[0][1] = ((PlainMatrix[0][1] % 26) + 26) % 26;

            PlainMatrix[0][2] = PlainMat[3][1] * PlainMat[4][2] - PlainMat[4][1] * PlainMat[3][2];
            PlainMatrix[0][2] = ((PlainMatrix[0][2] % 26) + 26) % 26;

            PlainMatrix[1][0] = PlainMat[1][2] * PlainMat[2][3] - PlainMat[2][2] * PlainMat[1][3];
            PlainMatrix[1][0] = ((PlainMatrix[1][0] % 26) + 26) % 26;

            PlainMatrix[1][1] = PlainMat[2][2] * PlainMat[3][3] - PlainMat[3][2] * PlainMat[2][3];
            PlainMatrix[1][1] = ((PlainMatrix[1][1] % 26) + 26) % 26;

            PlainMatrix[1][2] = PlainMat[3][2] * PlainMat[4][3] - PlainMat[4][2] * PlainMat[3][3];
            PlainMatrix[1][2] = ((PlainMatrix[1][2] % 26) + 26) % 26;

            PlainMatrix[2][0] = PlainMat[1][3] * PlainMat[2][4] - PlainMat[2][3] * PlainMat[1][4];
            PlainMatrix[2][0] = ((PlainMatrix[2][0] % 26) + 26) % 26;

            PlainMatrix[2][1] = PlainMat[2][3] * PlainMat[3][4] - PlainMat[3][3] * PlainMat[2][4];
            PlainMatrix[2][1] = ((PlainMatrix[2][1] % 26) + 26) % 26;

            PlainMatrix[2][2] = PlainMat[3][3] * PlainMat[4][4] - PlainMat[4][3] * PlainMat[3][4];
            PlainMatrix[2][2] = ((PlainMatrix[2][2] % 26) + 26) % 26;

            // To Calc the inverse and make values between 0 to 26
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    PlainMatrix[i][j] *= Det;
                    PlainMatrix[i][j] = ((PlainMatrix[i][j] % 26) + 26) % 26;
                }
            }
            return PlainMatrix;
        }

        int CalculateDet3by3(List<List<int>> Matrix)
        {
            int Temp1 = (Matrix[0][0] * (Matrix[1][1] * Matrix[2][2] - Matrix[1][2] * Matrix[2][1]));
            int Temp2 = (Matrix[0][1] * (Matrix[1][0] * Matrix[2][2] - Matrix[1][2] * Matrix[2][0]));
            int Temp3 = (Matrix[0][2] * (Matrix[1][0] * Matrix[2][1] - Matrix[1][1] * Matrix[2][0]));
            int Det = (Temp1 - Temp2 + Temp3) % 26;
            Det = (Det + 26) % 26;

            if (gcd(Det, 26) != 1 || Det == 0)
            {
                throw new InvalidAnlysisException();
            }
            // Calculate b which b * det mod 26 = 1
            for (int i = 1; i < 26; i++)
            {
                if ((Det * i) % 26 == 1)
                {
                    Det = i;
                    break;
                }
            }
            return Det;
        }

        long gcd(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }
            return gcd(b, a % b);
        }

        public string Analyse3By3Key(string plain3, string cipher3)
        {
            throw new NotImplementedException();
        }
    }
}
