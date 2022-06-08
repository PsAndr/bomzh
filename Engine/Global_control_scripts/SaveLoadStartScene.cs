using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Engine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Engine
{
    [System.Serializable]
    public class SceneEngine
    {
        public int sceneNumber;
        public string sceneName;
        public int numberCommandScene;

        public void SetDefault()
        {
            numberCommandScene = -1;
            sceneName = null;
            numberCommandScene = 0;
        }

        public void SetValues(int sceneNumber, string sceneName, int numberCommandScene)
        {
            this.sceneNumber = sceneNumber;
            this.sceneName = sceneName;
            this.numberCommandScene = numberCommandScene;
        }

        public void SetValues(SceneEngine sceneNow)
        {
            this.sceneNumber = sceneNow.sceneNumber + 0;
            this.sceneName = sceneNow.sceneName + "";
            this.numberCommandScene = sceneNow.numberCommandScene + 0;
        }
    }

    public class SaveLoadStartScene
    {
        private readonly static string path = Application.dataPath + "/Resources/startScene.save";

        public static void Save(int number, string name, int numberCommand)
        {
            SceneEngine scene = new SceneEngine();
            scene.SetValues(number, name, numberCommand);

            string path = SaveLoadStartScene.path;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, scene);
            fs.Close();
        }

        public static void Save(SceneEngine sceneNow)
        {
            SceneEngine scene = new SceneEngine();
            scene.SetValues(sceneNow);

            string path = SaveLoadStartScene.path;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Create);
            bf.Serialize(fs, scene);
            fs.Close();
        }

        public static SceneEngine Load()
        {
            string path = SaveLoadStartScene.path;

            SceneEngine scene = new SceneEngine();

            if (!File.Exists(path))
            {
                scene.SetDefault();

                SaveLoadStartScene.Save(scene);

                return scene;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            scene = (SceneEngine)bf.Deserialize(fs);

            fs.Close();

            return scene;
        }
    }
}
