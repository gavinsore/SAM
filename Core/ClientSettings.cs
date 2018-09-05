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
        public int StatsTick { get; set; }
        public int DiskStatsTick { get; set; }

        public bool LoadFromXML()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\clientsettings.xml"))
            {
                
                try
                {
                    XDocument xml = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + "\\clientsettings.xml");

                    foreach (var node in xml.DescendantNodes().OfType<XText>())
                    {
                        if (node.Parent.Name.ToString() == "ServerID")
                            ServerGuid = Guid.Parse(node.Value);
                        else if (node.Parent.Name.ToString() == "CompanyName")
                            CompanyName = node.Value;
                        else if (node.Parent.Name.ToString() == "SAMServerURL")
                            SAMServerURL = node.Value;
                        else if (node.Parent.Name.ToString() == "StatsTick")
                            StatsTick = int.Parse(node.Value);
                        else if (node.Parent.Name.ToString() == "DiskStatsTick")
                            DiskStatsTick = int.Parse(node.Value);
                    }

                    return true;
                }
                catch { return false; }  
            }
            else
                return false;
        }

        public bool SaveToXML(Guid p_ServerID, string p_CompanyName, string p_SAMServerURL, int p_StatsTick, int p_DiskStatsTick)
        {
            try
            {
                XDocument doc = new XDocument(new XElement("ClientSettings",
                                                   new XElement("ServerID", p_ServerID.ToString()),
                                                   new XElement("CompanyName", p_CompanyName),
                                                   new XElement("SAMServerURL", p_SAMServerURL),
                                                   new XElement("StatsTick", p_StatsTick),
                                                   new XElement("DiskStatsTick", p_DiskStatsTick)));
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
