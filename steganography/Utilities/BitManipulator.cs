using System;
using System.Collections.Generic;
using System.Text;

namespace steganography.Utilities
{
    class BitManipulator
    {
        public static int ReverseByte(int n)
        {
            int result = 0;

            //1 byte equals 1 bit
            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }
    }
}
