using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Engine.Files;

namespace Engine
{
    public class VideoFinder
    {
        public List<string> pathsVideo;
        public List<int> numbersVideo;
        public List<string> namesVideo;

        private static class Constants
        {
            public static readonly string[] types = { "mp4", "mov" };
            public static readonly string nameList = "saveListVideo";
            public static readonly string nameDirectory = "Video";
            public static readonly string pathToResource = Application.dataPath + @"/Resources/";
            public static readonly string pathResourceList = $"{nameDirectory}/{nameList}";
            public static readonly string typeList = "txt";
            public static readonly string path = pathToResource + $"{nameDirectory}/";
            public static readonly string pathList = $"{path}/{nameList}.{typeList}";
            public static readonly System.Text.Encoding encoding = System.Text.Encoding.UTF8;
        }

        public VideoFinder()
        {
            pathsVideo = new List<string>();
            numbersVideo = new List<int>();
            namesVideo = new List<string>();

            if (Application.isEditor)
            {
                string path = Constants.path;

                WorkWithFiles.CheckPath(path);
                WorkWithFiles.CheckFiles(path, Constants.types);

                Pair<string, int>[] numsAndNames = WorkWithFiles.GetFilesNumsAndNames(path, Constants.types);

                string text_file_list = "";

                foreach ((string name, int number) in numsAndNames)
                {
                    this.namesVideo.Add(name);
                    this.numbersVideo.Add(number);
                    this.pathsVideo.Add($"{Constants.nameDirectory}/{name} {number}");
                    text_file_list += $"{name} {number}\n";
                }

                File.WriteAllText(Constants.pathList, text_file_list, Constants.encoding);
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
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

                    this.pathsVideo.Add($"{Constants.nameDirectory}/{nameNumAudio}");
                    this.namesVideo.Add(name);
                    this.numbersVideo.Add(number);
                }
            }
        }
    }
}
