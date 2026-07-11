using System;
using Unity.Properties;

namespace SpaceTransit.Stations
{

    [Serializable]
    public sealed record StopItem([property: CreateProperty] string Station, [property: CreateProperty] string Time);

}
