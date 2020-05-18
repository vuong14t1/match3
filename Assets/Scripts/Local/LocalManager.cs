using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace Local
{
    public class LocalManager
    {    
        public static void SaveGame()
        {
            DataLocal dataLocal = DataLocal.CreateData();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave1.save");
            bf.Serialize(file, dataLocal);
            file.Close();
            Debug.Log("Game Saved");
        }

        public static DataLocal LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave1.save"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave1.save", FileMode.Open);
                DataLocal dataLocal = (DataLocal)bf.Deserialize(file);
                file.Close();
                return dataLocal;
            }else
            {
                Debug.Log("No game saved!");
            }

            return null;
        }
    }
}