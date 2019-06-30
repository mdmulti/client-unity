using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MDMulti
{
    public class StorageHelper
    {
        public static void SaveToFile(string data, string filename)
        {
            Debug.Log(Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
            bf.Serialize(file, data);
            file.Close();
        }

        public static void SaveToFile(System.SerializableAttribute data, string filename)
        {
            Debug.Log(Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
            bf.Serialize(file, data);
            file.Close();
        }
    }
}