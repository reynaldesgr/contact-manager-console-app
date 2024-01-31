using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ContactManagerApp
{
    /// <summary>
    /// Represents a folder in the contact manager, capable of containing other folders and contacts.
    /// This class supports custom XML serialization and deserialization.
    /// </summary>
    [Serializable]
    public class Folder : IXmlSerializable
    {
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        public string           Name             { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the folder.
        /// </summary>
        public DateTime         CreationDate     { get; set; }

        /// <summary>
        /// Gets or sets the date of the last modification of the folder.
        /// </summary>
        public DateTime         ModificationDate { get; set; }

        /// <summary>
        /// Gets or sets the list of subfolders contained within this folder.
        /// </summary>
        public List<Folder>     SubFolders       { get; set; }

        /// <summary>
        /// Gets or sets the list of contacts contained within this folder.
        /// </summary>
        public List<Contact>    Contacts         { get; set; }

        /// <summary>
        /// Initializes a new instance of the Folder class.
        /// </summary>
        public Folder() { }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface,
        /// you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required,
        /// apply the <see cref="XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>null</returns>
        public Folder(string name)
        {
            Name = name;
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
            SubFolders = new List<Folder>();
            Contacts = new List<Contact>();
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface,
        /// you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required,
        /// apply the <see cref="XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> stream from which the object is deserialized.</param>
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

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("CreationDate", CreationDate.ToString());
            writer.WriteElementString("ModificationDate", ModificationDate.ToString());

            // Write SubFolders
            if (null != SubFolders)
            {
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
            }

            // Write Contacts
            if (null != Contacts)
            {
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
}
