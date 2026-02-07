using System.Collections.Generic;
using SpaceTransit.Routes;
using UnityEngine;

namespace SpaceTransit.Vaulter
{

    public sealed class RouteManager : MonoBehaviour
    {

        private static readonly List<ServiceSequence> Purge = new();

        private static ServiceSequence[] _sequences;

        public static ServiceSequence[] Sequences => _sequences ??= Resources.LoadAll<ServiceSequence>("Services");

        public static RouteManager Current { get; private set; }

        private readonly Dictionary<ServiceSequence, RouteRotor> _rotors = new();

        private readonly List<RouteRotor> _new = new();

        private float _wait;

        private void Awake() => Current = this;

        public void RefreshLines()
        {
            foreach (var sequence in Sequences)
                if (!_rotors.ContainsKey(sequence) && ShouldLoadSequence(sequence))
                    _new.Add(new RouteRotor(sequence));
        }

        private void Update()
        {
            if (_new.Count != 0)
            {
                foreach (var rotor in _new)
                    rotor.Initialize();
                _new.RemoveAll(static e => e.Destroyed);
                foreach (var rotor in _new)
                    _rotors.TryAdd(rotor.Sequence, rotor);
                _new.Clear();
            }

            if ((_wait -= Clock.UnscaledDelta) > 0)
                return;
            _wait = 0.5f;
            foreach (var rotor in _rotors.Values)
                rotor.Update();
            PurgeUnloaded();
        }

        private void PurgeUnloaded()
        {
            foreach (var (sequence, rotor) in _rotors)
                if (rotor.Destroyed)
                    Purge.Add(sequence);
            foreach (var sequence in Purge)
                _rotors.Remove(sequence);
            Purge.Clear();
        }

        private static bool ShouldLoadSequence(ServiceSequence sequence)
        {
            foreach (var route in sequence.routes)
            {
                if (route.Origin.Station.IsLoaded() || route.Destination.Station.IsLoaded())
                    return true;
                foreach (var stop in route.IntermediateStops)
                    if (stop.Station.IsLoaded())
                        return true;
            }

            return false;
        }

    }

}
