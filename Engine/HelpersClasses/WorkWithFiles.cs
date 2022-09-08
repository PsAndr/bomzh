using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Engine.Files
{
    public class WorkWithFiles
    {
        static public void CheckPath(string path)
        {
            string[] pathParts = path.Split('/');
            string newPath = "";
            for (int i = 0; i < pathParts.Length - 1; i++)
            {
                newPath += pathParts[i] + '/';
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
            }
        }

        static public void CheckFiles(string path, params string[] types)
        {
            List<string> trash = new();
            List<string> trashTypes = new();
            HashSet<int> usedNumbers = new();

            foreach (string type in types)
            {
                string[] pathsWithTrash = Directory.GetFiles(path, $"*.{type}");

                foreach (string pathFull in pathsWithTrash)
                {
                    string pathPart = pathFull.Split('/')[^1][..^(type.Length + 1)];
                    if (pathPart.Split(' ').Length < 2)
                    {
                        trash.Add(pathPart);
                        trashTypes.Add(type);
                    }
                    else if (pathPart.Split(' ').Length == 2)
                    {
                        try
                        {
                            int numToUsed = Convert.ToInt32(pathPart.Split(' ')[1]);

                            if (numToUsed < 0)
                            {
                                trash.Add(pathPart);
                                trashTypes.Add(type);
                            }
                            else
                            {
                                usedNumbers.Add(numToUsed);
                            }
                        }
                        catch { }
                    }
                }
            }

            int num = 0;
            int i = 0;

            foreach (string pathPart in trash)
            {
                while (usedNumbers.Contains(num))
                {
                    num++;
                }
                string newPath = pathPart.Split(' ')[0] + $" {num}.{trashTypes[i]}";

                File.Copy(path + pathPart + $".{trashTypes[i]}", path + newPath, true);
                File.Delete(path + pathPart + $".{trashTypes[i]}");

                if (File.Exists(path + pathPart + $".{trashTypes[i]}.meta"))
                {
                    File.Copy(path + pathPart + $".{trashTypes[i]}.meta", path + newPath + ".meta", true);
                    File.Delete(path + pathPart + $".{trashTypes[i]}.meta");
                }

                usedNumbers.Add(num);
                i++;
            }

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        static public Pair<string, int>[] GetFilesNumsAndNames(string path, params string[] types)
        {
            List<Pair<string, int>> result = new List<Pair<string, int>>();

            foreach (string type in types)
            {
                string[] pathsWithTrash = Directory.GetFiles(path, $"* *.{type}");

                foreach (string str in pathsWithTrash)
                {
                    string nameAndNum = str.Split('/')[^1][..^4];
                    string[] nameAndNumSplit = nameAndNum.Split(' ');

                    string name = nameAndNumSplit[0];
                    int number;
                    try
                    {
                        number = Convert.ToInt32(nameAndNumSplit[1]);
                    }
                    catch
                    {
                        continue;
                    }

                    result.Add(new Pair<string, int>(name, number));
                }
            }

            return result.ToArray();
        }

        static public void CheckDirectory(string pathDirectory)
        {
            string[] directories = pathDirectory.Split('/');

            if (directories[^1].IndexOf('.') != -1)
            {
                directories[^1] = string.Empty;
            }

            string path = "";

            foreach (string directory in directories)
            {
                path += directory + "/";

                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch { }
                }
            }
        }
    }
}
