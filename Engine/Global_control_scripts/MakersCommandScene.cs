using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Engine
{ 
    public static class MakersCommandScene
    {
        /// <summary>
        /// changeSprite command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool ChangeSprite(Global_control global_Control, Scene_class.Command command)
        {
            if (string.IsNullOrEmpty(command.name_obj))
            {
                Debug.LogException(new Exception("Don't get name sprite to change"));
                return false;
            }

            GameObject sprite = null;

            for (int i = 0; i < global_Control.ToSpawnSprite.transform.childCount; i++)
            {
                GameObject gameObject = global_Control.ToSpawnSprite.transform.GetChild(i).gameObject;

                if (gameObject.GetComponent<Image>() != null && gameObject.name == command.name_obj)
                {
                    sprite = gameObject;
                    break;
                }
            }

            if (sprite == null)
            {
                Debug.LogException(new Exception($"Don`t find sprite of this name: {command.name_obj}"));
                return false;
            }

            Vector3 position = sprite.transform.localPosition;
            Vector3 rotation = sprite.transform.localRotation.eulerAngles;
            Vector3 size = sprite.transform.localScale;
            Vector2 sizeDelta = sprite.GetComponent<RectTransform>().sizeDelta;

            string name = sprite.name;

            if (command.dict_values_str.ContainsKey("nameSprite"))
            {
                name = command.dict_values_str["nameSprite"][0];
            }

            if (command.dict_values.ContainsKey("positionSprite"))
            {
                double[] values = command.dict_values["positionSprite"];

                if (values.Length >= 3)
                {
                    position = new Vector3(((float)values[0]), ((float)values[1]), ((float)values[2]));
                }
            }

            if (command.dict_values.ContainsKey("sizeSprite"))
            {
                double[] values = command.dict_values["sizeSprite"];

                if (values.Length >= 3)
                {
                    size = new Vector3((float)values[0], (float)values[1], (float)values[2]);
                }
            }

            if (command.dict_values.ContainsKey("sizeDeltaSprite"))
            {
                double[] values = command.dict_values["sizeDeltaSprite"];

                if (values.Length >= 2)
                {
                    sizeDelta = new Vector2((float)values[0], (float)values[1]);
                }
            }

            if (command.dict_values.ContainsKey("rotationSprite"))
            {
                double[] values = command.dict_values["rotationSprite"];

                if (values.Length >= 3)
                {
                    rotation = new Vector3((float)values[0], (float)values[1], (float)values[2]);
                }
            }

            sprite.name = name;

            sprite.transform.localPosition = position;
            sprite.transform.localRotation = Quaternion.Euler(rotation);
            sprite.transform.localScale = size;
            sprite.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            return true;
        }

        /// <summary>
        /// deleteSprite command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool DeleteSprite(Global_control global_Control, Scene_class.Command command)
        {
            if (string.IsNullOrEmpty(command.name_obj))
            {
                Debug.LogException(new Exception("Don't get name sprite to delete"));
                return false;
            }

            GameObject sprite = null;

            for (int i = 0; i < global_Control.ToSpawnSprite.transform.childCount; i++)
            {
                GameObject gameObject = global_Control.ToSpawnSprite.transform.GetChild(i).gameObject;

                if (gameObject.GetComponent<Image>() != null && gameObject.name == command.name_obj)
                {
                    sprite = gameObject;
                    break;
                }
            }

            if (sprite == null)
            {
                Debug.LogException(new Exception($"Don`t find sprite of this name: {command.name_obj}"));
                return false;
            }

            global_Control.DestroyObject(sprite);

            return true;
        }
    }
}

