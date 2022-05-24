using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class BackgroundsFinder
{
    public List<string> paths_backgrounds;
    public List<string> names_backgrounds;
    public List<int> numbers_backgrounds;
    
    public BackgroundsFinder()
    {
        paths_backgrounds = new List<string>();
        names_backgrounds = new List<string>();
        numbers_backgrounds = new List<int>();

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string text_file_list = Resources.Load<TextAsset>("Backgrounds/save_list_backgrounds").text;

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

                paths_backgrounds.Add(name_and_num_background);
                names_backgrounds.Add(name);
                numbers_backgrounds.Add(number);
            }
        }
        else if (Application.isEditor)
        {
            string path = Application.dataPath + @"/Resources/Backgrounds/";
            string[] paths_backgrounds_with_trash = Directory.GetFiles(path, "* *.jpg");

            string text_file_list = "";

            foreach (string str in paths_backgrounds_with_trash)
            {
                string name_and_num_background = str.Split('/')[^1][..^4];
                string[] name_and_num_background_split = name_and_num_background.Split(' ');

                string name = name_and_num_background_split[0];
                int number;
                try
                {
                    number = Convert.ToInt32(name_and_num_background_split[1]);
                }
                catch
                {
                    continue;
                }

                text_file_list += name_and_num_background + '\n';

                paths_backgrounds.Add(name_and_num_background);
                names_backgrounds.Add(name);
                numbers_backgrounds.Add(number);
            }
            File.WriteAllText(path + @"save_list_backgrounds.txt", text_file_list);
        }
    }
}
