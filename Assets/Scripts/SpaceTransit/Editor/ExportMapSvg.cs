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
                if (tube is SplineTube spline)
                    WriteSpline(spline, file);
                else
                    WriteStraight(tube, file);

            file.Write("</svg>");
        }

        private static void WriteSpline(SplineTube spline, StreamWriter file)
        {
            var nodes = spline.Nodes;
            file.Write("<path d=\"");
            file.Write("M ");
            file.Write(nodes[0].Position.x);
            file.Write(' ');
            file.Write(-nodes[0].Position.z);
            file.Write(' ');
            for (var i = 0; i < nodes.Count - 1; i++)
            {
                var n1 = nodes[i];
                var n2 = nodes[i + 1];
                file.Write("C ");
                file.Write(n1.Direction.x);
                file.Write(' ');
                file.Write(-n1.Direction.z);
                file.Write(", ");
                file.Write(n2.Direction.x);
                file.Write(' ');
                file.Write(-n2.Direction.z);
                file.Write(", ");
                file.Write(n2.Position.x);
                file.Write(' ');
                file.Write(-n2.Position.z);
                file.Write(' ');
            }

            file.Write("\" stroke=\"orange\" stroke-width=\"100\" />");
        }

        private static void WriteStraight(TubeBase tube, StreamWriter file)
        {
            var start = tube.Sample(0).Position;
            var end = tube.Sample(tube.Length).Position;
            file.Write("<line x1=\"");
            file.Write(start.x);
            file.Write("\" x2=\"");
            file.Write(end.x);
            file.Write("\" y1=\"");
            file.Write(-start.z);
            file.Write("\" y2=\"");
            file.Write(-end.z);
            file.Write("\" stroke=\"orange\" stroke-width=\"100\" />");
        }

    }

}
