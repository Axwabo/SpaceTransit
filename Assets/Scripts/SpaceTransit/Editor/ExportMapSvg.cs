using System.IO;
using SpaceTransit.Tubes;
using UnityEditor;
using UnityEngine;

namespace SpaceTransit.Editor
{

    public sealed class ExportMapSvg : EditorWindow
    {

        private const float Step = 1 / 60f;
        private const string StrokeStyle = "stroke=\"orange\" stroke-width=\"5\"";

        [MenuItem("Window/SpaceTransit/Export Map as SVG")]
        public static void ShowWindow()
        {
            var window = GetWindow<ExportMapSvg>();
            window.titleContent = new GUIContent("Export Map");
            window.Show();
        }

        private Transform _anchor;

        private void OnGUI()
        {
            _anchor = (Transform) EditorGUILayout.ObjectField("Anchor", _anchor, typeof(Transform), true);
            if (_anchor && GUILayout.Button("Export"))
                Export(_anchor);
        }

        private static void Export(Transform anchor)
        {
            var path = EditorUtility.SaveFilePanel("Save Map SVG", null, null, "svg");
            if (string.IsNullOrWhiteSpace(path))
                return;
            var topLeft = anchor.TransformPoint(new Vector3(-0.5f, 0, 0.25f));
            var bottomRight = anchor.TransformPoint(new Vector3(0.5f, 0, -0.25f));
            using var file = File.CreateText(path);
            file.Write("<svg viewBox=\"");
            file.Write(topLeft.x);
            file.Write(' ');
            file.Write(-topLeft.z);
            file.Write(' ');
            file.Write(bottomRight.x - topLeft.x);
            file.Write(' ');
            file.Write(topLeft.z - bottomRight.z);
            file.Write("\" xmlns=\"http://www.w3.org/2000/svg\">");
            foreach (var tube in FindObjectsByType<TubeBase>(FindObjectsSortMode.None))
                if (tube is SplineTube spline)
                    WriteSpline(spline, file);
                else
                    WriteStraight(tube, file);
            file.Write("</svg>");
        }

        private static void WriteSpline(SplineTube spline, StreamWriter file)
        {
            file.Write("<path d=\"");
            for (var i = 0f; i <= 1; i += Step)
            {
                var sample = spline.Sample(i * spline.Length).Position;
                file.Write(i is 0 ? 'M' : 'L');
                file.Write(sample.x);
                file.Write(' ');
                file.Write(-sample.z);
                file.Write(' ');
            }

            file.Write($"\" {StrokeStyle} fill=\"transparent\" />");
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
            file.Write($"\" {StrokeStyle} />");
        }

    }

}
