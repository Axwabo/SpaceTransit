using System;
using UnityEngine;

namespace SpaceTransit
{

    public static class AnimationCurveExtensions
    {

        public static float Duration(this AnimationCurve curve)
        {
            Span<Keyframe> span = stackalloc Keyframe[curve.length];
            curve.GetKeys(span);
            return span[^1].time;
        }

    }

}
