using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkIris
{
    internal class Utilities
    {
        public static float ColorByteToFloat(byte Value)
        {
            return (float)((double)Value / 255.0);
        }

        public static byte ColorFloatToByte(float Value)
        {
            var f2 = Math.Max(0.0, Math.Min(1.0, Value));
            return Convert.ToByte(Math.Floor(f2 == 1.0 ? 255 : f2 * 256.0));
        }

        public static float ClampFloat(float Value, float Min, float Max)
        {
            return (float)Math.Max(Min, Math.Min(Max, Value));
        }
    }
}
