using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Always close your streams to avoid IOExceptions!

namespace MDMulti
{
    public class StorageHelper
    {
        public static string GetFullPath(string filename)
        {
            return Application.persistentDataPath + "/" + filename;
        }

        public static void SaveToFile(string data, string filename)
        {
            Debug.Log(Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetFullPath(filename));
            bf.Serialize(file, data);
            file.Close();
        }

        public static void SaveToFile(System.SerializableAttribute data, string filename)
        {
            Debug.Log(Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetFullPath(filename));
            bf.Serialize(file, data);
            file.Close();
        }

        /// <summary>
        /// Save to a File (Alternate) - Suitable for JSON files.
        /// </summary>
        /// <param name="data">The data to save.</param>
        /// <param name="filename">The filename to use.</param>
        public static void SaveToFileAlternate(string data, string filename)
        {
            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(GetFullPath(filename), data);
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(Application.persistentDataPath + "/" + filename);
        }

        /// <summary>
        /// Tests to see if the specified **existing** file is valid JSON.
        /// </summary>
        /// <param name="filename">File to test</param>
        /// <returns>async Boolean exists</returns>
        private static async Task<bool> FileIsJson(string filename)
        {
            try
            {
                JObject.Parse(Encoding.UTF8.GetString(await ReadFileByte(filename)));
                // If we get to this line then the file is valid JSON.
                return true;
            } catch (JsonReaderException)
            {
                // File is not valid JSON.
                return false;
            }
        }

        public static async Task<bool> FileExistsAndIsJson(string filename)
        {
            bool exists = FileExists(filename);

            if (!exists)
            {
                return false;
            } else
            {
                return await FileIsJson(filename);
            }
        }

        public static async Task<byte[]> ReadFileByte(string filename)
        {
            FileStream file = File.OpenRead(GetFullPath(filename));
            byte[] buffer = new byte[file.Length];
            await file.ReadAsync(buffer, 0, (int)file.Length);
            file.Close();
            return buffer;
        }

        public static byte[] ReadFileByteSync(string filename)
        {
            FileStream file = File.OpenRead(GetFullPath(filename));
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            file.Close();
            return buffer;
        }
    }
}