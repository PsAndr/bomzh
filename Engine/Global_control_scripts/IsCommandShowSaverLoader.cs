using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Engine
{
    public class IsCommandShowSaverLoader
    {
        private static class Constants
        {
            public static readonly string path = Application.persistentDataPath + "/isCommandsShow.save";
        }

        public static MyDictionary<int, bool[]> Load(MyDictionary<int, Scene_class> scenes)
        {
            if (!File.Exists(Constants.path))
            {
                MyDictionary<int, bool[]> toFirstSave = new();

                foreach (var kvp in scenes.GetValues())
                {
                    toFirstSave[kvp.first] = new bool[kvp.second.parts_scene.Length];

                    for (int i = 0; i < kvp.second.parts_scene.Length; i++)
                    {
                        toFirstSave[kvp.first][i] = false;
                    }
                }

                Save(toFirstSave);

                return toFirstSave;
            }
            else
            {
                FileStream fs = new FileStream(Constants.path, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();

                MyDictionary<int, bool[]> toReturn = (MyDictionary<int, bool[]>)bf.Deserialize(fs);

                return toReturn;
            }
        }

        public static void Save(MyDictionary<int, bool[]> toSave)
        {
            FileStream fs = new FileStream(Constants.path, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(fs, toSave);

            fs.Close();
        }

        public static void DeleteSave()
        {
            if (File.Exists(Constants.path))
            {
                File.Delete(Constants.path);
            }
        }
    }
}
