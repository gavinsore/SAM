using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;

namespace Core
{
    public class ClientSettings
    {
        public Guid ServerGuid { get; set; }
        public string CompanyName { get; set; }
        public string SAMServerURL { get; set; }

        public bool LoadFromXML()
        {
            if (File.Exists("clientsettings.xml"))
            {

                try
                {
                    XDocument xml = XDocument.Load("clientsettings.xml");

                    foreach (var node in xml.DescendantNodes().OfType<XText>())
                    {
                        if (node.Parent.Name.ToString() == "ServerID")
                            ServerGuid = Guid.Parse(node.Value);
                        else if (node.Parent.Name.ToString() == "CompanyName")
                            CompanyName = node.Value;
                        else if (node.Parent.Name.ToString() == "SAMServerURL")
                            SAMServerURL = node.Value;
                    }

                    return true;
                }
                catch { return false; }
            }
            else
                return false;
        }

        public bool SaveToXML(Guid p_ServerID, string p_CompanyName, string p_SAMServerURL)
        {
            try
            {
                XDocument doc = new XDocument(new XElement("ClientSettings",
                                                   new XElement("ServerID", p_ServerID.ToString()),
                                                   new XElement("CompanyName", p_CompanyName),
                                                   new XElement("SAMServerURL", p_SAMServerURL)));
                doc.Save("clientsettings.xml");
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
