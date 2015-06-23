using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sonos.Client.Models
{
    [XmlRoot(ElementName = "property", Namespace = "urn:schemas-upnp-org:event-1-0")]
    public class Property
    {
        [XmlElement(ElementName = "UpdateID")]
        public string UpdateID { get; set; }
        [XmlElement(ElementName = "ThirdPartyHash")]
        public string ThirdPartyHash { get; set; }
        [XmlElement(ElementName = "LastChange")]
        public string LastChange { get; set; }
    }

    [XmlRoot(ElementName = "propertyset", Namespace = "urn:schemas-upnp-org:event-1-0")]
    public class Notification
    {
        [XmlElement(ElementName = "property", Namespace = "urn:schemas-upnp-org:event-1-0")]
        public List<Property> Property { get; set; }
        [XmlAttribute(AttributeName = "e", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string E { get; set; }
    }
}
