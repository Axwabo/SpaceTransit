using SpaceTransit.Ships.Modules.Displays;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenBase : ModuleUIComponent
    {

        public abstract void Navigate(bool forwards);

        public abstract void Confirm();

        public abstract bool Select(int index);

    }

}
