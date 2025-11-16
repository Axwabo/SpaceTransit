using UnityEngine;

namespace SpaceTransit.Ships.Modules
{

    public abstract class SubcomponentBase<T> : MonoBehaviour
    {

        public Transform Transform { get; private set; }

        public T Parent { get; private set; }

        protected virtual void Awake() => Transform = transform;

        public void Initialize(T parent)
        {
            Parent = parent;
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

    }

}
