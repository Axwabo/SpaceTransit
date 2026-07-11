using System;
using System.Threading;
using SpaceTransit.Routes;
using SpaceTransit.Routes.Stops;
using SpaceTransit.Stations.Announcements.Katilects;

namespace SpaceTransit.Stations.Announcements
{

    public abstract class ScheduledAnnouncementBase<T> : StopAnnouncementBase<T> where T : IStop
    {

        private readonly TimeSpan _time;

        private readonly TimeSpan _expiry;

        protected int MinuteMark { get; }

        protected CancellationToken Cancellation { get; init; }

        public bool CustomExpiry => Cancellation.CanBeCanceled;

        protected ScheduledAnnouncementBase(RouteDescriptor route, T stop, int minuteMark, IKatilect station) : this(route, stop, minuteMark, 1, station)
        {
        }

        protected ScheduledAnnouncementBase(RouteDescriptor route, T stop, int minuteMark, int expiryMinutes, IKatilect station) : base(route, stop, station)
        {
            MinuteMark = minuteMark;
            _time = stop switch
            {
                IArrival arrival => arrival.Arrival.Value,
                IDeparture departure => departure.Departure.Value,
                _ => throw new InvalidOperationException($"No timestamp could be extracted from {stop}")
            } - TimeSpan.FromMinutes(minuteMark);
            _expiry = _time + TimeSpan.FromMinutes(expiryMinutes - 0.1);
        }

        public sealed override UpdateResult UpdateQueued() => HasExpired()
            ? UpdateResult.Remove
            : Clock.Now < _time
                ? UpdateResult.Idle
                : Stop.AnyShip()
                    ? UpdateResult.Ready
                    : UpdateResult.Idle;

        private bool HasExpired() => CustomExpiry ? Cancellation.IsCancellationRequested : Clock.Now >= _expiry;

    }

}
