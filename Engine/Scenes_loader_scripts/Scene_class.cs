using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using WorkWithDictionary;

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

    public void UpdateAfterLoadingJSON()
    {
        foreach (DialogueOrChoiceOrCommand dialogueOrChoiceOrCommand in parts_scene)
        {
            switch (dialogueOrChoiceOrCommand.type)
            {
                case 0:
                    dialogueOrChoiceOrCommand.dialogue.UpdateAfterLoadingJSON();
                    break;

                case 1:
                    dialogueOrChoiceOrCommand.choice.UpdateAfterLoadingJSON();
                    break;

                case 2:
                    dialogueOrChoiceOrCommand.command.UpdateAfterLoadingJSON();
                    break;
            }
        }
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
        public NeedFlag[][] needFlags;

        public string[] choices;

        public ChangeFlag[][] changeFlags;

        public Command[][] commands;

        public PartChoice[] partsChoice;

        public ChoiceText(NeedFlag[][] needFlags, string[] choices, ChangeFlag[][] changeFlags, Command[][] commands)
        {
            this.needFlags = needFlags;
            this.choices = choices;
            this.changeFlags = changeFlags;
            this.commands = commands;

            this.partsChoice = new PartChoice[choices.Length];

            for (int i = 0; i < choices.Length; i++)
            {
                partsChoice[i] = new PartChoice(needFlags[i], choices[i], changeFlags[i], commands[i]);
            }
        }

        public void UpdateAfterLoadingJSON()
        {
            this.needFlags = new NeedFlag[this.partsChoice.Length][];
            this.choices = new string[this.partsChoice.Length];
            this.changeFlags = new ChangeFlag[this.partsChoice.Length][];
            this.commands = new Command[this.partsChoice.Length][];

            for (int i = 0; i < this.partsChoice.Length; i++)
            {
                this.needFlags[i] = this.partsChoice[i].needFlags;
                this.choices[i] = this.partsChoice[i].text_choice;
                this.changeFlags[i] = this.partsChoice[i].changeFlags;
                this.commands[i] = this.partsChoice[i].commands;
            }
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
        public Dictionary<string, string[]> dict_values_str;

        public Dictionary<string, char> signs;

        public DictionaryToTwoArrays<string, WorkWithJSON_mass<double>> values_arrays;
        public DictionaryToTwoArrays<string, WorkWithJSON_mass<string>> values_str_arrays;
        public DictionaryToTwoArrays<string, char> signs_arrays;

        public Command(string name_command, string name_obj, int number_obj, Dictionary<string, double[]> dict_values, Dictionary<string, string[]> dict_values_str, Dictionary<string, char> signs)
        {
            this.name_command = name_command;
            this.name_obj = name_obj;
            this.number_obj = number_obj;
            this.dict_values = dict_values;
            this.dict_values_str = dict_values_str;
            this.signs = signs;

            Dictionary<string, WorkWithJSON_mass<double>> dict_valuesJSON = new Dictionary<string, WorkWithJSON_mass<double>>();
            Dictionary<string, WorkWithJSON_mass<string>> dict_values_strJSON = new Dictionary<string, WorkWithJSON_mass<string>>();

            foreach (KeyValuePair<string, double[]> kvp in dict_values)
            {
                dict_valuesJSON.Add(kvp.Key, new WorkWithJSON_mass<double>(kvp.Value));
            }
            foreach (KeyValuePair<string, string[]> kvp in dict_values_str)
            {
                dict_values_strJSON.Add(kvp.Key, new WorkWithJSON_mass<string>(kvp.Value));
            }

            this.values_arrays = new DictionaryToTwoArrays<string, WorkWithJSON_mass<double>>(dict_valuesJSON);
            this.values_str_arrays = new DictionaryToTwoArrays<string, WorkWithJSON_mass<string>>(dict_values_strJSON);
            this.signs_arrays = new DictionaryToTwoArrays<string, char>(signs);
        }

        public Command(string name_command, string name_obj, int number_obj, DictionaryToTwoArrays<string, WorkWithJSON_mass<double>> values_arrays, DictionaryToTwoArrays<string, WorkWithJSON_mass<string>> values_str_arrays, DictionaryToTwoArrays<string, char> signs_arrays)
        {
            this.name_command = name_command;
            this.name_obj = name_obj;
            this.number_obj = number_obj;
            
            Dictionary<string, WorkWithJSON_mass<double>> dict_values = values_arrays.ConvertToDictionary();
            Dictionary<string, WorkWithJSON_mass<string>> dict_values_str = values_str_arrays.ConvertToDictionary();

            this.dict_values = new Dictionary<string, double[]>();
            this.dict_values_str = new Dictionary<string, string[]>();

            foreach (KeyValuePair<string, WorkWithJSON_mass<double>> kvp in dict_values)
            {
                this.dict_values.Add(kvp.Key, kvp.Value.item);
            }
            foreach (KeyValuePair<string, WorkWithJSON_mass<string>> kvp in dict_values_str)
            {
                this.dict_values_str.Add(kvp.Key, kvp.Value.item);
            }

            this.signs = signs_arrays.ConvertToDictionary();

            this.values_arrays = values_arrays;
            this.values_str_arrays = values_str_arrays;
            this.signs_arrays = signs_arrays;
        }

        public void UpdateAfterLoadingJSON()
        {
            Dictionary<string, WorkWithJSON_mass<double>> dict_values = this.values_arrays.ConvertToDictionary();
            Dictionary<string, WorkWithJSON_mass<string>> dict_values_str = this.values_str_arrays.ConvertToDictionary();

            this.dict_values = new Dictionary<string, double[]>();
            this.dict_values_str = new Dictionary<string, string[]>();

            foreach (KeyValuePair<string, WorkWithJSON_mass<double>> kvp in dict_values)
            {
                this.dict_values.Add(kvp.Key, kvp.Value.item);
            }
            foreach (KeyValuePair<string, WorkWithJSON_mass<string>> kvp in dict_values_str)
            {
                this.dict_values_str.Add(kvp.Key, kvp.Value.item);
            }

            this.signs = signs_arrays.ConvertToDictionary();
        }
    }
}
