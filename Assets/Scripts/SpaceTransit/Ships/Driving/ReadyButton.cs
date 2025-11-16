namespace SpaceTransit.Ships.Driving
{

    public sealed class ReadyButton : ToggleButtonBase
    {

        public override void OnInteracted()
        {
            if (Controller.State == ShipState.Docked)
                Controller.MarkReady();
        }

        public override void OnStateChanged()
        {
            if (State == ShipState.WaitingForDeparture)
                Press();
            else if (State == ShipState.LiftingOff)
                Release();
        }

    }

}
