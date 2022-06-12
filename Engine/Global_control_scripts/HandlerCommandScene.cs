using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace Engine
{
    public class HandlerCommandScene
    {
        public bool IsPrintingText;
        WorkingButtons save_work_choices;
        public bool flag_check_choice;

        public bool IsLookScene;

        public HandlerCommandScene()
        {
            IsPrintingText = false;
            flag_check_choice = false;
            IsLookScene = true;
        }

        public bool CanDoNextCommand()
        {
            bool flag = true;

            flag = flag && !this.IsPrintingText;
            flag = flag && !this.flag_check_choice;
            flag = flag && this.IsLookScene;

            return flag;
        }

        public void StopLookScene(Global_control global_Control)
        {
            this.IsLookScene = false;
            global_Control.ButtonScreen.enabled = false;

            global_Control.gameObject.GetComponent<TextPrintingClass>().PausePrinting();

            foreach (AudioHelper audioHelper in global_Control.gameObject.GetComponents<AudioHelper>())
            {
                audioHelper.Pause();
            }
        }

        public void StartLookScene(Global_control global_Control)
        {
            this.IsLookScene = true;
            global_Control.ButtonScreen.enabled = true;

            global_Control.gameObject.GetComponent<TextPrintingClass>().ResumePrinting();

            foreach (AudioHelper audioHelper in global_Control.gameObject.GetComponents<AudioHelper>())
            {
                audioHelper.UnPause();
            }
        }

        public void SetCommand(Global_control global_Control, Scene_class.DialogueOrChoiceOrCommand command)
        {
            switch (command.type)
            {
                case 0:
                    this.HandleDialogue(global_Control, command.dialogue);
                    break;

                case 1:
                    this.HandleChoice(global_Control, command.choice);
                    break;

                case 2:
                    this.HandleCommand(global_Control, command.command);
                    break;

                default:
                    break;
            }
        }

        private void HandleCommand(Global_control global_Control, Scene_class.Command command, bool IsStartedFromScene = true)
        {
            switch (command.name_command)
            {
                case "changeScene":
                    if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
                    {
                        Debug.LogException(new Exception("Don`t get name or number scene to change"));
                        return;
                    }

                    int number_command = 0;

                    if (command.dict_values != null && command.dict_values.ContainsKey("commandNumber"))
                    {
                        if (command.dict_values["commandNumber"].Length > 0)
                        {
                            number_command = Convert.ToInt32(command.dict_values["commandNumber"][0]);
                        }
                    }

                    global_Control.ChangeScene(command.number_obj, command.name_obj, number_command);

                    return;

                case "changeFlag":
                    if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
                    {
                        Debug.LogException(new Exception("Don`t get name or number flag to change"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj) && global_Control.Flags_name.Count <= command.number_obj)
                    {
                        Debug.LogException(new Exception("Overflow flag number"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj))
                    {
                        command.name_obj = global_Control.Flags_name[command.number_obj];
                    }

                    if (!global_Control.Flags.ContainsKey(command.name_obj))
                    {
                        global_Control.Flags.Add(command.name_obj, 0);
                        global_Control.Flags_name.Add(command.name_obj);
                    }

                    if (command.dict_values.ContainsKey("newValue"))
                    {
                        if (command.dict_values["newValue"].Length == 0 || command.dict_values["newValue"] == null)
                        {
                            Debug.LogException(new Exception("Don`t get new value to flag"));
                            return;
                        }
                        switch (command.signs["newValue"])
                        {
                            case '=':
                                global_Control.Flags[command.name_obj] = Convert.ToInt32(command.dict_values["newValue"][0]);
                                break;

                            case '+':
                                global_Control.Flags[command.name_obj] += Convert.ToInt32(command.dict_values["newValue"][0]);
                                break;
                        }
                    }
                    break;

                case "changeBackground":
                    if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
                    {
                        Debug.LogException(new Exception("Don`t get name or number background to change"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.backgroundsLoader.backgrounds_names.ContainsKey(command.number_obj))
                    {
                        Debug.LogException(new Exception("Haven`t name background of this number: " + command.number_obj.ToString() + "!"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj))
                    {
                        command.name_obj = global_Control.backgroundsLoader.backgrounds_names[command.number_obj];
                    }

                    if (!global_Control.backgroundsLoader.backgrounds.ContainsKey(command.name_obj))
                    {
                        Debug.LogException(new Exception("Haven`t background of this name: " + command.name_obj + "!"));
                        return;
                    }

                    global_Control.background.sprite = global_Control.backgroundsLoader.backgrounds[command.name_obj];
                    break;

                case "spawnSprite":
                    if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
                    {
                        Debug.LogException(new Exception("Don`t get name or number sprite to spawn"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.spritesLoader.sprites_names.ContainsKey(command.number_obj))
                    {
                        Debug.LogException(new Exception("Haven`t name background of this number: " + command.number_obj.ToString() + "!"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj))
                    {
                        command.name_obj = global_Control.spritesLoader.sprites_names[command.number_obj];
                    }

                    Sprite sprite = global_Control.spritesLoader.sprites[command.name_obj];

                    Vector3 position = new Vector3(0, 0, 0);
                    Vector3 rotation = new Vector3(0, 0, 0);
                    Vector3 size = new Vector3(1, 1, 1);

                    string name = null;

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
                            size = new Vector3(((float)values[0]), ((float)values[1]), ((float)values[2]));
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

                    GameObject sprite_obj = global_Control.SpawnObject(global_Control.prefab_sprites, position, size, rotation, name, global_Control.ToSpawnSprite.transform);
                    sprite_obj.GetComponent<Image>().sprite = sprite;
                    sprite_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

                    break;

                case "playAudio":
                    if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
                    {
                        Debug.LogException(new Exception("Don`t get name or number audio"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.audioLoader.audioNames.ContainsKey(command.number_obj))
                    {
                        Debug.LogException(new Exception("Haven`t name audio of this number: " + command.number_obj.ToString() + "!"));
                        return;
                    }
                    else if (string.IsNullOrEmpty(command.name_obj))
                    {
                        command.name_obj = global_Control.audioLoader.audioNames[command.number_obj];
                    }

                    DynamicArray<AudioClip> audio = new(global_Control.audioLoader.audioSources[command.name_obj]);

                    float volume = 1f;
                    float pitch = 1f;
                    float panStereo = 0f;
                    int countRepeat = 1;

                    if (command.dict_values.ContainsKey("volumeAudio"))
                    {
                        if (command.dict_values["volumeAudio"] != null && command.dict_values["volumeAudio"].Length > 0)
                        {
                            volume = (float)command.dict_values["volumeAudio"][0];
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

                    global_Control.audioHandler.PlayClip(countRepeat, panStereo, volume, pitch, audio.ToArray());

                    break;

                default:
                    Debug.LogException(new Exception("Don`t know this command: " + command.name_command));
                    break;

            }

            if (IsStartedFromScene)
            {
                global_Control.NewCommandScene();
            }
        }

        private void HandleDialogue(Global_control global_Control, Scene_class.DialogueText dialogueText)
        {
            bool flag_to_show = true;

            foreach (Scene_class.NeedFlag needFlag in dialogueText.needFlags)
            {
                flag_to_show = flag_to_show && CompareFlag(global_Control, needFlag);
            }

            if (!flag_to_show)
            {
                global_Control.NewCommandScene();
                return;
            }

            global_Control.gameObject.GetComponent<TextPrintingClass>().Init(global_Control, global_Control.text_dialogue, 
                dialogueText.text, global_Control.indexPrint);

            global_Control.indexPrint = 0;

            global_Control.text_character.text = dialogueText.character_name;

            float width_character_text = global_Control.text_dialogue.transform.GetComponent<RectTransform>().sizeDelta.x;
            global_Control.text_character.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width_character_text, global_Control.text_character.preferredHeight);
        }

        private void HandleChoice(Global_control global_Control, Scene_class.ChoiceText choiceText)
        {
            flag_check_choice = true;
            int flag_index = 0;
            int index_of_button = 0;

            while (choiceText.needFlags.Length != flag_index)
            {
                bool flag_to_show = true;
                foreach (Scene_class.NeedFlag needFlag in choiceText.needFlags[flag_index])
                {
                    flag_to_show = flag_to_show && CompareFlag(global_Control, needFlag);
                }
                if (flag_to_show)
                {
                    index_of_button++;
                }
                flag_index++;
            }

            WorkingButtons workingButtons = new WorkingButtons(index_of_button);
            flag_index = 0;
            index_of_button = 0;

            while (choiceText.needFlags.Length != flag_index)
            {
                bool flag_to_show = true;
                foreach (Scene_class.NeedFlag needFlag in choiceText.needFlags[flag_index])
                {
                    flag_to_show = flag_to_show && CompareFlag(global_Control, needFlag);
                }
                if (flag_to_show)
                {
                    workingButtons.names[index_of_button] = choiceText.choices[flag_index];
                    workingButtons.changeFlag[index_of_button] = choiceText.changeFlags[flag_index];
                    workingButtons.commands[index_of_button] = choiceText.commands[flag_index];
                    index_of_button++;
                }
                flag_index++;
            }

            workingButtons.count = index_of_button;
            RectTransform rt = global_Control.button_field.GetComponent(typeof(RectTransform)) as RectTransform;

            for (int i = 0; i < workingButtons.count; i++)
            {
                global_Control.SpawnObject(global_Control.button, new Vector3(0, -1 * rt.sizeDelta.y / (workingButtons.count + 1) * (i + 1) + rt.sizeDelta.y / 2, 90),
                    new Vector3(1, 1, 1), new Vector3(0, 0, 0), i.ToString(),
                    global_Control.button_field.transform).GetComponentInChildren<TextMeshProUGUI>().text = workingButtons.names[i];
            }

            save_work_choices = workingButtons;
        }
        public void On_Click_Choices(Global_control global_Control, string s)
        {
            int num_of_but = Convert.ToInt32(s);

            global_Control.DestroyAllObjects(global_Control.button_field.transform);

            foreach (Scene_class.ChangeFlag changeFlag in this.save_work_choices.changeFlag[num_of_but])
            {
                this.ChangeFlag(global_Control, changeFlag);
            }

            bool flag_do_new_command = true;

            foreach (Scene_class.Command command in this.save_work_choices.commands[num_of_but])
            {
                this.HandleCommand(global_Control, command, false);

                if (command.name_command == "changeScene")
                {
                    flag_do_new_command = false;
                }
            }

            this.save_work_choices = null;

            flag_check_choice = false;

            if (flag_do_new_command)
            {
                global_Control.NewCommandScene();
            }
        }

        private void ChangeFlag(Global_control global_Control, Scene_class.ChangeFlag changeFlag)
        {
            switch (changeFlag.change_sign)
            {
                case '=':
                    if (global_Control.Flags.ContainsKey(changeFlag.name))
                    {
                        global_Control.Flags[changeFlag.name] = changeFlag.value;
                    }
                    break;

                case '+':
                    if (global_Control.Flags.ContainsKey(changeFlag.name))
                    {
                        global_Control.Flags[changeFlag.name] += changeFlag.value;
                    }
                    break;

                case '-':
                    if (global_Control.Flags.ContainsKey(changeFlag.name))
                    {
                        global_Control.Flags[changeFlag.name] -= changeFlag.value;
                    }
                    break;

                default:
                    return;
            }
        }

        private bool CompareFlag(Global_control global_Control, Scene_class.NeedFlag flag)
        {
            if (!global_Control.Flags.ContainsKey(flag.name))
            {
                global_Control.Flags.Add(flag.name, 0);
                global_Control.Flags_name.Add(flag.name);
            }

            switch (flag.compare_sign)
            {
                case '=':
                    return global_Control.Flags[flag.name] == flag.value;

                case '>':
                    return global_Control.Flags[flag.name] > flag.value;

                case '<':
                    return global_Control.Flags[flag.name] < flag.value;

                default:
                    return false;
            }
        }
    }
}
