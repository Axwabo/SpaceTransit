using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenBase : MonoBehaviour
    {

        public abstract void Navigate(bool forwards);

        public abstract void Confirm();

        public abstract bool Select(int index);

        public abstract void SetVisibility(bool visible);

    }

}
