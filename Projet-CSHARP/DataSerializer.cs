using System.IO;
using System.Xml.Serialization;

namespace Projet_CSHARP
{
    public class DataSerializer
    {
        public void SerializeToFile<T>(T data, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }

        public T DeserializeFromFile<T>(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamReader reader = new StreamReader(fileName))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
