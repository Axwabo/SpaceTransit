using UnityEngine;

namespace SpaceTransit.Tubes
{

    public abstract class TubeBase : MonoBehaviour
    {

        public abstract float Length { get; }

        public abstract (Vector3, Quaternion) Sample(float distance);

        public abstract float GetDistance(Vector3 point);

    }

}
