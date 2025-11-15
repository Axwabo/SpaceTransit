namespace SpaceTransit.Ships.Modules
{

    public abstract class ShipComponentBase : SubcomponentBase<ShipController>
    {

        public ShipAssembly Assembly => Parent.Assembly;

        public virtual void OnStateChanged(ShipState previousState)
        {
        }

    }

}
