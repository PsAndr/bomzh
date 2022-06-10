using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class AudioFinder
{
    public List<string> pathsAudio;
    public List<string> namesAudio;
    public List<int> numbersAudio;
    public AudioFinder()
    {
        pathsAudio = new List<string>();
        namesAudio = new List<string>();    
        numbersAudio = new List<int>();

        if (Application.isEditor)
        {
            string path = Application.dataPath + "/Resources/Audio/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string[] pathsAudioWithTrash = Directory.GetFiles(path, "* *.mp3");

            string textFileList = "";

            foreach (string pathTrash in pathsAudioWithTrash)
            {
                string nameNumAudio = pathTrash.Split('/')[^1][..^4];
                string[] nameNumAudioSplit = nameNumAudio.Split(' ');

                string name = nameNumAudioSplit[0];
                int number;

                try
                {
                    number = Convert.ToInt32(nameNumAudioSplit[1]);
                }
                catch
                {
                    continue;
                }

                textFileList += nameNumAudio + '\n';

                this.pathsAudio.Add(nameNumAudio);
                this.namesAudio.Add(name);
                this.numbersAudio.Add(number);
            }

            File.WriteAllText(path + "saveListAudio.txt", textFileList, System.Text.Encoding.UTF8);
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string textFileList = Resources.Load<TextAsset>("Audio/saveListAudio").text;

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

                this.pathsAudio.Add(nameNumAudio);
                this.namesAudio.Add(name);
                this.numbersAudio.Add(number);
            }
        }
    }
}
