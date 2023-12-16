using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_CSHARP
{
    public interface IDataEntityFactory
    {
        Folder  CreateFolder(string name);
        Contact CreateContact(string lastName, string firstName, string email, string company, TLink link);
    }

    public class DataEntityFactory : IDataEntityFactory
    {
        public Folder CreateFolder(string name)
        {
            return new Folder(name);
        }

        public Contact CreateContact(string lastName, string firstName, string email, string company, TLink link)
        {
            return new Contact(lastName, firstName, email, company, link);
        }
    }


}
