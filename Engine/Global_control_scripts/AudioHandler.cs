using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    /// <summary>
    /// Control and handle all audio on scene
    /// </summary>
    public class AudioHandler : MonoBehaviour
    {
        public AudioHandler()
        {

        }

        public void PlayClip(int cnt, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, cnt, 1f, 1f, 0f, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, 1, 1f, 1f, 0f, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(int cnt, float panStereo, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, cnt, 1f, 1f, panStereo, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(float panStereo, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, 1, 1f, 1f, panStereo, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(int cnt, float panStereo, float volume, float pitch, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, cnt, volume, pitch, panStereo, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(int cnt, float panStereo, float volume, float pitch, float startWait, float betweenWait, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, cnt, volume, pitch, panStereo, 0f, startWait, betweenWait, 0, this, audio);
        }

        public void PlayClip(string name, int cnt, float panStereo, float volume, float pitch, float startWait, float betweenWait, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(name, audioNames, cnt, volume, pitch, panStereo, 0f, startWait, betweenWait, 0, this, audio);
        }

        public void PlayClip(float panStereo, float volume, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, 1, volume, 1f, panStereo, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(float panStereo, float volume, float pitch, params AudioClip[] audio)
        {
            string[] audioNames = new string[audio.Length];
            for (int i = 0; i < audio.Length; i++)
            {
                audioNames[i] = audio[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<AudioHelper>().Init(null, audioNames, 1, volume, pitch, panStereo, 0f, 0f, 0f, 0, this, audio);
        }

        public void PlayClip(Global_control global_Control, AudioHelper.SaveClass audioHelper)
        {
            AudioHelper audioHelperNew = gameObject.AddComponent<AudioHelper>();

            AudioClip[] audios = new AudioClip[audioHelper.nameAudio.Length];
            for (int i = 0; i < audioHelper.nameAudio.Length; i++)
            {
                audios[i] = global_Control.audioLoader.audioSources[audioHelper.nameAudio[i]];
            }

            audioHelperNew.Init(audioHelper.nameHelper, audioHelper.nameAudio, audioHelper.cnt, audioHelper.volume,
                audioHelper.pitch, audioHelper.panStereo, audioHelper.time, audioHelper.startWait, audioHelper.betweenWait, audioHelper.indexAudio, this, audios);
        }

        public void DeleteClip(string name)
        {
            AudioHelper audioHelper = GetAudioHelper(name);

            if (audioHelper != null)
            {
                audioHelper.Delete();
            }
        }

        public AudioHelper GetAudioHelper(string name)
        {
            AudioHelper[] audioHelpers = gameObject.GetComponents<AudioHelper>();

            foreach (AudioHelper audioHelper in audioHelpers)
            {
                if (audioHelper.GetSave().nameHelper == name)
                {
                    return audioHelper;
                }
            }

            return null;
        }

        public void ChangeClip(Global_control global_Control, string name, string newName = null, int cnt = 0, float panStereo = -1000f, float volume = -1000f, float pitch = -1000f, float startWait = -1000f, float betweenWait = -1000f, params AudioClip[] audio)
        {
            AudioHelper audioHelper = GetAudioHelper(name);

            if (!string.IsNullOrEmpty(newName))
            {
                name = newName;
            }
            if (cnt == 0)
            {
                cnt = audioHelper.GetSave().cnt;
            }
            if (panStereo == -1000f)
            {
                panStereo = audioHelper.GetSave().panStereo;
            }
            if (volume == -1000f)
            {
                volume = audioHelper.GetSave().volume;
            }
            if (pitch == -1000f)
            {
                pitch = audioHelper.GetSave().pitch;
            }
            if (startWait == -1000f)
            {
                startWait = audioHelper.GetSave().startWait;
            }
            if (betweenWait == -1000f)
            {
                betweenWait = audioHelper.GetSave().betweenWait;
            }
            if (audio == null || audio.Length == 0)
            {
                List<AudioClip> list = new List<AudioClip>();
                foreach (string str in audioHelper.GetSave().nameAudio)
                {
                    list.Add(global_Control.audioLoader.audioSources[str]);
                }
                audio = list.ToArray();
            }

            audioHelper.Delete();
            PlayClip(name, cnt, panStereo, volume, pitch, startWait, betweenWait, audio);
        }

        public AudioSource GetAudioSource()
        {
            AudioSource source = null;
            foreach (AudioSource audioSource in gameObject.GetComponents<AudioSource>())
            {
                if (!audioSource.isPlaying)
                {
                    source = audioSource;
                    break;
                }
            }
            if (source == null)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
            return source;
        }

        public void StopAll()
        {
            foreach (AudioSource audioSource in gameObject.GetComponents<AudioSource>())
            {
                audioSource.Stop();
            }

            foreach (AudioHelper audioHelper in gameObject.GetComponents<AudioHelper>())
            {
                Destroy(audioHelper);
            }
        }
    }
}
