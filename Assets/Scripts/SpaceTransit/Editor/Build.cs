using System.IO;
using System.Linq;
using UnityEditor;

// ReSharper disable once CheckNamespace
public static class Build
{

    private const string Folder = "Build";

    public static void Windows() => BuildFor(BuildTarget.StandaloneWindows64, "exe");

    private static void BuildFor(BuildTarget target, string extension)
    {
        Directory.CreateDirectory(Folder);
        BuildPipeline.BuildPlayer(new BuildPlayerOptions
        {
            target = target,
            locationPathName = Path.Combine(Folder, $"SpaceTransit.{extension}"),
            scenes = EditorBuildSettings.scenes.Select(e => e.path).ToArray()
        });
    }

}
