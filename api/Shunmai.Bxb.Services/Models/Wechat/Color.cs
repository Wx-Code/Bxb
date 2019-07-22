using System;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    public class Color
    {
        private static void EnsureValueInRange(string paraName, short value)
        {
            if (value < 0 || value > 255)
            {
                throw new ArgumentOutOfRangeException(paraName, "RGB value must be in the range of 0~255.");
            }
        }

        public short R { get; private set; }
        public short G { get; private set; }
        public short B { get; private set; }

        private Color(short r, short g, short b)
        {
            R = r;
            G = g;
            B = b;
        }

        public static Color Of(short r, short g, short b)
        {
            EnsureValueInRange(nameof(r), r);
            EnsureValueInRange(nameof(g), g);
            EnsureValueInRange(nameof(b), b);

            var color = new Color(r, g, b);
            return color;
        }

        public static Color White = Of(255, 255, 255);
        public static Color Black = Of(0, 0, 0);
    }
}
