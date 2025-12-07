using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class DockPicker : PickerBase
    {

        private static readonly Color EnteringColor = new(0, 1, 0, 0.24f);
        private static readonly Color FailedColor = new(1, 0, 0, 0.24f);

        public void Pick(bool locked) => BackgroundColor = locked ? EnteringColor : FailedColor;

        public override bool Picked => BackgroundColor == EnteringColor;

    }

}
