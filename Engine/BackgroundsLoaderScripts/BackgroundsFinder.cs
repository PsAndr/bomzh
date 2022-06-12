using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Engine.Files;

namespace Engine
{
    public class BackgroundsFinder
    {
        public List<string> paths_backgrounds;
        public List<string> names_backgrounds;
        public List<int> numbers_backgrounds;

        private static class Constants
        {
            public static readonly string[] types = { "png", "jpg" };
            public static readonly string nameList = "save_list_backgrounds";
            public static readonly string nameDirectory = "Backgrounds";
            public static readonly string pathToResource = Application.dataPath + @"/Resources/";
            public static readonly string pathResourceList = $"{nameDirectory}/{nameList}";
            public static readonly string typeList = "txt";
            public static readonly string path = pathToResource + $"{nameDirectory}/";
            public static readonly string pathList = $"{path}/{nameList}.{typeList}";
            public static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        }

        public BackgroundsFinder()
        {
            paths_backgrounds = new List<string>();
            names_backgrounds = new List<string>();
            numbers_backgrounds = new List<int>();

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                string text_file_list = Resources.Load<TextAsset>(Constants.pathResourceList).text;

                string[] names_and_nums_backgrounds = text_file_list.Split('\n');

                foreach (string name_and_num_background in names_and_nums_backgrounds)
                {
                    if (string.IsNullOrEmpty(name_and_num_background))
                    {
                        continue;
                    }
                    string[] name_and_num_background_split = name_and_num_background.Split(' ');

                    string name = name_and_num_background_split[0];
                    int number = Convert.ToInt32(name_and_num_background_split[1]);

                    paths_backgrounds.Add($"{Constants.nameDirectory}/{name_and_num_background}");
                    names_backgrounds.Add(name);
                    numbers_backgrounds.Add(number);
                }
            }
            else if (Application.isEditor)
            {
                string path = Constants.path;

                WorkWithFiles.CheckFiles(path, Constants.types);

                Pair<string, int>[] numsAndNames = WorkWithFiles.GetFilesNumsAndNames(path, Constants.types);

                string text_file_list = "";

                foreach ((string name, int number) in numsAndNames)
                {
                    this.names_backgrounds.Add(name);
                    this.numbers_backgrounds.Add(number);
                    this.paths_backgrounds.Add($"{Constants.nameDirectory}/{name} {number}");
                    text_file_list += $"{name} {number}\n";
                }

                File.WriteAllText(Constants.pathList, text_file_list, Constants.encoding);
            }
        }
    }
}
