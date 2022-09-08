using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Engine.Files;

namespace Engine
{
    public class SpritesFinder
    {
        public List<string> paths_sprites;
        public List<string> names_sprites;
        public List<int> numbers_sprites;

        private static class Constants
        {
            public static readonly string[] types = { "png", "jpg" };
            public static readonly string nameList = "save_list_sprites";
            public static readonly string nameDirectory = "Sprites";
            public static readonly string pathToResource = Application.dataPath + @"/Resources/";
            public static readonly string pathResourceList = $"{nameDirectory}/{nameList}";
            public static readonly string typeList = "txt";
            public static readonly string path = pathToResource + $"{nameDirectory}/";
            public static readonly string pathList = $"{path}/{nameList}.{typeList}";
            public static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        }

        public SpritesFinder()
        {
            paths_sprites = new List<string>();
            names_sprites = new List<string>();
            numbers_sprites = new List<int>();

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android)
            {
                string text_file_list = Resources.Load<TextAsset>(Constants.pathResourceList).text;

                string[] names_and_nums_sprites = text_file_list.Split('\n');

                foreach (string name_and_num_sprite in names_and_nums_sprites)
                {
                    if (string.IsNullOrEmpty(name_and_num_sprite))
                    {
                        continue;
                    }
                    string[] name_and_num_sprite_split = name_and_num_sprite.Split(' ');

                    string name = name_and_num_sprite_split[0];
                    int number = Convert.ToInt32(name_and_num_sprite_split[1]);

                    paths_sprites.Add($"{Constants.nameDirectory}/{name_and_num_sprite}");
                    names_sprites.Add(name);
                    numbers_sprites.Add(number);
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
                    this.names_sprites.Add(name);
                    this.numbers_sprites.Add(number);
                    this.paths_sprites.Add($"{Constants.nameDirectory}/{name} {number}");
                    text_file_list += $"{name} {number}\n";
                }

                File.WriteAllText(Constants.pathList, text_file_list, Constants.encoding);
            }
        }
    }
}
