using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader
{
    public Dictionary<string, AudioClip> audioSources;
    public Dictionary<int, string> audioNames;

    public AudioLoader()
    {
        audioSources = new Dictionary<string, AudioClip>();
        audioNames = new Dictionary<int, string>();

        AudioFinder audioFinder = new AudioFinder();

        for (int i = 0; i < audioFinder.pathsAudio.Count; i++)
        {
            AudioClip audio = Resources.Load<AudioClip>($"Audio/{audioFinder.pathsAudio[i]}");

            audioSources.Add(audioFinder.namesAudio[i], audio);
            audioNames.Add(audioFinder.numbersAudio[i], audioFinder.namesAudio[i]);
        }
    }
}
