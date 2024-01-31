using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ContactManagerApp
{
    /// <summary>
    /// Represents a contact with personal and professional details.
    /// Capable of XML serialization and deserialization to store and retrieve contact information.
    /// </summary>
    [Serializable]
    public class Contact : IXmlSerializable
    {
        /// <summary>Gets or sets the last name of the contact.</summary>
        public string   LastName               { get; set; }

        /// <summary>Gets or sets the first name of the contact.</summary>
        public string   FirstName              { get; set; }

        /// <summary>Gets or sets the email address of the contact.</summary>
        public string   Email                  { get; set; }

        /// <summary>Gets or sets the company name associated with the contact.</summary>
        public string   Company                { get; set; }

        /// <summary>Gets or sets the type of link or relationship of the contact.</summary>
        public TLink    Link                   { get; set; }

        /// <summary>Gets or sets the creation date and time of the contact record.</summary>
        public DateTime CreationDate           { get; set; }

        /// <summary>Gets or sets the last modification date and time of the contact record.</summary>
        public DateTime ModificationDate       { get; set; }


        /// <summary>
        /// Initializes a new instance of the Contact class.
        /// </summary>
        public Contact() { }

        /// <summary>
        /// Initializes a new instance of the Contact class with parameters for details.
        /// </summary>
        /// <param name="lastName">The last name of the contact.</param>
        /// <param name="firstName">The first name of the contact.</param>
        /// <param name="email">The email address of the contact.</param>
        /// <param name="company">The company associated with the contact.</param>
        /// <param name="link">The type of link representing the contact's relationship.</param>
        public Contact(string lastName, string firstName, string email, string company, TLink link)
        {
            LastName         = lastName;
            FirstName        = firstName;
            Email            = email;
            Company          = company;
            Link             = link;
            CreationDate     = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        /// <summary>
        /// This method is reserved, should not be used, and will always return null (Nothing in Visual Basic).
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() => null;

        /// <summary>
        /// Generates a contact object from its XML representation.
        /// </summary>
        /// <param name="reader">The XMLReader stream from which the object is deserialized.</param>
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

        /// <summary>
        /// Converts a contact object into its XML representation.
        /// </summary>
        /// <param name="writer">The XMLWriter stream to which the object is serialized.</param>
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
