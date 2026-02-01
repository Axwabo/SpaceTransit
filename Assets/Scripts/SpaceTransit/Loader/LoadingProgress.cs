namespace SpaceTransit.Loader
{

    public sealed class LoadingProgress
    {

        public int Total { get; }

        public int Current { get; set; }

        public LoadingProgress(int total) => Total = total;

    }

}
