using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


namespace EWGames.Dev.Scripts.Serialization
{
    public static class SerializationManager
    {
        public static bool Save(string saveName, object saveData)
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            if (!Directory.Exists(Application.persistentDataPath+"/saves"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/saves");
            }

            string path = Application.persistentDataPath + "/saves/"+saveName + ".save";

            FileStream file = File.Create(path);
            formatter.Serialize(file,saveData);
            file.Close();
            
            return true;
        }

        public static object Load(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log("Dosya bulunamadi");
                return null;
            }

            BinaryFormatter formatter = GetBinaryFormatter();

            FileStream file = File.Open(path, FileMode.Open);

            try
            {
                object save = formatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Failed to  load file at {0}",path);
                file.Close();
                return null;
            }
        }

        private static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter;
        }
    }
}