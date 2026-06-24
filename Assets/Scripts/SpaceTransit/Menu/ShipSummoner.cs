using SpaceTransit.Build;
using SpaceTransit.Movement;
using SpaceTransit.Routes;
using SpaceTransit.Ships;
using UnityEngine;

namespace SpaceTransit.Menu
{

    public sealed class ShipSummoner : AutoRegisterButton
    {

        public static GameObject CurrentShip { get; private set; }

        public static void Destroy()
        {
            if (!CurrentShip)
                return;
            Destroy(CurrentShip);
            CurrentShip = null;
        }

        [SerializeField]
        private ShipAssembly assembly;

        protected override void Click()
        {
            if (CurrentShip || !MovementController.Current || TryFindDock(out var dock))
                return;
            var ship = Instantiate(assembly, World.Current);
            ship.startTube = dock;
            CurrentShip = ship.gameObject;
        }

        private static bool TryFindDock(out Dock dock)
        {
            if (Physics.Raycast(
                    MovementController.Current.LastPosition,
                    Vector3.down,
                    out var result,
                    1,
                    LayerMask.GetMask("Dock")
                ) && result.collider.TryGetComponent(out DockPlane plane) && !plane.Dock.Safety.IsOccupied)
            {
                dock = plane.Dock;
                return true;
            }

            dock = null;
            return false;
        }

    }

}
