using SpaceTransit.Routes;

namespace SpaceTransit.Ships.Driving.Screens
{

    public sealed record RoutePicker(RouteDescriptor Descriptor) : PickerBase
    {

        public static RoutePicker ExitService { get; } = new((RouteDescriptor) null);

    }

}
