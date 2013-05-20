using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UPnP_Device
{
    class InvokeResponseGen
    {
        private string AppendHead(string body)
        {
			//body = body.Replace ("&gt;", ">");
			//body = body.Replace ("&lt;", "<");

            string s = "HTTP/1.1 200 OK\r\n" +
                       "CONTENT-TYPE: text/xml;charset=\"utf-8\"\r\n" +
                       "CONTENT-LENGTH: " + body.Length + "\r\n" +
                       "EXT:\r\n" +
                       "SERVER: Windows NT/5.0, UPnP/1.0 HiPi/1.0 \r\n";

            return s + "\r\n" + body;
        }

        public string InvokeResponse(string funcName, List<UPnPArg> args)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement env = doc.CreateElement("s", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            doc.AppendChild(env);
            env.SetAttribute("encodingStyle", "http://schemas.xmlsoap.org/soap/envelope/", "http://schemas.xmlsoap.org/soap/encoding/");

            XmlElement body = doc.CreateElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
            body.Prefix = "s";
            env.AppendChild(body);

            XmlElement func = doc.CreateElement("u", funcName + "Response", "urn:schemas-upnp-org:service:AVTransport:1");
            body.AppendChild(func);
            func.SetAttribute("xmlns:u", "urn:schemas-upnp-org:service:AVTransport:1");

            if (args != null)
            {
                foreach (UPnPArg s in args)
                {
                    XmlElement f = doc.CreateElement(s.ArgName);
                    func.AppendChild(f);
                    f.InnerText = s.ArgVal;
                }
            }

            //Saved for debugging:
            doc.Save(@"InvokeResponse.xml");

            string msg = AppendHead(doc.OuterXml);
            return msg;
        }
    }
}
