using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundsLoader
{
    public Dictionary<string, Sprite> backgrounds;
    public Dictionary<int, string> backgrounds_names;

    public BackgroundsLoader()
    {
        BackgroundsFinder backgroundsFinder = new BackgroundsFinder();
        backgrounds_names = new Dictionary<int, string>();
        backgrounds = new Dictionary<string, Sprite>();

        for (int index = 0; index < backgroundsFinder.paths_backgrounds.Count; index++)
        {
            Sprite background_image = Resources.Load<Sprite>("Backgrounds/" + backgroundsFinder.paths_backgrounds[index]);

            backgrounds.Add(backgroundsFinder.names_backgrounds[index], background_image);
            backgrounds_names.Add(backgroundsFinder.numbers_backgrounds[index], backgroundsFinder.names_backgrounds[index]);     
        }
    }
}
