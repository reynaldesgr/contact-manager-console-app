using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


// Cyptography
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace ContactManagerApp
{
    /// <summary>
    /// Manages the operations for contact and folder data within the application.
    /// This includes creation, serialization, and deserialization of contact and folder information.
    /// </summary>
    class ContactManager
    {
        private Folder            root;
        private Folder            current;
        private DataEntityFactory entityFactory;
        private DataSerializer    serializer;

        private string            encryptionKey;
        private string            decryptionKey;

        /// <summary>
        /// Initializes a new instance of the ContactManagerApp class. Sets up the factory, serializer,
        /// and loads existing data if available.
        /// </summary>
        public ContactManager()
        {
            entityFactory = new DataEntityFactory();
            serializer    = new DataSerializer();
            encryptionKey = GetEncryptionKey();
            LoadData(); 
        }

        /// <summary>
        /// Creates a new folder with the given name and adds it to the current folder's subfolders.
        /// </summary>
        /// <param name="name">The name of the new folder.</param>
        public void CreateNewFolder(string name)
        {
            if (current.SubFolders == null)
            {
                current.SubFolders = new List<Folder>();
            }

            if (current.SubFolders.Any(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"A folder named '{name}' already exists in '{current.Name}'.");
                return; 
            }

            Folder newFolder = entityFactory.CreateFolder(name);
            current.SubFolders.Add(newFolder);
            Console.WriteLine($"New folder '{name}' created in '{current.Name}'");
        }

        /// <summary>
        /// Creates a new contact with the given details and adds it to the current folder's contacts.
        /// </summary>
        /// <param name="lastName">The last name of the contact.</param>
        /// <param name="firstName">The first name of the contact.</param>
        /// <param name="email">The email address of the contact.</param>
        /// <param name="company">The company name associated with the contact.</param>
        /// <param name="link">The type of link or relationship of the contact.</param>
        public void CreateNewContact(string lastName, string firstName, string email, string company, TLink link)
        {
            if (current.Contacts == null)
            {
                current.Contacts = new List<Contact>();
            }

            Contact newContact = entityFactory.CreateContact(lastName, firstName, email, company, link);
            current.Contacts.Add(newContact);
            Console.WriteLine($"New contact '{firstName} {lastName}' created in '{current.Name}'.");
        }

        /// <summary>
        /// Selects the current working folder based on the provided folder name.
        /// </summary>
        /// <param name="folderName">The name of the folder to set as current.</param>
        public void SelectCurrentFolder(string folderName)
        {
            Folder selected = FindFolderByName(root, folderName);
            if (selected != null)
            {
                current = selected;
                Console.WriteLine($"Current folder set to '{folderName}'.");
            }
            else
            {
                Console.WriteLine($"Folder '{folderName}' not found.");
            }
        }

        public void Display()
        {
            DisplayStructure(root, 0);
        }

        /// <summary>
        /// Saves the current contact and folder data to an XML file, encrypting the content with the set encryption key.
        /// </summary>
        public void SaveData()
        {
            string fileName = "contactsData.xml";
            string fullPath = Path.GetFullPath(fileName);

            if (!File.Exists(fileName))
            {
                using (FileStream fs = File.Create(fileName))
                {
                    fs.Close();
                }
            }
            if (string.IsNullOrEmpty(encryptionKey))
            {
                encryptionKey = GetEncryptionKey(); 
            }

            serializer.SerializeToFile(root, fileName, encryptionKey);
            Console.WriteLine("Data saved successfully in " + fullPath + ".");
        }

        private string GetEncryptionKey()
        {
            string usid = WindowsIdentity.GetCurrent().User.Value;

            using (SHA256 sha256 = SHA256.Create()) 
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(usid));
                return Convert.ToBase64String(hash);
            }
        }

        public void LoadData()
        {
            string fileName = "contactsData.xml";

            if (File.Exists(fileName))
            {
                if (string.IsNullOrEmpty(decryptionKey))
                {
                    decryptionKey = GetEncryptionKey();
                }

                root = serializer.DeserializeFromFile<Folder>(fileName, decryptionKey);
                current = root;
                Console.WriteLine("Data loaded successfully.");
            }
            else
            {
                root = entityFactory.CreateFolder("root");
                current = root;
                Console.WriteLine("No existing data. Created a new root folder.");
            }
        }

        public void UnloadData()
        {
            root = entityFactory.CreateFolder("root");
            current = root;
            Console.WriteLine("Data unloaded successfully.");
        }

        /// <summary>
        /// Displays the hierarchical structure of folders and contacts starting from a specified folder.
        /// </summary>
        /// <param name="folder">The starting folder to display the structure of.</param>
        /// <param name="depth">The depth of indentation for displaying the structure.</param>
        public void DisplayStructure(Folder folder, int depth)
        {
            string indent = new string(' ', depth * 4);

            Console.WriteLine($"{indent}[D] - {folder.Name}");

            if (folder.Contacts != null)
            {
                foreach (var contact in folder.Contacts)
                {
                    Console.WriteLine($"{indent} | [C] - {contact.FirstName} {contact.LastName} from {contact.Company} - {contact.Link} ({contact.Email})");
                }

            }
            if (folder.SubFolders != null)
            {
                foreach (var subFolder in folder.SubFolders)
                {
                    DisplayStructure(subFolder, depth + 1);
                }
            }
        }

        private Folder FindFolderByName(Folder folder, string folderName)
        {
            if (folder.Name.Equals(folderName, StringComparison.OrdinalIgnoreCase))
            {
                return folder;
            }

            if (null != folder.SubFolders)
            {
                foreach (var subfolder in folder.SubFolders)
                {
                    var foundFolder = FindFolderByName(subfolder, folderName);
                    if (foundFolder != null)
                    {
                        return foundFolder;
                    }
                }

            }
            return null;
        }
    }
}
