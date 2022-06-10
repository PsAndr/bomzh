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

        public void PlayClip(int cnt, AudioClip audio)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, cnt, 1f, 1f, 0f, 0f, audio, this);
        }

        public void PlayClip(AudioClip audio)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, 1, 1f, 1f, 0f, 0f, audio, this);
        }

        public void PlayClip(int cnt, AudioClip audio, float panStereo)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, cnt, 1f, 1f, panStereo, 0f, audio, this);
        }

        public void PlayClip(AudioClip audio, float panStereo)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, 1, 1f, 1f, panStereo, 0f, audio, this);
        }

        public void PlayClip(int cnt, AudioClip audio, float panStereo, float volume, float pitch)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, cnt, volume, pitch, panStereo, 0f, audio, this);
        }

        public void PlayClip(AudioClip audio, float panStereo, float volume)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, 1, volume, 1f, panStereo, 0f, audio, this);
        }

        public void PlayClip(AudioClip audio, float panStereo, float volume, float pitch)
        {
            gameObject.AddComponent<AudioHelper>().Init(null, audio.name, 1, volume, pitch, panStereo, 0f, audio, this);
        }

        public void PlayClip(Global_control global_Control, AudioHelper.SaveClass audioHelper)
        {
            AudioHelper audioHelperNew = gameObject.AddComponent<AudioHelper>();
            audioHelperNew.Init(audioHelper.nameHelper, audioHelper.nameAudio, audioHelper.cnt, audioHelper.volume,
                audioHelper.pitch, audioHelper.panStereo, audioHelper.time, global_Control.audioLoader.audioSources[audioHelper.nameAudio.Split(' ')[0]], this);
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
