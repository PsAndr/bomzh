using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandlerCommandScene
{
    public HandlerCommandScene()
    {

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

    private void HandleCommand(Global_control global_Control, Scene_class.Command command)
    {
        switch (command.name_command)
        {
            case "changeScene":
                if (command.number_obj == -1 && command.name_obj == null)
                {
                    Debug.LogException(new Exception("Don`t get name or number scene to change"));
                }
                else
                {
                    global_Control.ChangeScene(command.number_obj, command.name_obj);
                }

                return;

            case "changeFlag":
                if (command.number_obj == -1 && command.name_obj == null)
                {
                    Debug.LogException(new Exception("Don`t get name or number flag to change"));
                    return;
                }
                else if (command.name_obj == null && global_Control.Flags_name.Count <= command.number_obj)
                {
                    Debug.LogException(new Exception("Overflow flag number"));
                    return;
                }
                else if (command.name_obj == null)
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

            default:
                Debug.LogException(new Exception("Don`t know this command: " + command.name_command));
                break;
        }
        global_Control.NewCommandScene();
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

        global_Control.text_dialogue.text = dialogueText.text;
        global_Control.text_character.text = dialogueText.character_name;
    }

    private void HandleChoice(Global_control global_Control, Scene_class.ChoiceText choiceText)
    {
        global_Control.NewCommandScene();
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
