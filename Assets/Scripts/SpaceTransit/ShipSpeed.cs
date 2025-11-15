using UnityEngine;

namespace SpaceTransit
{

    public readonly struct ShipSpeed
    {

        /*
         1698.899643887 pixels = 106.69 km
          1 pixel = 0.062799472 km
          1 pixel = 62.799472 m
          1 unit = 10m
          1 pixel = 6.2799472 units
          0.159237008 pixels = 1 unit
         */

        public float Raw { get; }

        public bool IsReverse { get; }

        public float Relative => IsReverse ? -Raw : Raw;

        public float RawKmh => Raw * 3.6f;

        public ShipSpeed Clamp(float max) => new(Mathf.Min(Raw, max), IsReverse);

        public ShipSpeed MoveTowards(float target, float delta, float max)
            => new(Mathf.Min(Mathf.MoveTowards(Raw, target, delta), max), IsReverse);

        public ShipSpeed FlipReverse() => new(Raw, !IsReverse);

        public ShipSpeed(float value, bool isReverse)
        {
            Raw = Mathf.Max(0, value);
            IsReverse = isReverse;
        }

        public static implicit operator ShipSpeed(float value) => new(value, value < 0);

        public static float operator *(ShipSpeed speed, float scalar) => scalar * speed.Relative;

        public static ShipSpeed operator +(ShipSpeed speed, float amount) => new(speed.Raw + amount, speed.IsReverse);

        public static ShipSpeed operator -(ShipSpeed speed, float amount) => new(speed.Raw - amount, speed.IsReverse);

        public static bool operator >(ShipSpeed left, ShipSpeed right) => left.Raw > right.Raw;

        public static bool operator <(ShipSpeed left, ShipSpeed right) => left.Raw < right.Raw;

    }

}
