using System.Xml.Linq;
using UnityEngine;

namespace Services.Localization
{
    public static class XmlFileParse
    {
        public static XDocument ParseXmlFromResources(string fileName)
        {
            TextAsset textFile = Resources.Load<TextAsset>("Languages/" + fileName);
            return XDocument.Parse(textFile.text);
        }
    }
}