using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SpritesFinder
{
    public List<string> paths_sprites;
    public List<string> names_sprites;
    public List<int> numbers_sprites;

    public SpritesFinder()
    {
        paths_sprites = new List<string>();
        names_sprites = new List<string>();
        numbers_sprites = new List<int>();

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string text_file_list = Resources.Load<TextAsset>("Sprites/save_list_sprites").text;

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

                paths_sprites.Add(name_and_num_sprite);
                names_sprites.Add(name);
                numbers_sprites.Add(number);
            }
        }
        else if (Application.isEditor)
        {
            string path = Application.dataPath + @"/Resources/Sprites/";
            string[] paths_sprites_with_trash_jpg = Directory.GetFiles(path, "* *.jpg");
            string[] paths_sprites_with_trash_png = Directory.GetFiles(path, "* *.png");

            string text_file_list = "";

            foreach (string str in paths_sprites_with_trash_jpg)
            {
                string name_and_num_sprite = str.Split('/')[^1][..^4];
                string[] name_and_num_sprite_split = name_and_num_sprite.Split(' ');

                string name = name_and_num_sprite_split[0];
                int number;
                try
                {
                    number = Convert.ToInt32(name_and_num_sprite_split[1]);
                }
                catch
                {
                    continue;
                }

                text_file_list += name_and_num_sprite + '\n';

                paths_sprites.Add(name_and_num_sprite);
                names_sprites.Add(name);
                numbers_sprites.Add(number);
            }

            foreach (string str in paths_sprites_with_trash_png)
            {
                string name_and_num_sprite = str.Split('/')[^1][..^4];
                string[] name_and_num_sprite_split = name_and_num_sprite.Split(' ');

                string name = name_and_num_sprite_split[0];
                int number;
                try
                {
                    number = Convert.ToInt32(name_and_num_sprite_split[1]);
                }
                catch
                {
                    continue;
                }

                text_file_list += name_and_num_sprite + '\n';

                paths_sprites.Add(name_and_num_sprite);
                names_sprites.Add(name);
                numbers_sprites.Add(number);
            }

            File.WriteAllText(path + @"save_list_sprites.txt", text_file_list);
        }
    }
}
