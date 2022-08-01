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

        public bool isWaiting; 

        private static class Constants
        {
            public static readonly float secondsToWaitInSkiping = 0.2f;
        }

        public HandlerCommandScene()
        {
            IsPrintingText = false;
            flag_check_choice = false;
            IsLookScene = true;

            isWaiting = false;
        }

        public bool CanDoNextCommand()
        {
            bool flag = true;

            flag = flag && !this.IsPrintingText;
            flag = flag && !this.flag_check_choice;
            flag = flag && this.IsLookScene;
            flag = flag && !this.isWaiting;

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

            foreach (VideoHelper videoHelper in global_Control.gameObject.GetComponents<VideoHelper>())
            {
                videoHelper.Pause();
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

            foreach (VideoHelper videoHelper in global_Control.gameObject.GetComponents<VideoHelper>())
            {
                videoHelper.UnPause();
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

        public void SetCommand(Global_control global_Control, Scene_class.Command command, bool IsStartedFromScene = true)
        {
            this.HandleCommand(global_Control, command, IsStartedFromScene);
        }

        public void SetCommand(Global_control global_Control, Scene_class.DialogueText command)
        {
            this.HandleDialogue(global_Control, command);
        }

        public void SetCommand(Global_control global_Control, Scene_class.ChoiceText command)
        {
            this.HandleChoice(global_Control, command);
        }

        private void HandleCommand(Global_control global_Control, Scene_class.Command command, bool IsStartedFromScene = true)
        {
            if (CompareFlags(global_Control, command.needFlags))
            {
                DebugEngine.Log(command.name_command);
                switch (command.name_command)
                {
                    case "changeScene":
                        if (command.number_obj == -1 && string.IsNullOrEmpty(command.name_obj))
                        {
                            DebugEngine.LogException(new Exception("Don`t get name or number scene to change"));
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
                        if (string.IsNullOrEmpty(command.name_obj))
                        {
                            DebugEngine.LogException(new Exception("Don`t get name of flag to change"));
                            return;
                        }

                        if (!global_Control.Flags.ContainsKey(command.name_obj))
                        {
                            global_Control.Flags.Add(command.name_obj, 0);
                        }

                        if (command.dict_values.ContainsKey("newValue"))
                        {
                            if (command.dict_values["newValue"].Length == 0 || command.dict_values["newValue"] == null)
                            {
                                DebugEngine.LogException(new Exception("Don`t get new value to flag"));
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
                            DebugEngine.LogException(new Exception("Don`t get name or number background to change"));
                            return;
                        }
                        else if (string.IsNullOrEmpty(command.name_obj) && !global_Control.backgroundsLoader.backgrounds_names.ContainsKey(command.number_obj))
                        {
                            DebugEngine.LogException(new Exception("Haven`t name background of this number: " + command.number_obj.ToString() + "!"));
                            return;
                        }
                        else if (string.IsNullOrEmpty(command.name_obj))
                        {
                            command.name_obj = global_Control.backgroundsLoader.backgrounds_names[command.number_obj];
                        }

                        if (!global_Control.backgroundsLoader.backgrounds.ContainsKey(command.name_obj))
                        {
                            DebugEngine.LogException(new Exception("Haven`t background of this name: " + command.name_obj + "!"));
                            return;
                        }

                        global_Control.background.sprite = global_Control.backgroundsLoader.backgrounds[command.name_obj];
                        break;

                    case "spawnSprite":
                        DebugEngine.Log("Start spawn sprite");
                        if (!MakersCommandScene.SpawnSprite(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        DebugEngine.Log("End spawn sprite");
                        break;

                    case "changeSprite":
                        if (!MakersCommandScene.ChangeSprite(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "deleteSprite":
                        if (!MakersCommandScene.DeleteSprite(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "playAudio":
                        if (!MakersCommandScene.PlayAudio(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "changeAudio":
                        if (!MakersCommandScene.ChangeAudio(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "deleteAudio":
                        if (!MakersCommandScene.DeleteAudio(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "waitSeconds":
                        if (!MakersCommandScene.WaitSeconds(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "playVideo":
                        if (!MakersCommandScene.PlayVideo(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    case "deleteVideo":
                        if (!MakersCommandScene.DeleteVideo(global_Control, command))
                        {
                            DebugEngine.LogWarning(command.name_command);
                            return;
                        }
                        break;

                    default:
                        DebugEngine.LogException(new Exception("Don`t know this command: " + command.name_command));
                        break;

                }
            }

            Type[] types = TypesCommandsSkiped.GetTypes(global_Control.settings.TypeSkiping);

            if (global_Control.isSkiping && FindInArray.Find(typeof(Scene_class.Command), ref types) != -1)
            {
                bool isCommandShow = global_Control.IsCommandShow[global_Control.GetSceneValues().first][global_Control.GetSceneValues().second];

                if (!IsStartedFromScene || !TypesCommandsSkiped.GetIsShowedSkip(global_Control.settings.TypeSkiping) || isCommandShow)
                {
                    global_Control.waitSceneCommand.StopWait(global_Control);
                }
            }

            if (IsStartedFromScene)
            {
                global_Control.NewCommandScene();
            }
        }

        private void HandleDialogue(Global_control global_Control, Scene_class.DialogueText dialogueText)
        {
            DebugEngine.Log("Dialogue start");
            Type[] types = TypesCommandsSkiped.GetTypes(global_Control.settings.TypeSkiping);

            bool flag_to_show = CompareFlags(global_Control, dialogueText.needFlags);

            if (!flag_to_show)
            {
                global_Control.NewCommandScene();
                return;
            }

            global_Control.gameObject.GetComponent<TextPrintingClass>().Init(global_Control, global_Control.text_dialogue,
            dialogueText.text, global_Control.settings.SpeedTextPrinting, global_Control.indexPrint);

            global_Control.indexPrint = 0;

            global_Control.text_character.text = dialogueText.character_name;

            float width_character_text = global_Control.text_dialogue.transform.GetComponent<RectTransform>().sizeDelta.x;
            global_Control.text_character.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width_character_text, global_Control.text_character.preferredHeight);

            if (global_Control.isSkiping && FindInArray.Find(typeof(Scene_class.DialogueText), ref types) != -1)
            {
                bool isCommandShow = global_Control.IsCommandShow[global_Control.GetSceneValues().first][global_Control.GetSceneValues().second];

                if (!TypesCommandsSkiped.GetIsShowedSkip(global_Control.settings.TypeSkiping) || isCommandShow)
                {
                    global_Control.gameObject.GetComponent<TextPrintingClass>().FinishPrinting();
                    global_Control.waitSceneCommand.StartWait(global_Control, Constants.secondsToWaitInSkiping);
                }
            }
        }

        private void HandleChoice(Global_control global_Control, Scene_class.ChoiceText choiceText)
        {
            flag_check_choice = true;
            int flag_index = 0;
            int index_of_button = 0;

            while (choiceText.partsChoice.Length != flag_index)
            {
                bool flag_to_show = CompareFlags(global_Control, choiceText.partsChoice[flag_index].needFlags);
                if (flag_to_show)
                {
                    index_of_button++;
                }
                flag_index++;
            }

            WorkingButtons workingButtons = new WorkingButtons(index_of_button);
            flag_index = 0;
            index_of_button = 0;

            while (choiceText.partsChoice.Length != flag_index)
            {
                bool flag_to_show = CompareFlags(global_Control, choiceText.partsChoice[flag_index].needFlags);

                if (flag_to_show)
                {
                    workingButtons.names[index_of_button] = choiceText.partsChoice[flag_index].text_choice;
                    workingButtons.changeFlag[index_of_button] = choiceText.partsChoice[flag_index].changeFlags;
                    workingButtons.commands[index_of_button] = choiceText.partsChoice[flag_index].commands;
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

            Type[] types = TypesCommandsSkiped.GetTypes(global_Control.settings.TypeSkiping);

            if (global_Control.isSkiping && FindInArray.Find(typeof(Scene_class.ChoiceText), ref types) != -1)
            {
                bool isCommandShow = global_Control.IsCommandShow[global_Control.GetSceneValues().first][global_Control.GetSceneValues().second];

                if (!TypesCommandsSkiped.GetIsShowedSkip(global_Control.settings.TypeSkiping) || isCommandShow)
                {
                    int randomChoice = UnityEngine.Random.Range(0, choiceText.partsChoice.Length - 1);

                    global_Control.waitSceneCommand.StartWait(global_Control, Constants.secondsToWaitInSkiping);

                    On_Click_Choices(global_Control, randomChoice.ToString());
                }
            }
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

        private bool CompareFlags(Global_control global_Control, params Scene_class.NeedFlag[] flags)
        {
            bool isCompare = true;

            foreach (Scene_class.NeedFlag flag in flags)
            {
                if (!global_Control.Flags.ContainsKey(flag.name))
                {
                    global_Control.Flags.Add(flag.name, 0);
                }

                switch (flag.compare_sign)
                {
                    case '=':
                        isCompare = isCompare && global_Control.Flags[flag.name] == flag.value;
                        break;

                    case '>':
                        isCompare = isCompare && global_Control.Flags[flag.name] > flag.value;
                        break;

                    case '<':
                        isCompare = isCompare && global_Control.Flags[flag.name] < flag.value;
                        break;

                    default:
                        isCompare = isCompare && false;
                        break;
                }
            }

            return isCompare;
        }
    }
}
