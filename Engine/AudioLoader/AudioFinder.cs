using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Engine.Files;

namespace Engine
{
    public class AudioFinder
    {
        public List<string> pathsAudio;
        public List<string> namesAudio;
        public List<int> numbersAudio;

        private static class Constants
        {
            public static readonly string[] types = { "mp3" };
            public static readonly string nameList = "saveListAudio";
            public static readonly string nameDirectory = "Audio";
            public static readonly string pathToResource = Application.dataPath + @"/Resources/";
            public static readonly string pathResourceList = $"{nameDirectory}/{nameList}";
            public static readonly string typeList = "txt";
            public static readonly string path = pathToResource + $"{nameDirectory}/";
            public static readonly string pathList = $"{path}/{nameList}.{typeList}";
            public static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        }

        public AudioFinder()
        {
            pathsAudio = new List<string>();
            namesAudio = new List<string>();
            numbersAudio = new List<int>();

            if (Application.isEditor)
            {
                string path = Constants.path;

                WorkWithFiles.CheckPath(path);
                WorkWithFiles.CheckFiles(path, Constants.types);

                Pair<string, int>[] numsAndNames = WorkWithFiles.GetFilesNumsAndNames(path, Constants.types);

                string text_file_list = "";

                foreach ((string name, int number) in numsAndNames)
                {
                    this.namesAudio.Add(name);
                    this.numbersAudio.Add(number);
                    this.pathsAudio.Add($"{Constants.nameDirectory}/{name} {number}");
                    text_file_list += $"{name} {number}\n";
                }

                File.WriteAllText(Constants.pathList, text_file_list, Constants.encoding);
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.Android)
            {
                string textFileList = Resources.Load<TextAsset>(Constants.pathResourceList).text;

                string[] nameNumAudios = textFileList.Split('\n');

                foreach (string nameNumAudio in nameNumAudios)
                {
                    if (string.IsNullOrEmpty(nameNumAudio))
                    {
                        continue;
                    }
                    string[] nameNumAudioSplit = nameNumAudio.Split(' ');

                    string name = nameNumAudioSplit[0];
                    int number = Convert.ToInt32(nameNumAudioSplit[1]);

                    this.pathsAudio.Add($"{Constants.nameDirectory}/{nameNumAudio}");
                    this.namesAudio.Add(name);
                    this.numbersAudio.Add(number);
                }
            }
        }
    }
}
