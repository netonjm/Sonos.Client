using Sonos.Client.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sonos.Client
{
    public class SonosClient
    {

        public event NotificationEventHandler NotificationEvent;
        public delegate void NotificationEventHandler(Object sender, Event e);

        protected virtual void OnNotificationEvent(Event e)
        {
            NotificationEventHandler handler = NotificationEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private string BaseUrl;
        private string BaseUrlFormat = "http://{0}:{1}";
        private int DefaultPort = 1400;
        private string SoapActionHeader = "SOAPACTION";

        private const string DeviceDescriptionUrl = "xml/device_description.xml";
        private const string MediaRendererAVTransportUrl = "MediaRenderer/AVTransport/Control";
        private const string MediaRendererRenderingControlUrl = "MediaRenderer/RenderingControl/Control";
        private const string MediaRendererAVTransportEventUrl = "MediaRenderer/AVTransport/Event";

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

        public async Task<Event> ParseNotification(string notification)
        {
            try
            {
                notification = notification.Replace("<e:propertyset xmlns:e=\"urn:schemas-upnp-org:event-1-0\"><e:property><LastChange>", "");
                notification = notification.Replace("</LastChange></e:property></e:propertyset>", "");
                notification = notification.Replace("&lt;", "<");
                notification = notification.Replace("&gt;", ">");
                notification = notification.Replace("&quot;", "\"");
                notification = notification.Replace("&amp;", "&");
                notification = notification.Replace("<r:", "<");
                notification = notification.Replace("  ", "");
                notification = notification.Replace("\t", "");
                notification = notification.Replace("xmlns:dc=&quot;http://purl.org/dc/elements/1.1/&quot; xmlns:upnp=&quot;urn:schemas-upnp-org:metadata-1-0/upnp/&quot; xmlns:r=&quot;urn:schemas-rinconnetworks-com:metadata-1-0/&quot; xmlns=&quot;urn:schemas-upnp-org:metadata-1-0/DIDL-Lite/&quot;", "");

                Regex namespaceRegex = new Regex("xmlns:*(.*?)=(\".*?\")");
                var notification2 = namespaceRegex.Replace(notification, "");
                notification2 = notification2.Replace("  >", ">");
                notification2 = notification2.Replace(" >", ">");
                var settings = new XmlReaderSettings();
                var obj = new Event();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Event));
                obj = (Event)serializer.Deserialize(new StringReader(notification2));

                try
                {
                    obj.InstanceID.CurrentTrackMetaData.TrackMeta = await GetTrackMetaData(obj.InstanceID.CurrentTrackMetaData.Val);
                }
                catch (Exception e) { }
                try
                {
                    obj.InstanceID.NextTrackMetaData.TrackMeta = await GetTrackMetaData(obj.InstanceID.NextTrackMetaData.Val);
                }
                catch (Exception e) { }

                OnNotificationEvent(obj);

                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private async Task<TrackMeta> GetTrackMetaData(string metaData)
        {
            metaData = metaData.Replace("upnp:", "");
            metaData = metaData.Replace("dc:", "");
            metaData = metaData.Replace("r:", "");
            metaData = metaData.Replace("upnp:", "");

            var trackMetaSerializer = new XmlSerializer(typeof(TrackMeta));
            TrackMeta trackMeta = (TrackMeta)trackMetaSerializer.Deserialize(new StringReader(metaData));

            return trackMeta;
        }

        public async Task<bool> Subscribe(string localIpAddress, int localPort)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add(SoapActionHeader, PlaySoapAction);
                client.DefaultRequestHeaders.Add("CALLBACK", string.Format("<http://{0}:{1}/notify>", localIpAddress, localPort));
                client.DefaultRequestHeaders.Add("TIMEOUT", "Second-3600");
                client.DefaultRequestHeaders.Add("NT", "upnp:event");
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("SUBSCRIBE"), BaseUrl + "/" + MediaRendererAVTransportEventUrl);
                HttpResponseMessage response = await client.SendAsync(request);
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

        public async Task<DeviceDescription> GetDeviceDescription()
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(BaseUrl + "/" + DeviceDescriptionUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var settings = new XmlReaderSettings();
                    var obj = new DeviceDescription();
                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DeviceDescription));
                    obj = (DeviceDescription)serializer.Deserialize(new StringReader(content));

                    return obj;
                }
                else
                {
                    return null;
                }
            }
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
