using UnityEngine;

namespace SpaceTransit
{

    public readonly struct ShipSpeed
    {

        public const float MetersPerSecondToWorldUnits = 0.01f;

        public float Raw { get; }

        public bool IsReverse { get; }

        public float World => IsReverse ? -Raw : Raw; // TODO: convert from smaller scale

        public ShipSpeed MoveTowards(float target, float delta, float max)
            => new(Mathf.Clamp(Mathf.MoveTowards(Raw, target, delta), 0, max), IsReverse);

        public ShipSpeed FlipReverse() => new(Raw, !IsReverse);

        public ShipSpeed(float value, bool isReverse)
        {
            Raw = Mathf.Abs(value);
            IsReverse = isReverse;
        }

        public static implicit operator ShipSpeed(float value) => new(value, value < 0);

        public static float operator *(ShipSpeed speed, float scalar) => scalar * speed.World;

        public static ShipSpeed operator +(ShipSpeed speed, float amount) => speed.Raw + amount;

        public static ShipSpeed operator -(ShipSpeed speed, float amount) => speed.Raw - amount;

        public static bool operator >(ShipSpeed left, ShipSpeed right) => left.Raw > right.Raw;

        public static bool operator <(ShipSpeed left, ShipSpeed right) => left.Raw < right.Raw;

    }

}
