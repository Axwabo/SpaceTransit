using UnityEngine;

namespace SpaceTransit.Stations
{

    public abstract class EntryDisplayBase<T> : MonoBehaviour
    {

        public abstract void Apply(T item);

    }

}
