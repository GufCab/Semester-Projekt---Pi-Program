using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UPnP_Device
{
    class InvokeResponseGen
    {
        public string InvokeResponseHead(int bodyLength)
        {
            string s = "HTTP/1.1 200 OK\r\n" +
                      "CONTENT-TYPE: text/xml;charset=utf-8\r\n" +
                      "CONTENT-LENGTH: " + bodyLength + "\r\n";
        }

        public string InvokeResponseBody(string funcName, List<Tuple<string, string>> args)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement env = doc.CreateElement("s:Envelope");
            doc.AppendChild(env);
            env.SetAttribute("xmlns:s", "http://schemas.xmlsoap.org/soap/envelope/");
            env.SetAttribute("s:encodingStyle", "http://schemas.xmlsoap.org/soap/encoding/");

            XmlElement body = doc.CreateElement("s:Body");
            env.AppendChild(body);

            XmlElement func = doc.CreateElement("u:" + funcName + "Response");
            body.AppendChild(func);
            func.SetAttribute("xmlns:u", "urn:schemas-upnp-org:service:AVTransport:1");

            foreach (Tuple<string, string> s in args)
            {
                XmlElement f = doc.CreateElement(s.Item1);
                func.AppendChild(f);
                f.InnerText = s.Item2;
            }
            //Saved for debugging:
            doc.Save(@"InvokeResponse.xml");

            return doc.OuterXml;
        }
    }
}
