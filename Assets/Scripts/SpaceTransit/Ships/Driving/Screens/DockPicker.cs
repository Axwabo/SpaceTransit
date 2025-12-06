using UnityEngine;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed class DockPicker : PickerBase
    {

        private static readonly Color EnteringColor = new(0, 1, 0, 0.24f);
        private static readonly Color FailedColor = new(1, 0, 0, 0.24f);

        protected override string Text
        {
            set => text.text = value;
        }

        public void Pick(bool locked) => BackgroundColor = locked ? EnteringColor : FailedColor;

    }

}
