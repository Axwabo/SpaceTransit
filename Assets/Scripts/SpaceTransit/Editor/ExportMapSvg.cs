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

            /*foreach (var tubeBase in Object.FindObjectsByType<TubeBase>())
            {

                new XElement().CreateWriter()
            }*/

            using var file = File.Create(path);
            using var writer = XmlWriter.Create(file);
            writer.WriteStartElement("svg");
            writer.WriteAttributeString("xmlns", "http://www.w3.org/2000/svg");
            writer.WriteAttributeString("viewBox", "0 0 100 100");
            writer.WriteStartElement("path");
            writer.WriteAttributeString("d", "M 10,30 A 20,20 0,0,1 50,30 A 20,20 0,0,1 90,30 Q 90,60 50,90 Q 10,60 10,30 z");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        private static XElement NoNamespace(XElement xElement)
        {
            xElement.Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            return xElement;
        }

    }

}
