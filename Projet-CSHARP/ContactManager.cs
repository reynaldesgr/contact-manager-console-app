using System;
using System.Collections.Generic;
using System.IO;

namespace Projet_CSHARP
{
    class ContactManager
    {
        private Folder            root;
        private Folder            current;
        private DataEntityFactory entityFactory;
        private DataSerializer    serializer;

        public ContactManager()
        {
            entityFactory = new DataEntityFactory();
            serializer    = new DataSerializer();
            LoadData(); 
        }

        public void CreateNewFolder(string name)
        {
            // Ensure current.SubFolders is not null
            if (current.SubFolders == null)
            {
                current.SubFolders = new List<Folder>();
            }

            Folder newFolder = entityFactory.CreateFolder(name);
            current.SubFolders.Add(newFolder);
            Console.WriteLine($"New folder '{name}' created in '{current.Name}'");
        }

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

        public void SaveData()
        {
            string fileName = "contactsData.xml";

            // Get the full path of the file
            string fullPath = Path.GetFullPath(fileName);

            // Check if the file exists
            if (!File.Exists(fileName))
            {
                using (FileStream fs = File.Create(fileName))
                {
                    fs.Close();
                }
            }
            serializer.SerializeToFile(root, fileName);
            Console.WriteLine("Data saved successfully in " + fullPath + ".");
        }

        public void LoadData()
        {
            string fileName = "contactsData.xml";
            if (File.Exists(fileName))
            {
                root = serializer.DeserializeFromFile<Folder>(fileName);
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

        public void DisplayStructure(Folder folder, int depth)
        {
            string indent = new string(' ', depth * 4);

            Console.WriteLine($"{indent}[D] - {folder.Name}");

            if (folder.Contacts != null)
            {
                foreach (var contact in folder.Contacts)
                {
                    Console.WriteLine($"{indent} | [C] - {contact.FirstName} {contact.LastName} ({contact.Email})");
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
