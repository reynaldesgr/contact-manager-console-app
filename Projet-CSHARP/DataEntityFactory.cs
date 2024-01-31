using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagerApp
{
    /// <summary>
    /// Defines a factory for creating data entities such as folders and contacts.
    /// </summary>
    public interface IDataEntityFactory
    {
        Folder  CreateFolder(string name);
        Contact CreateContact(string lastName, string firstName, string email, string company, TLink link);
    }

    /// <summary>
    /// Provides implementation for the IDataEntityFactory interface, creating Folder and Contact instances.
    /// </summary>
    public class DataEntityFactory : IDataEntityFactory
    {
        /// <summary>
        /// Creates a new folder with the specified name.
        /// </summary>
        /// <param name="name">The name of the folder to create.</param>
        /// <returns>A new Folder instance with the given name.</returns>
        public Folder CreateFolder(string name)
        {
            return new Folder(name);
        }


        /// <summary>
        /// Creates a new contact with the provided details.
        /// </summary>
        /// <param name="lastName">The last name of the contact.</param>
        /// <param name="firstName">The first name of the contact.</param>
        /// <param name="email">The email address of the contact.</param>
        /// <param name="company">The company associated with the contact.</param>
        /// <param name="link">The type of link associated with the contact, assumed to be an enum.</param>
        /// <returns>A Contact instance populated with the provided details.</returns>
        public Contact CreateContact(string lastName, string firstName, string email, string company, TLink link)
        {
            return new Contact(lastName, firstName, email, company, link);
        }
    }


}
