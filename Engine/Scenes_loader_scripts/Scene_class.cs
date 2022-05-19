using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Scene_class
{
    public int number;
    public string name;

    public DialogueOrChoiceOrCommand[] parts_scene;

    public Scene_class(int number, string name)
    {
        this.number = number;
        this.name = name;
    }

    public Scene_class(int number, string name, DialogueOrChoiceOrCommand[] parts_scene) : this(number, name)
    {
        this.number = number;
        this.name = name;
        this.parts_scene = parts_scene;
    }

    public string SaveToString()
    {
        return JsonUtility.ToJson(this);
    }

    [System.Serializable]
    public class DialogueOrChoiceOrCommand
    {
        public int type;

        public DialogueText dialogue;
        public ChoiceText choice;
        public Command command;

        public DialogueOrChoiceOrCommand(int type, object dialogueOrChoiceOrCommand)
        {
            this.type = type;

            switch (type)
            {
                case 0:
                    this.dialogue = (DialogueText)dialogueOrChoiceOrCommand;
                    break;

                case 1:
                    this.choice = (ChoiceText)dialogueOrChoiceOrCommand;
                    break;

                case 2:
                    this.command = (Command)dialogueOrChoiceOrCommand;
                    break;

                default:
                    return;
            }
        }
    }

    [System.Serializable]
    public class DialogueText // текст диалога и дополнительные параметры
    {
        public NeedFlag[] needFlags;

        public string character_name;

        public string text;

        public DialogueText(NeedFlag[] needFlags, string character_name, string text)
        {
            this.text = text;
            this.needFlags = needFlags;
            this.character_name = character_name;

            if (needFlags == null)
            {
                this.needFlags = new NeedFlag[0];
            }
            if (character_name == null)
            {
                this.character_name = "";
            }
            if (text == null)
            {
                this.text = "";
            }
        }
    }

    [System.Serializable]
    public class ChoiceText // выборы и необходимы значения + изменение значений при выборе и/или сцены
    {
        public NeedFlag[][] needFlags;

        public string[] choices;

        public ChangeFlag[][] changeFlags;

        public int[] scene_change_numbers;
        public string[] scene_change_names;

        public ChoiceText(NeedFlag[][] needFlags, string[] choices, ChangeFlag[][] changeFlags, int[] scene_change_numbers, string[] scene_change_names)
        {
            this.needFlags = needFlags;
            this.choices = choices;
            this.changeFlags = changeFlags;
            this.scene_change_numbers = scene_change_numbers;
            this.scene_change_names = scene_change_names;
        }
    }

    [System.Serializable]
    public class NeedFlag // необходимый флаг
    {
        public string name;
        public char compare_sign;
        public int value;

        public NeedFlag(string name, char compare_sign, int value)
        {
            this.value = value;
            this.name = name;
            this.compare_sign = compare_sign;
        }
    }

    [System.Serializable]
    public class ChangeFlag // изменение флага
    {
        public string name;
        public char change_sign;
        public int value;

        public ChangeFlag(string name, char change_sign, int value)
        {
            this.value = value;
            this.name = name;
            this.change_sign = change_sign;
        }
    }

    [System.Serializable]
    public class Command
    {
        public string name_command;

        public string name_obj;
        public int number_obj;

        public Dictionary<string, double[]> dict_values;

        public char[] signs;

        public Command(string name_command, string name_obj, int number_obj, Dictionary<string, double[]> dict_values, char[] signs)
        {
            this.name_command = name_command;
            this.name_obj = name_obj;
            this.number_obj = number_obj;
            this.dict_values = dict_values;
            this.signs = signs;
        }
    }
}
