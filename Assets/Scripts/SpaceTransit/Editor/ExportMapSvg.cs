using System.IO;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;

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
            using var file = File.CreateText(path);
            file.Write("<svg viewBox=\"-8192 -4096 16384 8192\" xmlns=\"http://www.w3.org/2000/svg\">");
            foreach (var tube in Object.FindObjectsByType<TubeBase>(FindObjectsSortMode.None))
            {
                if (tube is not StraightTube straight)
                    continue;
                var start = straight.Sample(0).Position;
                var end = straight.Sample(straight.Length).Position;
                file.Write("<line x1=\"");
                file.Write(start.x);
                file.Write("\" x2=\"");
                file.Write(end.x);
                file.Write("\" y1=\"");
                file.Write(start.z);
                file.Write("\" y2=\"");
                file.Write(end.z);
                file.Write("\" stroke=\"orange\" stroke-width=\"2\" />");
                // file.Write("<path d=\"");
                // file.Write("M 10,30 A 20,20 0,0,1 50,30 A 20,20 0,0,1 90,30 Q 90,60 50,90 Q 10,60 10,30 z");
                // file.Write("\" />");
            }

            file.Write("</svg>");
        }

    }

}
