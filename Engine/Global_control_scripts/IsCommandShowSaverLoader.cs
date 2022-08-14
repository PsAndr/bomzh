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

                MyDictionary<int, bool[]> toReturnWithoutUpdate = (MyDictionary<int, bool[]>)bf.Deserialize(fs);
                MyDictionary<int, bool[]> toReturn = new();

                fs.Close();

                foreach (var kvp in scenes.GetValues())
                {
                    toReturn[kvp.first] = new bool[kvp.second.parts_scene.Length];

                    for (int i = 0; i < kvp.second.parts_scene.Length; i++)
                    {
                        toReturn[kvp.first][i] = false;
                    }

                    if (toReturnWithoutUpdate.ContainsKey(kvp.first))
                    {
                        for (int i = 0; i < Mathf.Min(toReturn[kvp.first].Length, toReturnWithoutUpdate[kvp.first].Length); i++)
                        {
                            toReturn[kvp.first][i] = toReturnWithoutUpdate[kvp.first][i];
                        }
                    }
                }

                Save(toReturn);

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
