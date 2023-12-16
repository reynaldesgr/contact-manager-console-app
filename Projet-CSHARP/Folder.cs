using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Projet_CSHARP
{
    [Serializable]
    public class Folder : IXmlSerializable
    {
        public string           Name             { get; set; }
        public DateTime         CreationDate     { get; set; }
        public DateTime         ModificationDate { get; set; }
        public List<Folder>     SubFolders       { get; set; }
        public List<Contact>    Contacts         { get; set; }

        public Folder() { }
        public Folder(string name)
        {
            Name = name;
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            SubFolders = new List<Folder>();
            Contacts = new List<Contact>();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement(); 

            while (reader.NodeType == XmlNodeType.Element)
            {
                switch (reader.Name)
                {
                    case "Name":
                        Name = reader.ReadElementContentAsString();
                        break;

                    case "CreationDate":
                        CreationDate = DateTime.Parse(reader.ReadElementContentAsString());
                        break;

                    case "ModificationDate":
                        ModificationDate = DateTime.Parse(reader.ReadElementContentAsString());
                        break;

                    case "SubFolders":
                        reader.ReadStartElement(); 
                        SubFolders = new List<Folder>();

                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            Folder subFolder = new Folder();
                            subFolder.ReadXml(reader);
                            SubFolders.Add(subFolder);
                        }
                        reader.ReadEndElement(); 
                        break;

                    case "Contacts":
                        reader.ReadStartElement(); 
                        Contacts = new List<Contact>();

                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            Contact contact = new Contact();
                            contact.ReadXml(reader);
                            Contacts.Add(contact);
                        }
                        reader.ReadEndElement(); 
                        break;

                    default:
                        reader.Skip(); 
                        break;
                }
            }

            reader.ReadEndElement(); 
        }


        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("CreationDate", CreationDate.ToString());
            writer.WriteElementString("ModificationDate", ModificationDate.ToString());

            // Write SubFolders

            if (SubFolders.Count != 0)
            {
                writer.WriteStartElement("SubFolders");
                foreach (var subFolder in SubFolders)
                {
                    writer.WriteStartElement("Folder");
                    subFolder.WriteXml(writer);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // </SubFolders>
            }

            // Write Contacts
  
           if (Contacts.Count != 0)
            {
                writer.WriteStartElement("Contacts");
                foreach (var contact in Contacts)
                {
                    writer.WriteStartElement("Contact");
                    contact.WriteXml(writer);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // </Contacts>
            }
        }
    }
}
