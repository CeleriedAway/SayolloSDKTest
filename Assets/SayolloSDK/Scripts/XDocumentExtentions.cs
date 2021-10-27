using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SayolloSDK
{
    public static class XDocumentExtentions
    {
        public static Dictionary<string, string> ToDictionary(this XDocument doc)
        {
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

            foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;
                while (dataDictionary.ContainsKey(keyName))
                {
                    keyName = element.Name.LocalName + "_" + keyInt++;
                }
                dataDictionary.Add(keyName, element.Value);
            }
            return dataDictionary;
        }
    }
}
