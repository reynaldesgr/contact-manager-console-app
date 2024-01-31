using System.IO;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace ContactManagerApp
{
    /// <summary>
    /// Provides functionality to serialize and deserialize data with encryption and decryption.
    /// </summary>
    public class DataSerializer
    {
        /// <summary>
        /// Serializes an object to a file with encryption.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="data">The object to serialize.</param>
        /// <param name="fileName">The name of the file to serialize the object to.</param>
        /// <param name="encryptionKey">The encryption key used for encrypting the data.</param>
        /// <remarks>
        /// The encryption key is padded or trimmed to ensure it matches the required length.
        /// The method uses AES encryption.
        /// </remarks>
        public void SerializeToFile<T>(T data, string fileName, string encryptionKey)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(encryptionKey.PadRight(32));
                aesAlg.Key = keyBytes.Take(aesAlg.KeySize / 8).ToArray();
                aesAlg.IV = new byte[16];

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            serializer.Serialize(writer, data);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deserializes an object from a file with decryption.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="fileName">The name of the file to deserialize the object from.</param>
        /// <param name="decryptionKey">The decryption key used for decrypting the data.</param>
        /// <returns>The deserialized object of type T.</returns>
        /// <remarks>
        /// The decryption key is padded or trimmed to ensure it matches the required length.
        /// The method uses AES decryption.
        /// </remarks>
        public T DeserializeFromFile<T>(string fileName, string decryptionKey)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (Aes aesAlg = Aes.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(decryptionKey.PadRight(32));
                aesAlg.Key = keyBytes.Take(aesAlg.KeySize / 8).ToArray();
                aesAlg.IV = new byte[16];

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptoStream))
                        {
                            return (T)serializer.Deserialize(reader);
                        }
                    }
                }
            }
        }
    }
}
