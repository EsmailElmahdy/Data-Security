using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.ElGamal
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        public int Power(int M, int e, int N)
        {
            if (e == 1)
            {
                return M;
            }
            long Temp = Power(M, e / 2, N);
            Temp = ((Temp % N) * (Temp % N)) % N;
            if (e % 2 != 0)
            {
                Temp = ((Temp % N) * (M % N)) % N;
            }
            return (int)Temp;
        }
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            //throw new NotImplementedException();
            int C1 = Power(alpha, k, q);
            int C2 = (Power(y, k, q) * m) % q;
            List<long> ciphertext = new List<long>();
            ciphertext.Add(C1);
            ciphertext.Add(C2);
            return ciphertext;

        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
            //throw new NotImplementedException();
            int k = Power(c1, x, q);
            int m = (c2 * Power(k, q - 2, q)) % q;
            return m;

        }
    }
}