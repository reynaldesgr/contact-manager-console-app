using System.Xml.Serialization;

namespace ContactManagerApp
{
    /// <summary>
    /// Enumerates the types of links or relationships that can exist between the user and their contacts.
    /// </summary>
    public enum TLink
    {
        /// <summary>
        /// Represents a friend relationship.
        /// </summary>
        [XmlEnum(Name = "Friend")]
        Friend,

        /// <summary>
        /// Represents a colleague relationship, typically used for work-related contacts.
        /// </summary>
        [XmlEnum(Name = "Colleague")]
        Colleague,

        /// <summary>
        /// Represents a familial or close personal relationship.
        /// </summary>
        [XmlEnum(Name = "Relation")]
        Relation,

        /// <summary>
        /// Represents contacts within a professional network, not necessarily close colleagues.
        /// </summary>
        [XmlEnum(Name = "Network")]
        Network,

        /// <summary>
        /// Represents a contact whose relationship type is not known or does not fit other categories.
        /// </summary>
        [XmlEnum(Name = "Unknown")]
        Unknown
    }
}
