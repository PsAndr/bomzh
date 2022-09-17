using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Engine.WorkWithRectTransform;

namespace Engine
{ 
    public static class MakersCommandScene
    {
        /// <summary>
        /// spawnSprite command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool SpawnSprite(Global_control global_Control, Scene_class.Command command)
        {
            if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
            {
                DebugEngine.LogException(new Exception("Don`t get name or number sprite to spawn"));
                return false;
            }
            else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.spritesLoader.sprites_names.ContainsKey(command.number_obj))
            {
                DebugEngine.LogException(new Exception("Haven`t name background of this number: " + command.number_obj.ToString() + "!"));
                return false;
            }
            else if (string.IsNullOrEmpty(command.name_obj))
            {
                command.name_obj = global_Control.spritesLoader.sprites_names[command.number_obj];
            }


            Sprite sprite = global_Control.spritesLoader.sprites[command.name_obj];

            Vector3 position = new Vector3(0, 0, 0);
            Vector3 rotation = new Vector3(0, 0, 0);
            Vector3 size = new Vector3(1, 1, 1);
            Vector2 sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

            string name = null;
            int hierarchyPosition = -1;

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
                    rotation = new Vector3(((float)values[0]), ((float)values[1]), ((float)values[2]));
                }
            }

            if (command.dict_values.ContainsKey("hierarchyPositionSprite"))
            {
                if (command.dict_values["hierarchyPositionSprite"] != null && command.dict_values["hierarchyPositionSprite"].Length > 0)
                {
                    hierarchyPosition = Convert.ToInt32(command.dict_values["hierarchyPositionSprite"][0]);
                }
            }
            DebugEngine.Log(command.name_obj);

            GameObject sprite_obj = Global_control.SpawnObject(global_Control.prefab_sprites, position, size, rotation, name + "___sprite", global_Control.ToSpawnSprite.transform);
            sprite_obj.GetComponent<Image>().sprite = sprite;
            sprite_obj.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            if (hierarchyPosition >= 0)
            {
                sprite_obj.transform.SetSiblingIndex(hierarchyPosition);
            }

            return true;
        }

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

                if (gameObject.GetComponent<Image>() != null && gameObject.name.Split("___")[0] == command.name_obj)
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

            int hierarchyPosition = sprite.transform.GetSiblingIndex();

            string name = sprite.name.Split("___")[0];

            if (command.dict_values_str.ContainsKey("nameSprite"))
            {
                name = command.dict_values_str["nameSprite"][0];
            }

            if (command.dict_values.ContainsKey("positionSprite"))
            {
                double[] values = command.dict_values["positionSprite"];

                if (values.Length >= 3)
                {
                    switch (command.signs["positionSprite"])
                    {
                        case '=':
                            position = new Vector3((float)values[0], (float)values[1], (float)values[2]);
                            break;

                        case '+':
                            position += new Vector3((float)values[0], (float)values[1], (float)values[2]);
                            break;
                    }

                }
            }

            if (command.dict_values.ContainsKey("sizeSprite"))
            {
                double[] values = command.dict_values["sizeSprite"];

                if (values.Length >= 3)
                {
                    switch (command.signs["sizeSprite"])
                    {
                        case '=':
                            size = new Vector3((float)values[0], (float)values[1], (float)values[2]);
                            break;

                        case '+':
                            size += new Vector3((float)values[0], (float)values[1], (float)values[2]);
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("sizeDeltaSprite"))
            {
                double[] values = command.dict_values["sizeDeltaSprite"];

                if (values.Length >= 2)
                {
                    switch (command.signs["sizeDeltaSprite"])
                    {
                        case '=':
                            sizeDelta = new Vector2((float)values[0], (float)values[1]);
                            break;

                        case '+':
                            sizeDelta += new Vector2((float)values[0], (float)values[1]);
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("rotationSprite"))
            {
                double[] values = command.dict_values["rotationSprite"];

                if (values.Length >= 3)
                {
                    switch (command.signs["rotationSprite"]) 
                    {
                        case '=':
                            rotation = new Vector3((float)values[0], (float)values[1], (float)values[2]);
                            break;

                        case '+':
                            rotation += new Vector3((float)values[0], (float)values[1], (float)values[2]);
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("hierarchyPositionSprite"))
            {
                if (command.dict_values["hierarchyPositionSprite"].Length > 0)
                {
                    switch (command.signs["hierarchyPositionSprite"])
                    {
                        case '=':
                            hierarchyPosition = Convert.ToInt32(command.dict_values["hierarchyPositionSprite"][0]);
                            break;

                        case '+':
                            hierarchyPosition += Convert.ToInt32(command.dict_values["hierarchyPositionSprite"][0]);
                            break;
                    }
                }
            }

            sprite.name = name + "___sprite";

            sprite.transform.localPosition = position;
            sprite.transform.localRotation = Quaternion.Euler(rotation);
            sprite.transform.localScale = size;
            sprite.GetComponent<RectTransform>().sizeDelta = sizeDelta;

            sprite.transform.SetSiblingIndex(hierarchyPosition);

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

                if (gameObject.GetComponent<Image>() != null && gameObject.name.Split("___")[0] == command.name_obj)
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

            global_Control.MyDestroyObject(sprite);

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

            AudioHelper.SaveClass audioHelperValues = global_Control.audioHandler.GetAudioHelper(command.name_obj).GetSave();

            DynamicArray<AudioClip> audio = new();

            string nameThisAudio = null;

            float volume = audioHelperValues.volume;
            float pitch = audioHelperValues.pitch;
            float panStereo = audioHelperValues.panStereo;
            int countRepeat = audioHelperValues.cnt;

            float startWait = audioHelperValues.startWait;
            float betweenWait = audioHelperValues.betweenWait;

            if (command.dict_values.ContainsKey("volumeAudio"))
            {
                if (command.dict_values["volumeAudio"] != null && command.dict_values["volumeAudio"].Length > 0)
                {
                    switch (command.signs["volumeAudio"])
                    {
                        case '=':
                            volume = (float)command.dict_values["volumeAudio"][0];
                            break;

                        case '+':
                            volume += (float)command.dict_values["volumeAudio"][0];
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("startWaitAudio"))
            {
                if (command.dict_values["startWaitAudio"] != null && command.dict_values["startWaitAudio"].Length > 0)
                {
                    switch (command.signs["startWaitAudio"])
                    {
                        case '=':
                            startWait = (float)command.dict_values["startWaitAudio"][0];
                            break;

                        case '+':
                            startWait += (float)command.dict_values["startWaitAudio"][0];
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("betweenWaitAudio"))
            {
                if (command.dict_values["betweenWaitAudio"] != null && command.dict_values["betweenWaitAudio"].Length > 0)
                {
                    switch (command.signs["betweenWaitAudio"])
                    {
                        case '=':
                            betweenWait = (float)command.dict_values["betweenWaitAudio"][0];
                            break;

                        case '+':
                            betweenWait = (float)command.dict_values["betweenWaitAudio"][0];
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("pitchAudio"))
            {
                if (command.dict_values["pitchAudio"] != null && command.dict_values["pitchAudio"].Length > 0)
                {
                    switch (command.signs["pitchAudio"])
                    {
                        case '=':
                            pitch = (float)command.dict_values["pitchAudio"][0];
                            break;
                        case '+':
                            pitch += (float)command.dict_values["pitchAudio"][0];
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("panStereoAudio"))
            {
                if (command.dict_values["panStereoAudio"] != null && command.dict_values["panStereoAudio"].Length > 0)
                {
                    switch (command.signs["panStereoAudio"])
                    {
                        case '=':
                            panStereo = (float)command.dict_values["panStereoAudio"][0];
                            break;
                        case '+':
                            panStereo += (float)command.dict_values["panStereoAudio"][0];
                            break;
                    }
                }
            }

            if (command.dict_values.ContainsKey("countRepeatAudio"))
            {
                if (command.dict_values["countRepeatAudio"] != null && command.dict_values["countRepeatAudio"].Length > 0)
                {
                    switch (command.signs["countRepeatAudio"])
                    {
                        case '=':
                            countRepeat = Convert.ToInt32(command.dict_values["countRepeatAudio"][0]);
                            break;
                        case '+':
                            countRepeat += Convert.ToInt32(command.dict_values["countRepeatAudio"][0]);
                            break;
                    }
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

        /// <summary>
        /// waitSeconds command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool WaitSeconds(Global_control global_Control, Scene_class.Command command)
        {
            if (command.number_obj <= 0)
            {
                Debug.LogException(new Exception("Wrong time wait in seconds"));
                return false;
            }

            float timeWait = command.number_obj;

            if (command.dict_values.ContainsKey("millisecondsWait"))
            {
                if (command.dict_values["millisecondsWait"] != null && command.dict_values["millisecondsWait"].Length > 0)
                {
                    timeWait += (float)command.dict_values["millisecondsWait"][0] / 1000f;
                }
            }

            global_Control.waitSceneCommand.StartWait(global_Control, timeWait);

            return true;
        }

        /// <summary>
        /// playVideo command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool PlayVideo(Global_control global_Control, Scene_class.Command command)
        {
            if (string.IsNullOrEmpty(command.name_obj) && command.number_obj == -1)
            {
                Debug.LogException(new Exception("Don`t get number or name of video"));
                return false;
            }
            else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.videoLoader.videoNames.ContainsKey(command.number_obj))
            {
                Debug.LogException(new Exception("Number of video is not valid!"));
                return false;
            }
            else if (string.IsNullOrEmpty(command.name_obj))
            {
                command.name_obj = global_Control.videoLoader.videoNames[command.number_obj];
            }

            Vector2 sizeDelta = new Vector2(global_Control.videoLoader.videoSources[command.name_obj].width, global_Control.videoLoader.videoSources[command.name_obj].height);
            Vector3 position = Vector3.zero;
            Vector3 rotation = Vector3.zero;
            Vector3 scale = Vector3.one;

            float volume = 1f;
            int cntRepeat = 1;
            float playbackSpeed = 1f;
            float panStereo = 0f;

            float startWait = 0f;
            float betweenWait = 0f;

            int hierarchyPosition = global_Control.toSpawnVideos.transform.childCount + 1;

            string name = string.Empty;

            List<VideoClip> videoClips = new();
            videoClips.Add(global_Control.videoLoader.videoSources[command.name_obj]);

            if (command.dict_values.ContainsKey("sizeDeltaVideo"))
            {
                if (command.dict_values["sizeDeltaVideo"] != null && command.dict_values["sizeDeltaVideo"].Length > 1)
                {
                    sizeDelta = WorkWithVectors.ConvertArrayToVector2(command.dict_values["sizeDeltaVideo"]);
                }
            }

            if (command.dict_values.ContainsKey("positionVideo"))
            {
                if (command.dict_values["positionVideo"] != null && command.dict_values["positionVideo"].Length > 2)
                {
                    position = WorkWithVectors.ConvertArrayToVector3(command.dict_values["positionVideo"]);
                }
            }

            if (command.dict_values.ContainsKey("sizeVideo"))
            {
                if (command.dict_values["sizeVideo"] != null && command.dict_values["sizeVideo"].Length > 2)
                {
                    scale = WorkWithVectors.ConvertArrayToVector3(command.dict_values["sizeVideo"]);
                }
            }

            if (command.dict_values.ContainsKey("rotationVideo"))
            {
                if (command.dict_values["rotationVideo"] != null && command.dict_values["rotationVideo"].Length > 2)
                {
                    rotation = WorkWithVectors.ConvertArrayToVector3(command.dict_values["rotationVideo"]);
                }
            }

            if (command.dict_values.ContainsKey("volumeVideo"))
            {
                if (command.dict_values["volumeVideo"] != null && command.dict_values["volumeVideo"].Length > 0)
                {
                    volume = (float)command.dict_values["volumeVideo"][0];
                }
            }

            if (command.dict_values.ContainsKey("countRepeatVideo"))
            {
                if (command.dict_values["countRepeatVideo"] != null && command.dict_values["countRepeatVideo"].Length > 0)
                {
                    cntRepeat = Convert.ToInt32(command.dict_values["countRepeatVideo"][0]);
                }
            }

            if (command.dict_values.ContainsKey("playbackSpeedVideo"))
            {
                if (command.dict_values["playbackSpeedVideo"] != null && command.dict_values["playbackSpeedVideo"].Length > 0)
                {
                    playbackSpeed = (float)command.dict_values["playbackSpeedVideo"][0];
                }
            }

            if (command.dict_values.ContainsKey("panStereoVideo"))
            {
                if (command.dict_values["panStereoVideo"] != null && command.dict_values["panStereoVideo"].Length > 0)
                {
                    panStereo = (float)command.dict_values["panStereoVideo"][0];
                }
            }

            if (command.dict_values.ContainsKey("startWaitVideo"))
            {
                if (command.dict_values["startWaitVideo"] != null && command.dict_values["startWaitVideo"].Length > 0)
                {
                    startWait = (float)command.dict_values["startWaitVideo"][0];
                }
            }

            if (command.dict_values.ContainsKey("betweenWaitVideo"))
            {
                if (command.dict_values["betweenWaitVideo"] != null && command.dict_values["betweenWaitVideo"].Length > 0)
                {
                    startWait = (float)command.dict_values["betweenWaitVideo"][0];
                }
            }

            if (command.dict_values_str.ContainsKey("nameVideo"))
            {
                if (command.dict_values_str["nameVideo"] != null && command.dict_values_str["nameVideo"].Length > 0)
                {
                    name = command.dict_values_str["nameVideo"][0];
                }
            }

            if (command.dict_values_str.ContainsKey("nameExtraVideo"))
            {
                if (command.dict_values_str["nameExtraVideo"] != null && command.dict_values_str["nameExtraVideo"].Length > 0)
                {
                    foreach (string nameVideo in command.dict_values_str["nameExtraVideo"])
                    {
                        if (global_Control.videoLoader.videoSources.ContainsKey(nameVideo))
                        {
                            videoClips.Add(global_Control.videoLoader.videoSources[nameVideo]);
                        }
                    }
                }
            }

            if (command.dict_values.ContainsKey("hierarchyPositionVideo"))
            {
                if (command.dict_values["hierarchyPositionVideo"] != null && command.dict_values["hierarchyPositionVideo"].Length > 0)
                {
                    hierarchyPosition = Convert.ToInt32(command.dict_values["hierarchyPositionVideo"][0]);
                }
            }

            global_Control.videoHandler.PlayVideo(name, cntRepeat, volume, playbackSpeed, panStereo, startWait, betweenWait, global_Control.toSpawnVideos, 
                new RectTransformSaveValuesSerializable(position, scale, rotation, sizeDelta, new Vector2(0.5f, 0.5f)), hierarchyPosition, videoClips.ToArray());

            return true;
        }

        /// <summary>
        /// deleteVideo command
        /// </summary>
        /// <param name="global_Control"></param>
        /// <param name="command"></param>
        /// <returns>is maked</returns>
        public static bool DeleteVideo(Global_control global_Control, Scene_class.Command command)
        {
            if (string.IsNullOrEmpty(command.name_obj))
            {
                Debug.LogException(new Exception("Don`t get name of video to delete!"));
                return false;
            }

            global_Control.videoHandler.DeleteVideo(command.name_obj);

            return true;
        }
    }
}

