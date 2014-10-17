using Orchard.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace SH.Ping.Services
{
    public class PingService : IPingService {

        public PingService() {
            Logger = NullLogger.Instance;
        }
        public ILogger Logger { get; set; }

        public bool SendPings(string siteName, string siteUrl, string services)
        {
            if (string.IsNullOrWhiteSpace(siteName) || string.IsNullOrWhiteSpace(siteUrl) || string.IsNullOrWhiteSpace(services))
                return false;
            string[] urls = services.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (urls.Length == 0)
                return false;
            return urls.Select(u => SendPing(siteName, siteUrl, u.Trim())).All(r => r);
        }

        private bool SendPing(string siteName, string siteUrl, string serviceEndpointUrl) {
            try {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(serviceEndpointUrl.Trim());
                request.Method = "POST";
                request.ContentType = "text/xml";
                request.Timeout = 3000;

                using (Stream stream = request.GetRequestStream()) {
                    using (XmlTextWriter writer = new XmlTextWriter(stream, Encoding.ASCII)) {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("methodCall");
                        writer.WriteElementString("methodName", "weblogUpdates.ping");
                        writer.WriteStartElement("params");
                        writer.WriteStartElement("param");
                        writer.WriteElementString("value", siteName);
                        writer.WriteEndElement();
                        writer.WriteStartElement("param");
                        writer.WriteElementString("value", siteUrl);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                    }

                    // For now, we don't care what the response is, it could be "get lost" but we're not tracking it
                    //  so unless someone wants to or needs this more advanced for tracking dead or failed pings, this
                    //  will get ignored.
                    request.GetResponse();
                }

                return true;
            }
            catch (Exception ex) {
                // Log the error.
                Logger.Error(ex, string.Format("Error sending XML-RPC ping request to '{0}'.", serviceEndpointUrl));
                return false;
            }
        }
    }
}