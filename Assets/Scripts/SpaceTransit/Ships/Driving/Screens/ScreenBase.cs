using SpaceTransit.Ships.Modules;

namespace SpaceTransit.Ships.Driving.Screens
{

    public abstract class ScreenBase : ModuleComponentBase
    {

        public abstract void Navigate(bool forwards);

        public abstract void Confirm();

        public abstract bool Select(int index);

    }

}
