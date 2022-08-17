using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class Scene_class
    {
        public int number;
        public string name;

        public DialogueOrChoiceOrCommand[] parts_scene;
        public Scene_class()
        {

        }
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

            public System.Type GetTypePart()
            {
                switch (type)
                {
                    case 0:
                        return typeof(DialogueText);

                    case 1:
                        return typeof(ChoiceText);

                    case 2:
                        return typeof(Command);

                    default:
                        return null;
                }
            }

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

            public DialogueOrChoiceOrCommand(DialogueText dialogueText)
            {
                this.type = 0;
                this.dialogue = dialogueText;
            }

            public DialogueOrChoiceOrCommand(ChoiceText choiceText)
            {
                this.type = 1;
                this.choice = choiceText;
            }

            public DialogueOrChoiceOrCommand(Command command)
            {
                this.type = 2;
                this.command = command;
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
                if (string.IsNullOrEmpty(character_name))
                {
                    this.character_name = "";
                }
                if (string.IsNullOrEmpty(text))
                {
                    this.text = "";
                }

                while (this.text[^1] == ' ' || this.text[^1] == '\n')
                {
                    this.text = this.text[..^1];
                }
            }

            public void UpdateAfterLoadingJSON()
            {

            }
        }

        [System.Serializable]
        public class ChoiceText // выборы и необходимы значения + команды
        {
            public PartChoice[] partsChoice;

            public ChoiceText(NeedFlag[][] needFlags, string[] choices, ChangeFlag[][] changeFlags, Command[][] commands)
            {
                this.partsChoice = new PartChoice[choices.Length];

                for (int i = 0; i < choices.Length; i++)
                {
                    partsChoice[i] = new PartChoice(needFlags[i], choices[i], changeFlags[i], commands[i]);
                }
            }

            public ChoiceText(PartChoice[] partsChoice)
            {
                this.partsChoice = partsChoice;
            }
        }

        [System.Serializable]
        public class PartChoice
        {
            public NeedFlag[] needFlags;
            public string text_choice;
            public ChangeFlag[] changeFlags;
            public Command[] commands;

            public PartChoice(NeedFlag[] needFlags, string text_choice, ChangeFlag[] changeFlags, Command[] commands)
            {
                this.needFlags = needFlags;
                this.text_choice = text_choice;
                this.changeFlags = changeFlags;
                this.commands = commands;
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

            public override string ToString()
            {
                return name + compare_sign + value.ToString();
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

            public MyDictionary<string, double[]> dict_values;
            public MyDictionary<string, string[]> dict_values_str;

            public MyDictionary<string, char> signs;

            public NeedFlag[] needFlags;

            public Command(string name_command, string name_obj, int number_obj, MyDictionary<string, double[]> dict_values, MyDictionary<string, string[]> dict_values_str,
                Dictionary<string, char> signs, NeedFlag[] needFlags)
            {
                this.name_command = name_command;
                this.name_obj = name_obj;
                this.number_obj = number_obj;
                this.dict_values = dict_values;
                this.dict_values_str = dict_values_str;
                this.signs = signs;

                this.needFlags = needFlags;
            }
        }
    }
}
