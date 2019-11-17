using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

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

        public static void SaveToFileAlternate(string data, string filename)
        {
            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(GetFullPath(filename), data);
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(Application.persistentDataPath + "/" + filename);
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