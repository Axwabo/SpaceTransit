using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public static class Screenshot
    {

        [MenuItem("Assets/Screenshot")]
        public static void Take() => ScreenCapture.CaptureScreenshot("Assets/Screenshot.png");

    }

}
