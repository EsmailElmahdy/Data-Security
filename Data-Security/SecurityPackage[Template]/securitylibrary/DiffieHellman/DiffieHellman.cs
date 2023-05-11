using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman
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
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            //throw new NotImplementedException();
            int Ya = Power(alpha, xa, q);
            int Yb = Power(alpha, xb, q);
            int Ka = Power(Ya, xb, q);
            int Kb = Power(Yb, xa, q);
            List<int> Keys = new List<int>();
            Keys.Add(Kb);
            Keys.Add(Ka);
            return Keys;
        }
    }
}