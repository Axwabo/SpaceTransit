using System.IO;
using System.Text;
using SpaceTransit.Routes;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public static class ExportMapSvg
    {

        private const string StrokeStyle = "stroke=\"orange\" stroke-width=\"5\"";
        private const string End = "</svg>";

        [MenuItem("Window/SpaceTransit/Export Map as SVG")]
        public static void Export()
        {
            var path = EditorUtility.SaveFilePanel("Save Map SVG", null, null, "svg");
            if (string.IsNullOrWhiteSpace(path) || !Selection.activeGameObject)
                return;
            WriteAsset(path);
            using var html = File.CreateText("map.svg");
            using (var original = File.OpenRead(path))
            {
                original.CopyTo(html.BaseStream);
            }

            html.BaseStream.Seek(-Encoding.UTF8.GetByteCount(End), SeekOrigin.Current);
            foreach (var station in Resources.LoadAll<StationId>("Stations"))
            {
                html.Write("<circle cx=\"");
                html.Write(station.position.x);
                html.Write("\" cy=\"");
                html.Write(-station.position.z);
                html.Write("\" fill=\"green\" r=\"20\"/>");
                html.Write("<text x=\"");
                html.Write(station.position.x);
                html.Write("\" y=\"");
                html.Write(-station.position.z - 40);
                html.Write("\" text-anchor=\"middle\">");
                html.Write(station.name);
                html.Write("</text>");
            }

            html.Write("<style>text{fill:white;font-family:sans-serif}</style>");
            html.Write(End);
        }

        private static void WriteAsset(string path)
        {
            var transform = Selection.activeGameObject.transform;
            var scale = transform.lossyScale;
            var topLeft = transform.TransformPoint(new Vector3(-0.5f, 0, 0.5f));
            using var file = File.CreateText(path);
            file.Write("<svg viewBox=\"");
            file.Write(topLeft.x);
            file.Write(' ');
            file.Write(-topLeft.z);
            file.Write(' ');
            file.Write(scale.x);
            file.Write(' ');
            file.Write(scale.z);
            file.Write("\" xmlns=\"http://www.w3.org/2000/svg\">");
            foreach (var tube in Object.FindObjectsByType<TubeBase>(FindObjectsSortMode.None))
                if (tube is SplineTube spline)
                    WriteSpline(spline, file);
                else
                    WriteStraight(tube, file);
            file.Write(End);
        }

        private static void WriteSpline(SplineTube spline, StreamWriter file)
        {
            file.Write("<path d=\"");
            var steps = Mathf.Clamp(Mathf.CeilToInt(spline.Length / 100), 10, 40);
            var size = 1f / steps;
            for (var i = 0; i < steps; i++)
                WriteSample(file, spline, i * size);
            WriteSample(file, spline, 1);
            file.Write($"\" {StrokeStyle} fill=\"transparent\"/>");
        }

        private static void WriteSample(StreamWriter file, SplineTube spline, float time)
        {
            var sample = spline.Sample(time * spline.Length).Position;
            file.Write(time is 0 ? 'M' : 'L');
            file.Write(sample.x);
            file.Write(' ');
            file.Write(-sample.z);
            file.Write(' ');
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
            file.Write($"\" {StrokeStyle}/>");
        }

    }

}
