using System;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;

namespace SpaceTransit.Stations.Announcements
{

    public abstract class ScheduledAnnouncementBase<T> : StopAnnouncementBase<T> where T : IStop
    {

        public TimeSpan Time { get; }

        public TimeSpan Expiry { get; }

        protected ScheduledAnnouncementBase(RouteDescriptor route, T stop, int minuteMark, IKatilect station) : this(route, stop, minuteMark, 1, station)
        {
        }

        protected ScheduledAnnouncementBase(RouteDescriptor route, T stop, int minuteMark, int expiryMinutes, IKatilect station) : base(route, stop, station)
        {
            Time = stop switch
            {
                IArrival arrival => arrival.Arrival.Value,
                IDeparture departure => departure.Departure.Value,
                _ => throw new InvalidOperationException($"No timestamp could be extracted from {stop}")
            } - TimeSpan.FromSeconds(minuteMark);
            Expiry = Time + TimeSpan.FromMinutes(expiryMinutes - 0.9);
        }

        public sealed override UpdateResult UpdateQueued() => Clock.Now < Time
            ? UpdateResult.Idle
            : Clock.Now >= Expiry
                ? UpdateResult.Remove
                : UpdateActive();

        protected abstract UpdateResult UpdateActive();

    }

}
