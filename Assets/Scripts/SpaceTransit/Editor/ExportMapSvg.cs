using System.IO;
using UnityEditor;

namespace SpaceTransit.Editor
{

    public static class ExportMapSvg
    {

        [MenuItem("Window/SpaceTransit/Export Map as SVG")]
        public static void Export()
        {
            var path = EditorUtility.SaveFilePanel("Save Map SVG", null, null, "svg");
            if (string.IsNullOrWhiteSpace(path))
                return;
            // var tubes = Object.FindObjectsByType<TubeBase>();

            /*foreach (var tubeBase in Object.FindObjectsByType<TubeBase>())
            {

                new XElement().CreateWriter()
            }*/

            using var file = File.CreateText(path);
            file.Write("<svg viewBox=\"0 0 100 100\" xmlns=\"http://www.w3.org/2000/svg\">");
            file.Write("<path d=\"");
            file.Write("M 10,30 A 20,20 0,0,1 50,30 A 20,20 0,0,1 90,30 Q 90,60 50,90 Q 10,60 10,30 z");
            file.Write("\" />");
            file.Write("</svg>");
        }

    }

}
