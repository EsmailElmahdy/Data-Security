using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
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
        public int D(int e, int X)
        {
            int i = 0;
            while (true)
            {
                i++;
                if ((e * i) % X == 1)
                    return i;
            }
        }

        public int Encrypt(int p, int q, int M, int e)
        {
            //throw new NotImplementedException();
            return Power(M, e, (p * q));

        }

        public int Decrypt(int p, int q, int C, int e)
        {
            //throw new NotImplementedException();
            return Power(C, D(e, ((p - 1) * (q - 1))), (p * q));
        }
    }
}