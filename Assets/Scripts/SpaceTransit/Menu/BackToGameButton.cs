namespace SpaceTransit.Menu
{

    public sealed class BackToGameButton : AutoRegisterButton
    {

        protected override void Click() => MenuScreen.Current.Toggle();

    }

}
