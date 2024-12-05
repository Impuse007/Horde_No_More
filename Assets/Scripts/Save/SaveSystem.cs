using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        public static string _savePath = Path.Combine(Application.persistentDataPath, "gamedata.save");

        public static void SaveGame(SkillTree skillTree)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_savePath, FileMode.Create))
            {
                GameData data = new GameData(skillTree);
                formatter.Serialize(stream, data);
            }
        }

        public static GameData LoadGame()
        {
            if (File.Exists(_savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(_savePath, FileMode.Open))
                {
                    return formatter.Deserialize(stream) as GameData;
                }
            }
            else
            {
                Debug.LogError("Save file not found in " + _savePath);
                return null;
            }
        }
    }
}