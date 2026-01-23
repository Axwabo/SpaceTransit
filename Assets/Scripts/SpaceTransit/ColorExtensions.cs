using UnityEngine;

namespace SpaceTransit
{

    public static class ColorExtensions
    {

        public static string ToHex(this Color color)
        {
            var r = color.r * 255;
            var g = color.g * 255;
            var b = color.b * 255;
            return $"#{(byte) r:X2}{(byte) g:X2}{(byte) b:X2}";
        }

    }

}
