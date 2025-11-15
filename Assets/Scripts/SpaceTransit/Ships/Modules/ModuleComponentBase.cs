namespace SpaceTransit.Ships.Modules
{

    public abstract class ModuleComponentBase : SubcomponentBase<ShipModule>
    {

        public ShipAssembly Assembly => Parent.Assembly;

        public ShipState State => Assembly.Parent.State;

        public virtual void OnStateChanged()
        {
        }

    }

}
