using System;

namespace Projet_CSHARP
{
    class Program
    {
        static void Main(string[] args)
        {
            ContactManager contactManager = new ContactManager();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display structure");
                Console.WriteLine("2. Create folder");
                Console.WriteLine("3. Create contact");
                Console.WriteLine("4. Select current folder");
                Console.WriteLine("5. Save data");
                Console.WriteLine("6. Unload data");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        contactManager.Display();
                        break;

                    case "2":
                        Console.Write("Enter folder name: ");
                        string folderName = Console.ReadLine();
                        contactManager.CreateNewFolder(folderName);
                        break;

                    case "3":
                        Console.Write("Enter contact details:\nLast Name: ");
                        string lastName = Console.ReadLine();
                        Console.Write("First Name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Email: ");
                        string email = Console.ReadLine(); 
                        Console.Write("Company: ");
                        string company = Console.ReadLine();
                        Console.Write("Enter TLink value (Friend, Colleague, Relation, Network): ");
                        string linkInput = Console.ReadLine();

                        TLink link;

                        switch (linkInput)
                        {
                            case "Friend":
                                link = TLink.Friend;
                                break;
                            case "Colleague":
                                link = TLink.Colleague;
                                break;
                            case "Relation":
                                link = TLink.Relation;
                                break;
                            case "Network":
                                link = TLink.Network;
                                break;
                            default:
                                link = TLink.Unknown;
                                return;
                        }
                        contactManager.CreateNewContact(lastName, firstName, email, company, link);
                        break;

                    case "4":
                        Console.Write("Enter folder name to select as current: ");
                        string folderToSelect = Console.ReadLine();
                        contactManager.SelectCurrentFolder(folderToSelect);
                        break;

                    case "5":
                        contactManager.SaveData();
                        break;

                    case "6":
                        contactManager.UnloadData();
                        Console.Write("Are you sure to unload data ? [Y/N] ");
                        string answer = Console.ReadLine();
                        switch(answer)
                        {
                            case "Y":
                                contactManager.UnloadData();
                                break;
                            case "N":
                                break;
                            default:
                                break;
                        }
                        break;

                    case "7":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
    }
}
