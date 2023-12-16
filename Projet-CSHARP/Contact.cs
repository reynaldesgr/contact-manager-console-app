using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Projet_CSHARP
{
    [Serializable]
    public class Contact : IXmlSerializable
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public TLink Link { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }

        public Contact() { }
        public Contact(string lastName, string firstName, string email, string company, TLink link)
        {
            LastName = lastName;
            FirstName = firstName;
            Email = email;
            Company = company;
            Link = link;
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();

            LastName            = reader.ReadElementContentAsString("LastName", "");
            FirstName           = reader.ReadElementContentAsString("FirstName", "");
            Email               = reader.ReadElementContentAsString("Email", "");
            Company             = reader.ReadElementContentAsString("Company", "");
            Link                = new TLink(); 
            Link                = (TLink)Enum.Parse(typeof(TLink), reader.ReadElementContentAsString("Link", ""));
            CreationDate        = DateTime.Parse(reader.ReadElementContentAsString("CreationDate", ""));
            ModificationDate    = DateTime.Parse(reader.ReadElementContentAsString("ModificationDate", ""));

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("LastName", LastName);
            writer.WriteElementString("FirstName", FirstName);
            writer.WriteElementString("Email", Email);
            writer.WriteElementString("Company", Company);
            writer.WriteElementString("Link", Link.ToString());
            writer.WriteElementString("CreationDate", CreationDate.ToString());
            writer.WriteElementString("ModificationDate", ModificationDate.ToString());
        }
    }
}
