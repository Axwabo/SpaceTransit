namespace SpaceTransit.Loader
{

    public sealed class LoadingProgress
    {

        public static LoadingProgress Current { get; set; }

        public int Total { get; }

        public int Completed { get; set; }

        public LoadingProgress(int total) => Total = total;

    }

}
