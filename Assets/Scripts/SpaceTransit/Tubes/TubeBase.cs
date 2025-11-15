using UnityEngine;

namespace SpaceTransit.Tubes
{

    [ExecuteInEditMode]
    public abstract class TubeBase : MonoBehaviour
    {

        protected Transform Transform { get; private set; }

        [field: SerializeField]
        public TubeBase Next { get; private set; }

        public bool HasNext { get; private set; }

        public TubeBase Previous { get; private set; }

        public bool HasPrevious { get; private set; }

        protected virtual void Awake()
        {
            Transform = transform;
            if (!Next)
                return;
            Next.Previous = this;
            Next.HasPrevious = true;
            HasNext = true;
        }

        public abstract float Length { get; }

        public abstract (Vector3 Position, Quaternion Rotation) Sample(float distance);

        public abstract float GetDistance(Vector3 point);

    }

}
