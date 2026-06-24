namespace SpaceTransit.Menu
{

    public sealed class ShipDestroyer : AutoRegisterButton
    {

        protected override void Click() => ShipSummoner.Destroy();

    }

}
