using System;
using System.Xml.Serialization;

namespace QRZ
{
    [XmlRoot(ElementName = "QRZDatabase")]
    public class Response
    {
        [XmlElement(ElementName = "Callsign")]
        public Person Person { get; set; }

        [XmlElement(ElementName = "Session")]
        public Session Session { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }

    [XmlRoot(ElementName = "Callsign")]
    public class Person
    {
        [XmlElement(ElementName = "call")]
        public string Call { get; set; }

        [XmlElement(ElementName = "aliases")]
        public string Aliases { get; set; }

        [XmlElement(ElementName = "dxcc")]
        public int DXCC { get; set; }

        [XmlElement(ElementName = "fname")]
        public string FirstName { get; set; }

        [XmlElement(ElementName = "name")]
        public string LastName { get; set; }

        [XmlElement(ElementName = "addr1")]
        public string Address1 { get; set; }

        [XmlElement(ElementName = "addr2")]
        public string Address2 { get; set; }

        [XmlElement(ElementName = "state")]
        public string State { get; set; }

        [XmlElement(ElementName = "zip")]
        public string Zip { get; set; }

        [XmlElement(ElementName = "country")]
        public string Country { get; set; }

        [XmlElement(ElementName = "ccode")]
        public int CountryCode { get; set; }

        [XmlElement(ElementName = "lat")]
        public decimal Latitude { get; set; }

        [XmlElement(ElementName = "lon")]
        public decimal Longitude { get; set; }

        [XmlElement(ElementName = "grid")]
        public string Grid { get; set; }

        [XmlElement(ElementName = "county")]
        public string County { get; set; }

        [XmlElement(ElementName = "fips")]
        public string FIPS { get; set; }

        [XmlElement(ElementName = "land")]
        public string Land { get; set; }

        [XmlElement(ElementName = "efdate")]
        public DateTime EffectiveDate { get; set; }

        [XmlElement(ElementName = "expdate")]
        public DateTime ExpirationDate { get; set; }

        [XmlElement(ElementName = "p_call")]
        public string PreviousCall { get; set; }

        [XmlElement(ElementName = "class")]
        public string LicenseClass { get; set; }

        [XmlElement(ElementName = "codes")]
        public string Codes { get; set; }

        [XmlElement(ElementName = "qslmgr")]
        public string QSLManager { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "url")]
        public string URL { get; set; }

        [XmlElement(ElementName = "u_views")]
        public int Views { get; set; }

        [XmlElement(ElementName = "bio")]
        public string Bio { get; set; }

        [XmlElement(ElementName = "image")]
        public string Image { get; set; }

        [XmlElement(ElementName = "serial")]
        public int Serial { get; set; }

        [XmlElement(ElementName = "moddate")]
        public DateTime ModifiedDate { get; set; }

        [XmlElement(ElementName = "MSA")]
        public int MSA { get; set; }

        [XmlElement(ElementName = "AreaCode")]
        public int AreaCode { get; set; }

        [XmlElement(ElementName = "TimeZone")]
        public string TimeZone { get; set; }

        [XmlElement(ElementName = "GMTOffset")]
        public int GMTOffset { get; set; }

        [XmlElement(ElementName = "DST")]
        public string DST { get; set; }

        [XmlElement(ElementName = "eqsl")]
        public string EQSL { get; set; }

        [XmlElement(ElementName = "mqsl")]
        public string MQSL { get; set; }

        [XmlElement(ElementName = "cqzone")]
        public int CQZone { get; set; }

        [XmlElement(ElementName = "ituzone")]
        public int ITUZone { get; set; }

        [XmlElement(ElementName = "geoloc")]
        public string Geoloc { get; set; }

        [XmlElement(ElementName = "attn")]
        public string Attention { get; set; }

        [XmlElement(ElementName = "nickname")]
        public string Nickname { get; set; }

        [XmlElement(ElementName = "name_fmt")]
        public string NameFormatted { get; set; }

        [XmlElement(ElementName = "born")]
        public int BirthYear { get; set; }
    }

    public class Session
    {
        [XmlElement(ElementName = "Key")]
        public string Key { get; set; }

        [XmlElement(ElementName = "Count")]
        public int Count { get; set; }

        [XmlElement(ElementName = "SubExp")]
        public string SubscriptionExpiration { get; set; }

        [XmlElement(ElementName = "GMTime")]
        public string GMTime { get; set; }

        [XmlElement(ElementName = "Error")]
        public string Error { get; set; }
    }
}