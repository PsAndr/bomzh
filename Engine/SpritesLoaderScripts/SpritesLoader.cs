using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritesLoader
{
    public Dictionary<string, Sprite> sprites;
    public Dictionary<int, string> sprites_names;

    public SpritesLoader()
    {
        SpritesFinder spritesFinder = new SpritesFinder();
        sprites_names = new Dictionary<int, string>();
        sprites = new Dictionary<string, Sprite>();

        for (int index = 0; index < spritesFinder.paths_sprites.Count; index++)
        {
            Sprite sprite_image = Resources.Load<Sprite>("Sprites/" + spritesFinder.paths_sprites[index]);

            sprites.Add(spritesFinder.names_sprites[index], sprite_image);
            sprites_names.Add(spritesFinder.numbers_sprites[index], spritesFinder.names_sprites[index]);
        }
    }
}
