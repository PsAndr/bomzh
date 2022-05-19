using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ConvertDialogueFileToSceneClass
{
    public List <Scene_class.DialogueOrChoiceOrCommand> parts;

    public ConvertDialogueFileToSceneClass(string path)
    {
        parts = new List<Scene_class.DialogueOrChoiceOrCommand>();

        string[] file_lines = File.ReadAllLines(path);

        int type_text = 0;

        Scene_class.NeedFlag[] needFlags = new Scene_class.NeedFlag[0];

        string text = "";
        string character_name = "";

        foreach (string line in file_lines)
        {
            Debug.Log(line);    
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
                                else if(Flag.IndexOf('<') != -1)
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
                            string[] parts_line = line.Split(';');
                            string name_command = parts_line[0].Split('_')[0];
                            string number_or_name = parts_line[0].Split('_')[1].Split('=')[0];

                            int number = -1;
                            string name = null;

                            Dictionary<string, double[]> dict_values = new Dictionary<string, double[]>();

                            switch (number_or_name)
                            {
                                case "number":
                                    number = Convert.ToInt32(parts_line[0].Split('=')[1]);
                                    break;

                                case "name":
                                    name = parts_line[0].Split('=')[1];
                                    break;
                            }

                            char[] signs = new char[parts_line.Length - 1];

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

                                    signs[i - 1] = '=';
                                }
                                if (parts_line[i].IndexOf('+') != -1)
                                {
                                    val_name = parts_line[i].Split('+')[0];
                                    val_of_val_str = parts_line[i].Split('+')[1].Split(',');
                                    val_of_val = new double[val_of_val_str.Length];

                                    signs[i - 1] = '+';
                                }

                                for (int j = 0; j < val_of_val_str.Length; j++)
                                {
                                    val_of_val[j] = Convert.ToDouble(val_of_val_str[j]);
                                }

                                dict_values.Add(val_name, val_of_val);
                            }

                            this.parts.Add(new Scene_class.DialogueOrChoiceOrCommand(2, new Scene_class.Command(name_command, name, number, dict_values, signs)));
                        }
                    }
                    break;

                case 1:
                    if (line[0] == '}')
                    {
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
                    // дописать
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
}

