using Sonos.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sonos.Client
{
    public class SonosClient
    {
        private string BaseUrl;
        private string BaseUrlFormat = "http://{0}:{1}";
        private int DefaultPort = 1400;
        private string SoapActionHeader = "SOAPACTION";

        private const string DeviceDescriptionUrl = "xml/device_description.xml";
        private const string MediaRendererAVTransportUrl = "MediaRenderer/AVTransport/Control";
        private const string MediaRendererRenderingControlUrl = "MediaRenderer/RenderingControl/Control";

        private const string PlayBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Speed>1</Speed></u:Play></s:Body></s:Envelope>";
        private const string PlaySoapAction = "urn:schemas-upnp-org:service:AVTransport:1#Play";

        private const string PauseBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:Pause xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Speed>1</Speed></u:Pause></s:Body></s:Envelope>";
        private const string PauseSoapAction = "urn:schemas-upnp-org:service:AVTransport:1#Pause";

        private const string NextBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:Next xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Speed>1</Next></u:Pause></s:Body></s:Envelope>";
        private const string NextSoapAction = "urn:schemas-upnp-org:service:AVTransport:1#Next";

        private const string PreviousBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:Previous xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID><Speed>1</Previous></u:Pause></s:Body></s:Envelope>";
        private const string PreviousSoapAction = "urn:schemas-upnp-org:service:AVTransport:1#Previous";

        private const string SetVolumeBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:SetVolume xmlns:u=\"urn:schemas-upnp-org:service:RenderingControl:1\"><InstanceID>0</InstanceID><Channel>Master</Channel><DesiredVolume>{0}</DesiredVolume></u:SetVolume></s:Body></s:Envelope>";
        private const string SetVolumeSoapAction = "urn:schemas-upnp-org:service:RenderingControl:1#SetVolume";

        private const string GetPlayingBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:GetTransportInfo xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>0</InstanceID></u:GetTransportInfo></s:Body></s:Envelope> ";
        private const string GetPlayingSoapAction = "urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo";

        private const string GetVolumeBody = "<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\"><s:Body><u:GetVolume xmlns:u=\"urn:schemas-upnp-org:service:RenderingControl:1\"><InstanceID>0</InstanceID><Channel>Master</Channel></u:GetVolume></s:Body></s:Envelope>";
        private const string GetVolumeSoapAction = "urn:schemas-upnp-org:service:RenderingControl:1#GetVolume";

        public SonosClient(string ipAddress)
        {
            BaseUrl = string.Format(BaseUrlFormat, ipAddress, DefaultPort);
        }

        public SonosClient(string ipAddress, int port)
        {
            BaseUrl = string.Format(BaseUrlFormat, ipAddress, port);
        }

        public async Task<DeviceDescription> GetDeviceDescription()
        {
            //using (var client = new HttpClient())
            //{
            //client.DefaultRequestHeaders.Add(SoapActionHeader, PlaySoapAction);
            //HttpContent postContent = new StringContent(PlayBody, Encoding.UTF8, "text/xml");
            //HttpResponseMessage response = await client.GetAsync(BaseUrl + "/" + MediaRendererAVTransportUrl);
            //if (response.IsSuccessStatusCode)
            //{
            //var content = await response.Content.ReadAsStringAsync();

            var settings = new XmlReaderSettings();
            var obj = new DeviceDescription();
            var reader = XmlReader.Create(BaseUrl + "/" + DeviceDescriptionUrl, settings);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DeviceDescription));
            obj = (DeviceDescription)serializer.Deserialize(reader);

            return obj;
            /*
            StringBuilder output = new StringBuilder();

            using (XmlReader reader = XmlReader.Create(new StringReader(content)))
            {
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(output, ws))
                {

                    // Parse the file and display each of the nodes.
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                writer.WriteStartElement(reader.Name);
                                break;
                            case XmlNodeType.Text:
                                writer.WriteString(reader.Value);
                                break;
                            case XmlNodeType.XmlDeclaration:
                            case XmlNodeType.ProcessingInstruction:
                                writer.WriteProcessingInstruction(reader.Name, reader.Value);
                                break;
                            case XmlNodeType.Comment:
                                writer.WriteComment(reader.Value);
                                break;
                            case XmlNodeType.EndElement:
                                writer.WriteFullEndElement();
                                break;
                        }
                    }

                }
            }*/
            //return true;
            //}
            //else
            //    {
            //        return false;
            //    }
            //}
        }

        public async Task<bool> Play()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, PlaySoapAction);
                HttpContent postContent = new StringContent(PlayBody, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererAVTransportUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Pause()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, PauseSoapAction);
                HttpContent postContent = new StringContent(PauseBody, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererAVTransportUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Next()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, NextSoapAction);
                HttpContent postContent = new StringContent(NextBody, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererAVTransportUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Previous()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, PreviousSoapAction);
                HttpContent postContent = new StringContent(PreviousBody, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererAVTransportUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> SetVolume(int percentage)
        {
            if (percentage < 0 || percentage > 100)
                percentage = 0;

            using (var client = new HttpClient())
            {
                var body = string.Format(SetVolumeBody, percentage);
                client.DefaultRequestHeaders.Add(SoapActionHeader, SetVolumeSoapAction);
                HttpContent postContent = new StringContent(body, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererRenderingControlUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public async Task<bool> IsPlaying()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, GetPlayingSoapAction);
                HttpContent postContent = new StringContent(GetPlayingBody, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererAVTransportUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    //Set proper content encoding without quotes
                    response.Content.Headers.Remove("CONTENT-TYPE");
                    response.Content.Headers.Add("CONTENT-TYPE", "text/xml; charset=UTF-8");
                    var content = await response.Content.ReadAsStringAsync();
                    return content.Contains("PLAYING");
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<int> GetVolume()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, GetVolumeSoapAction);
                HttpContent postContent = new StringContent(GetVolumeBody, Encoding.UTF8, "text/xml");
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/" + MediaRendererRenderingControlUrl, postContent);
                if (response.IsSuccessStatusCode)
                {
                    //Set proper content encoding without quotes
                    response.Content.Headers.Remove("CONTENT-TYPE");
                    response.Content.Headers.Add("CONTENT-TYPE", "text/xml; charset=UTF-8");
                    var content = await response.Content.ReadAsStringAsync();

                    var startIndex = content.IndexOf("<CurrentVolume>") + 15;
                    var endIndex = content.IndexOf("</CurrentVolume>");
                    var volumeString = content.Substring(startIndex, endIndex - startIndex);
                    var volume = 0;
                    if (Int32.TryParse(volumeString, out volume) && volume >= 0 && volume <= 100)
                    {
                        return volume;
                    }
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
