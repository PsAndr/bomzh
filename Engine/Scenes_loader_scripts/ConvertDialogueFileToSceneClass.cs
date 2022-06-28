using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace Engine
{
    public class ConvertDialogueFileToSceneClass
    {
        public List<Scene_class.DialogueOrChoiceOrCommand> parts;

        public ConvertDialogueFileToSceneClass(string path)
        {
            parts = new List<Scene_class.DialogueOrChoiceOrCommand>();

            string[] file_lines = File.ReadAllLines(path);

            int type_text = 0;

            Scene_class.NeedFlag[] needFlags = new Scene_class.NeedFlag[0];
            string text = "";
            string character_name = "";

            List<Scene_class.NeedFlag[]> needFlagsChoices = new List<Scene_class.NeedFlag[]>();
            List<string> choices = new List<string>();
            List<Scene_class.ChangeFlag[]> changeFlagsChoice = new List<Scene_class.ChangeFlag[]>();
            List<Scene_class.Command[]> commandsChoice = new List<Scene_class.Command[]>();
            int index_choice = 0;

            foreach (string line in file_lines)
            {
                switch (type_text)
                {
                    case 0:
                        if (line[0] == '{')
                        {
                            type_text = 1;

                            string[] parts_line = line[1..].Split(';');

                            if (!string.IsNullOrEmpty(parts_line[0]))
                            {
                                needFlags = new Scene_class.NeedFlag[Count(parts_line[0], '[')];

                                int index = 0;
                                int ind_flag = 0;

                                while (index != -1)
                                {
                                    index++;
                                    int index_end = parts_line[0].IndexOf(']', index);
                                    string Flag = parts_line[0][index..index_end];

                                    if (Flag.IndexOf('>') != -1)
                                    {
                                        string flagName = Flag.Split('>')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('>')[1]);
                                        needFlags[ind_flag] = new Scene_class.NeedFlag(flagName, '>', flagValue);
                                    }
                                    else if (Flag.IndexOf('<') != -1)
                                    {
                                        string flagName = Flag.Split('>')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('>')[1]);
                                        needFlags[ind_flag] = new Scene_class.NeedFlag(flagName, '<', flagValue);
                                    }
                                    else if (Flag.IndexOf('=') != -1)
                                    {
                                        string flagName = Flag.Split('=')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('=')[1]);
                                        needFlags[ind_flag] = new Scene_class.NeedFlag(flagName, '=', flagValue);
                                    }

                                    index = parts_line[0].IndexOf('[', index);
                                    ind_flag++;
                                }
                            }
                            else
                            {
                                needFlags = new Scene_class.NeedFlag[0];
                            }

                            character_name = parts_line[1];
                        }
                        else if (line[0] == '[')
                        {
                            type_text = 2;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(line))
                            {
                                string[] parts_line = line.Split(';')[0].Split(':');
                                string name_command = parts_line[0].Split('_')[0];
                                string number_or_name = parts_line[0].Split('_')[1].Split('=')[0];

                                int number = -1;
                                string name = null;

                                Dictionary<string, double[]> dict_values = new Dictionary<string, double[]>();
                                Dictionary<string, string[]> dict_values_str = new Dictionary<string, string[]>();

                                switch (number_or_name)
                                {
                                    case "number":
                                        number = Convert.ToInt32(parts_line[0].Split('=')[1]);
                                        break;

                                    case "name":
                                        name = parts_line[0].Split('=')[1];
                                        break;
                                }

                                Dictionary<string, char> signs = new Dictionary<string, char>();

                                for (int i = 1; i < parts_line.Length; i++)
                                {
                                    string val_name = null;
                                    string[] val_of_val_str = null;
                                    double[] val_of_val = null;

                                    if (parts_line[i].IndexOf('=') != -1)
                                    {
                                        val_name = parts_line[i].Split('=')[0];
                                        val_of_val_str = parts_line[i].Split('=')[1].Split(',');
                                        val_of_val = new double[val_of_val_str.Length];

                                        signs.Add(val_name, '=');
                                    }
                                    if (parts_line[i].IndexOf('+') != -1)
                                    {
                                        val_name = parts_line[i].Split('+')[0];
                                        val_of_val_str = parts_line[i].Split('+')[1].Split(',');
                                        val_of_val = new double[val_of_val_str.Length];

                                        signs.Add(val_name, '+');
                                    }

                                    if (val_name.IndexOf("name") == -1)
                                    {
                                        for (int j = 0; j < val_of_val_str.Length; j++)
                                        {
                                            val_of_val[j] = double.Parse(val_of_val_str[j], System.Globalization.CultureInfo.InvariantCulture);
                                        }

                                        dict_values.Add(val_name, val_of_val);
                                    }
                                    else
                                    {
                                        dict_values_str.Add(val_name, val_of_val_str);
                                    }
                                }

                                this.parts.Add(new Scene_class.DialogueOrChoiceOrCommand(2,
                                    new Scene_class.Command(name_command, name, number, dict_values, dict_values_str, signs, GetNeedFlags(line.Split(';')[1]))));
                            }
                        }
                        break;

                    case 1:
                        if (line[0] == '}')
                        {
                            text = text[..^1];
                            this.parts.Add(new Scene_class.DialogueOrChoiceOrCommand(0, new Scene_class.DialogueText(needFlags, character_name, text)));

                            text = "";
                            needFlags = new Scene_class.NeedFlag[0];
                            character_name = "";

                            type_text = 0;
                        }
                        else
                        {
                            text += line + '\n';
                        }
                        break;

                    case 2:
                        if (line[0] == ']')
                        {
                            this.parts.Add(new Scene_class.DialogueOrChoiceOrCommand(1, new Scene_class.ChoiceText(needFlagsChoices.ToArray(), choices.ToArray(),
                                changeFlagsChoice.ToArray(), commandsChoice.ToArray())));

                            type_text = 0;

                            needFlagsChoices = new List<Scene_class.NeedFlag[]>();
                            changeFlagsChoice = new List<Scene_class.ChangeFlag[]>();
                            choices = new List<string>();
                            commandsChoice = new List<Scene_class.Command[]>();
                            index_choice = 0;
                        }
                        else
                        {
                            string[] parts_line = line.Split(';');
                            Scene_class.NeedFlag[] needFlags_choice = new Scene_class.NeedFlag[0];

                            if (!string.IsNullOrEmpty(parts_line[0]))
                            {
                                needFlags_choice = new Scene_class.NeedFlag[Count(parts_line[0], '[')];

                                int index = 0;
                                int ind_flag = 0;

                                while (index != -1)
                                {
                                    index++;
                                    int index_end = parts_line[0].IndexOf(']', index);
                                    string Flag = parts_line[0][index..index_end];

                                    if (Flag.IndexOf('>') != -1)
                                    {
                                        string flagName = Flag.Split('>')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('>')[1]);
                                        needFlags_choice[ind_flag] = new Scene_class.NeedFlag(flagName, '>', flagValue);
                                    }
                                    else if (Flag.IndexOf('<') != -1)
                                    {
                                        string flagName = Flag.Split('>')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('>')[1]);
                                        needFlags_choice[ind_flag] = new Scene_class.NeedFlag(flagName, '<', flagValue);
                                    }
                                    else if (Flag.IndexOf('=') != -1)
                                    {
                                        string flagName = Flag.Split('=')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('=')[1]);
                                        needFlags_choice[ind_flag] = new Scene_class.NeedFlag(flagName, '=', flagValue);
                                    }

                                    index = parts_line[0].IndexOf('[', index);
                                    ind_flag++;
                                }
                            }
                            else
                            {
                                needFlags_choice = new Scene_class.NeedFlag[0];
                            }

                            needFlagsChoices.Add(needFlags_choice);

                            choices.Add(parts_line[1]);

                            Scene_class.ChangeFlag[] changeFlags_choice = new Scene_class.ChangeFlag[0];

                            if (!string.IsNullOrEmpty(parts_line[2]))
                            {
                                changeFlags_choice = new Scene_class.ChangeFlag[Count(parts_line[2], '<')];

                                int index = 0;
                                int ind_flag = 0;

                                while (index != -1)
                                {
                                    index++;
                                    int index_end = parts_line[2].IndexOf('>', index);
                                    string Flag = parts_line[2][index..index_end];

                                    if (Flag.IndexOf('-') != -1)
                                    {
                                        string flagName = Flag.Split('-')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('-')[1]);
                                        changeFlags_choice[ind_flag] = new Scene_class.ChangeFlag(flagName, '-', flagValue);
                                    }
                                    else if (Flag.IndexOf('+') != -1)
                                    {
                                        string flagName = Flag.Split('+')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('+')[1]);
                                        changeFlags_choice[ind_flag] = new Scene_class.ChangeFlag(flagName, '+', flagValue);
                                    }
                                    else if (Flag.IndexOf('=') != -1)
                                    {
                                        string flagName = Flag.Split('=')[0];
                                        int flagValue = Convert.ToInt32(Flag.Split('=')[1]);
                                        changeFlags_choice[ind_flag] = new Scene_class.ChangeFlag(flagName, '=', flagValue);
                                    }

                                    index = parts_line[2].IndexOf('<', index);
                                    ind_flag++;
                                }
                            }
                            else
                            {
                                changeFlags_choice = new Scene_class.ChangeFlag[0];
                            }

                            changeFlagsChoice.Add(changeFlags_choice);

                            Scene_class.Command[] commands_choice = new Scene_class.Command[parts_line.Length - 3];

                            if (!string.IsNullOrEmpty(parts_line[3]))
                            {
                                for (int i = 3; i < parts_line.Length; i++)
                                {
                                    if (string.IsNullOrEmpty(parts_line[i]))
                                    {
                                        continue;
                                    }

                                    string command = parts_line[i];

                                    Scene_class.Command new_command_choice = null;

                                    string[] parts_command = command.Split(':');
                                    string name_command = parts_command[0].Split('_')[0];
                                    string number_or_name = parts_command[0].Split('_')[1].Split('=')[0];

                                    int number = -1;
                                    string name = null;

                                    Dictionary<string, double[]> dict_values = new Dictionary<string, double[]>();
                                    Dictionary<string, string[]> dict_values_str = new Dictionary<string, string[]>();

                                    switch (number_or_name)
                                    {
                                        case "number":
                                            number = Convert.ToInt32(parts_command[0].Split('=')[1]);
                                            break;

                                        case "name":
                                            name = parts_command[0].Split('=')[1];
                                            break;
                                    }

                                    Dictionary<string, char> signs = new Dictionary<string, char>();

                                    for (int j = 1; j < parts_command.Length; j++)
                                    {
                                        string val_name = null;
                                        string[] val_of_val_str = null;
                                        double[] val_of_val = null;

                                        if (parts_command[i].IndexOf('=') != -1)
                                        {
                                            val_name = parts_command[i].Split('=')[0];
                                            val_of_val_str = parts_command[i].Split('=')[1].Split(',');
                                            val_of_val = new double[val_of_val_str.Length];

                                            signs.Add(val_name, '=');
                                        }
                                        if (parts_command[i].IndexOf('+') != -1)
                                        {
                                            val_name = parts_command[i].Split('+')[0];
                                            val_of_val_str = parts_command[i].Split('+')[1].Split(',');
                                            val_of_val = new double[val_of_val_str.Length];

                                            signs.Add(val_name, '+');
                                        }

                                        if (val_name.IndexOf("name") == -1)
                                        {
                                            for (int k = 0; k < val_of_val_str.Length; k++)
                                            {
                                                val_of_val[k] = double.Parse(val_of_val_str[k], System.Globalization.CultureInfo.InvariantCulture);
                                            }

                                            dict_values.Add(val_name, val_of_val);
                                        }
                                        else
                                        {
                                            dict_values_str.Add(val_name, val_of_val_str);
                                        }
                                    }

                                    new_command_choice = new Scene_class.Command(name_command, name, number, dict_values, dict_values_str, signs, new Scene_class.NeedFlag[0]);

                                    commands_choice[i - 3] = new_command_choice;
                                }
                            }
                            else
                            {
                                commands_choice = new Scene_class.Command[0];
                            }

                            commandsChoice.Add(commands_choice);

                            index_choice++;
                        }
                        break;
                }
            }
        }
        private int Count(string str, char ch)
        {
            int cnt = 0;
            foreach (char c in str)
            {
                if (c == ch)
                {
                    cnt++;
                }
            }
            return cnt;
        }

        public Scene_class.NeedFlag[] GetNeedFlags(string line)
        {
            List<Scene_class.NeedFlag> needFlags = new List<Scene_class.NeedFlag>();

            int startFlag = line.IndexOf('[');
            int endFlag = 0;

            while (startFlag != -1 && endFlag != -1)
            {
                startFlag++;
                endFlag = line.IndexOf(']', startFlag);

                if (endFlag != -1)
                {
                    string strFlag = line[startFlag..endFlag];

                    string nameFlag;
                    int valueFlag;
                    char signFlag;

                    if (strFlag.IndexOf('=') != -1)
                    {
                        signFlag = '=';
                        valueFlag = Convert.ToInt32(strFlag.Split('=')[1]);
                        nameFlag = strFlag.Split('=')[0];
                        Scene_class.NeedFlag needFlag = new Scene_class.NeedFlag(nameFlag, signFlag, valueFlag);
                        needFlags.Add(needFlag);
                    }
                    else if (strFlag.IndexOf('>') != -1)
                    {
                        signFlag = '>';
                        valueFlag = Convert.ToInt32(strFlag.Split('>')[1]);
                        nameFlag = strFlag.Split('>')[0];
                        Scene_class.NeedFlag needFlag = new Scene_class.NeedFlag(nameFlag, signFlag, valueFlag);
                        needFlags.Add(needFlag);
                    }
                    else if (strFlag.IndexOf('<') != -1)
                    {
                        signFlag = '<';
                        valueFlag = Convert.ToInt32(strFlag.Split('<')[1]);
                        nameFlag = strFlag.Split('<')[0];
                        Scene_class.NeedFlag needFlag = new Scene_class.NeedFlag(nameFlag, signFlag, valueFlag);
                        needFlags.Add(needFlag);
                    }

                    startFlag = line.IndexOf('[', endFlag);
                }
            }

            return needFlags.ToArray();
        }
    }
}
