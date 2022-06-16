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

        /// <summary>
        /// playAudio command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool PlayAudio(Global_control global_Control, Scene_class.Command command)
        {
            if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
            {
                Debug.LogException(new Exception("Don`t get name or number audio"));
                return false;
            }
            else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.audioLoader.audioNames.ContainsKey(command.number_obj))
            {
                Debug.LogException(new Exception("Haven`t name audio of this number: " + command.number_obj.ToString() + "!"));
                return false;
            }
            else if (string.IsNullOrEmpty(command.name_obj))
            {
                command.name_obj = global_Control.audioLoader.audioNames[command.number_obj];
            }

            DynamicArray<AudioClip> audio = new(global_Control.audioLoader.audioSources[command.name_obj]);

            string nameThisAudio = null;

            float volume = 1f;
            float pitch = 1f;
            float panStereo = 0f;
            int countRepeat = 1;

            float startWait = 0f;
            float betweenWait = 0f;

            if (command.dict_values.ContainsKey("volumeAudio"))
            {
                if (command.dict_values["volumeAudio"] != null && command.dict_values["volumeAudio"].Length > 0)
                {
                    volume = (float)command.dict_values["volumeAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("startWaitAudio"))
            {
                if (command.dict_values["startWaitAudio"] != null && command.dict_values["startWaitAudio"].Length > 0)
                {
                    startWait = (float)command.dict_values["startWaitAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("betweenWaitAudio"))
            {
                if (command.dict_values["betweenWaitAudio"] != null && command.dict_values["betweenWaitAudio"].Length > 0)
                {
                    betweenWait = (float)command.dict_values["betweenWaitAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("pitchAudio"))
            {
                if (command.dict_values["pitchAudio"] != null && command.dict_values["pitchAudio"].Length > 0)
                {
                    pitch = (float)command.dict_values["pitchAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("panStereoAudio"))
            {
                if (command.dict_values["panStereoAudio"] != null && command.dict_values["panStereoAudio"].Length > 0)
                {
                    panStereo = (float)command.dict_values["panStereoAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("countRepeatAudio"))
            {
                if (command.dict_values["countRepeatAudio"] != null && command.dict_values["countRepeatAudio"].Length > 0)
                {
                    countRepeat = Convert.ToInt32(command.dict_values["countRepeatAudio"][0]);
                }
            }

            if (command.dict_values_str.ContainsKey("nameExtraAudio"))
            {
                if (command.dict_values_str["nameExtraAudio"] != null && command.dict_values_str["nameExtraAudio"].Length > 0)
                {
                    foreach (string nameAudio in command.dict_values_str["nameExtraAudio"])
                    {
                        if (global_Control.audioLoader.audioSources.ContainsKey(nameAudio))
                        {
                            audio.Add(global_Control.audioLoader.audioSources[nameAudio]);
                        }
                    }
                }
            }

            if (command.dict_values_str.ContainsKey("nameAudio"))
            {
                if (command.dict_values_str["nameAudio"] != null && command.dict_values_str["nameAudio"].Length > 0)
                {
                    nameThisAudio = command.dict_values_str["nameAudio"][0];
                }
            }

            global_Control.audioHandler.PlayClip(nameThisAudio, countRepeat, panStereo, volume, pitch, startWait, betweenWait, audio.ToArray());

            return true;
        }

        /// <summary>
        /// changeAudio command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool ChangeAudio(Global_control global_Control, Scene_class.Command command)
        {
            if (string.IsNullOrEmpty(command.name_obj))
            {
                Debug.LogException(new Exception("Don`t get name audio"));
                return false;
            }

            DynamicArray<AudioClip> audio = new();

            string nameThisAudio = null;

            float volume = -1000f;
            float pitch = -1000f;
            float panStereo = -1000f;
            int countRepeat = 0;

            float startWait = -1000f;
            float betweenWait = -1000f;

            if (command.dict_values.ContainsKey("volumeAudio"))
            {
                if (command.dict_values["volumeAudio"] != null && command.dict_values["volumeAudio"].Length > 0)
                {
                    volume = (float)command.dict_values["volumeAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("startWaitAudio"))
            {
                if (command.dict_values["startWaitAudio"] != null && command.dict_values["startWaitAudio"].Length > 0)
                {
                    startWait = (float)command.dict_values["startWaitAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("betweenWaitAudio"))
            {
                if (command.dict_values["betweenWaitAudio"] != null && command.dict_values["betweenWaitAudio"].Length > 0)
                {
                    betweenWait = (float)command.dict_values["betweenWaitAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("pitchAudio"))
            {
                if (command.dict_values["pitchAudio"] != null && command.dict_values["pitchAudio"].Length > 0)
                {
                    pitch = (float)command.dict_values["pitchAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("panStereoAudio"))
            {
                if (command.dict_values["panStereoAudio"] != null && command.dict_values["panStereoAudio"].Length > 0)
                {
                    panStereo = (float)command.dict_values["panStereoAudio"][0];
                }
            }

            if (command.dict_values.ContainsKey("countRepeatAudio"))
            {
                if (command.dict_values["countRepeatAudio"] != null && command.dict_values["countRepeatAudio"].Length > 0)
                {
                    countRepeat = Convert.ToInt32(command.dict_values["countRepeatAudio"][0]);
                }
            }

            if (command.dict_values_str.ContainsKey("namePlayAudio"))
            {
                if (command.dict_values_str["namePlayAudio"] != null && command.dict_values_str["namePlayAudio"].Length > 0)
                {
                    foreach (string nameAudio in command.dict_values_str["namePlayAudio"])
                    {
                        if (global_Control.audioLoader.audioSources.ContainsKey(nameAudio))
                        {
                            audio.Add(global_Control.audioLoader.audioSources[nameAudio]);
                        }
                    }
                }
            }

            if (command.dict_values_str.ContainsKey("nameAudio"))
            {
                if (command.dict_values_str["nameAudio"] != null && command.dict_values_str["nameAudio"].Length > 0)
                {
                    nameThisAudio = command.dict_values_str["nameAudio"][0];
                }
            }

            global_Control.audioHandler.ChangeClip(global_Control, command.name_obj, nameThisAudio, countRepeat, panStereo, volume, pitch, startWait, betweenWait, audio.ToArray());

            return true;
        }

        /// <summary>
        /// deleteAudio command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool DeleteAudio(Global_control global_Control, Scene_class.Command command)
        {
            if (string.IsNullOrEmpty(command.name_obj))
            {
                Debug.LogException(new Exception("Don't get name audio to delete"));
                return false;
            }

            global_Control.audioHandler.DeleteClip(command.name_obj);

            return true;
        }
    }
}

