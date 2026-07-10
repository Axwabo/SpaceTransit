using SpaceTransit.Routes;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed record RoutePicker(JourneyDescriptorBase Descriptor) : PickerBase
    {

        public static RoutePicker ExitService { get; } = new((JourneyDescriptorBase) null);

    }

}
