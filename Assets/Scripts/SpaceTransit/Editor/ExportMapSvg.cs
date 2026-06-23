using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
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
            XNamespace ns = "http://www.w3.org/2000/svg";
            var root = new XElement(
                ns + "svg",
                new XAttribute("viewBox", "0 0 100 100"),
                NoNamespace(new XElement("path", new XAttribute("d", "M 10,30 A 20,20 0,0,1 50,30 A 20,20 0,0,1 90,30 Q 90,60 50,90 Q 10,60 10,30 z")))
            );
            /*foreach (var tubeBase in Object.FindObjectsByType<TubeBase>())
            {

                new XElement().CreateWriter()
            }*/

            using var file = File.Create(path);
            using var writer = XmlWriter.Create(file);
            root.WriteTo(writer);
        }

        private static XElement NoNamespace(XElement xElement)
        {
            xElement.Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            return xElement;
        }

    }

}
