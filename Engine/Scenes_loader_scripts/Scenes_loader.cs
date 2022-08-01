using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Engine.WorkWithJSON;

namespace Engine
{
    public class Scenes_loader
    {
        public string[] list_scenes_paths;
        public string[] list_scenes_names;
        public MyDictionary<int, Scene_class> Scenes_dict;
        public MyDictionary<string, int> Scenes_names_dict;

        public Scenes_loader()
        {
            this.Scenes_dict = new Dictionary<int, Scene_class>();
            this.Scenes_names_dict = new Dictionary<string, int>();
            if (Application.isEditor)
            {
                Scenes_finder scenes_Finder = new Scenes_finder();
                this.list_scenes_paths = scenes_Finder.list_dirs;
                this.list_scenes_names = new string[this.list_scenes_paths.Length];

                int[] mass_nums_scenes;
                string[] mass_names_scenes;
                Scene_class[] mass_scenes;


                for (int i = 0; i < this.list_scenes_paths.Length; i++)
                {
                    Debug.Log(list_scenes_paths[i]);
                    string[] split = this.list_scenes_paths[i].Split('\\');
                    this.list_scenes_names[i] = split[^1]; // ^1 == the last index

                    int num_scene;
                    try
                    {
                        num_scene = Convert.ToInt32(this.list_scenes_names[i].Split(' ')[^1]);
                    }
                    catch
                    {
                        continue;
                    }

                    string name_scene;
                    string path_name_scene = list_scenes_paths[i] + @"\name.txt";

                    if (File.Exists(path_name_scene))
                    {
                        name_scene = File.ReadAllText(path_name_scene);
                    }
                    else
                    {
                        name_scene = num_scene.ToString();
                        File.WriteAllText(path_name_scene, name_scene);
                    }

                    ConvertDialogueFileToSceneClass convertDialogueFileToScene = new ConvertDialogueFileToSceneClass(list_scenes_paths[i] + @"\dialogue.txt");

                    Scene_class new_scene = new Scene_class(num_scene, name_scene, convertDialogueFileToScene.parts.ToArray());

                    if (!this.Scenes_dict.ContainsKey(num_scene))
                    {
                        this.Scenes_dict.Add(num_scene, new_scene);
                    }

                    if (!this.Scenes_names_dict.ContainsKey(name_scene))
                    {
                        this.Scenes_names_dict.Add(name_scene, num_scene);
                    }
                }

                mass_nums_scenes = new int[Scenes_dict.Count];
                mass_names_scenes = new string[Scenes_dict.Count];
                mass_scenes = new Scene_class[Scenes_dict.Count];

                int index = 0;

                foreach (Pair<int, Scene_class> kvp in this.Scenes_dict.GetValues())
                {
                    mass_nums_scenes[index] = kvp.first;
                    mass_scenes[index] = kvp.second;
                    mass_names_scenes[index] = kvp.second.name;

                    index++;
                }

                string scenes = Scenes_dict.ToJSON();
                string names_scenes = Scenes_names_dict.ToJSON();

                BinaryFormatter bf = null;
                FileStream fs = null;

                bf = new BinaryFormatter();
                fs = new FileStream(Application.dataPath + @"\Resources\Scenes_names.save", FileMode.Create);

                bf.Serialize(fs, mass_names_scenes);  //save names of scenes

                fs.Close();
                File.WriteAllText(Application.dataPath + @"\Resources\Scenes_names.json", names_scenes);

                bf = new BinaryFormatter();
                fs = new FileStream(Application.dataPath + @"\Resources\Scenes.save", FileMode.Create);

                bf.Serialize(fs, mass_scenes);   //save scenes (type == Scene_class)

                fs.Close();
                File.WriteAllText(Application.dataPath + @"\Resources\Scenes.json", scenes);
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                DebugEngine.Log("Start load scenes");
                string json_names_scenes = Resources.Load<TextAsset>("Scenes_names").text;
                this.Scenes_names_dict = MyDictionary<string, int>.FromJSON(json_names_scenes);

                string json_scenes = Resources.Load<TextAsset>("Scenes").text;
                this.Scenes_dict = MyDictionary<int, Scene_class>.FromJSON(json_scenes);

                DebugEngine.Log("End load scenes");
            }
        }
    }
}