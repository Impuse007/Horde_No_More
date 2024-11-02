using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Save
{
    public static class SaveSystem
    {
        public static string _savePath = Application.persistentDataPath + "/gamedata.save";

        public static void SaveGame(PlayerController player, SkillTree skillTree)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(_savePath, FileMode.Create);

            GameData data = new GameData(player, skillTree);
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static GameData LoadGame()
        {
            if (File.Exists(_savePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(_savePath, FileMode.Open);

                GameData data = formatter.Deserialize(stream) as GameData;
                stream.Close();
                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + _savePath);
                return null;
            }
        }
    }
}