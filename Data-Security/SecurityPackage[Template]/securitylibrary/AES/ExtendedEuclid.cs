using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int x1 = 1, y1 = 0, x2 = 0, y2 = 1, quotient, temp;

            while (number > 1)
            {
                quotient = baseN / number;
                temp = x2;
                x2 = x1 - quotient * x2;
                x1 = temp;

                temp = y2;
                y2 = y1 - quotient * y2;
                y1 = temp;

                temp = number;
                number = baseN - quotient * number;
                baseN = temp;
                
            }
            if(number != 0)
            {
                if (y2 < 0)
                {
                    return y2 + 26;
                }
                return y2;
            }
            else
                return -1;
            
        }

    }
}