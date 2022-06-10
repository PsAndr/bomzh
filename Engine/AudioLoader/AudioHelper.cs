using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    [System.Serializable]
    public class AudioHelper : MonoBehaviour
    {
        [System.Serializable]
        public class SaveClass
        {
            [HideInInspector] public string nameHelper;
            [HideInInspector] public string nameAudio;

            [HideInInspector] public int cnt;
            [HideInInspector] public float volume;
            [HideInInspector] public float pitch;
            [HideInInspector] public float panStereo;

            public float time;

            [HideInInspector] public bool isPaused;
        }

        private SaveClass saveClass;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public SaveClass GetSave()
        {
            return this.saveClass;
        }

        public void Init(string nameHelper, string nameAudio, int cnt, float volume, float pitch, float panStereo, float time, AudioClip audio, AudioHandler audioHandler)
        {
            this.saveClass = new SaveClass();
            this.saveClass.nameHelper = nameHelper;
            this.saveClass.nameAudio = nameAudio;
            this.saveClass.cnt = cnt;
            this.saveClass.volume = volume;
            this.saveClass.pitch = pitch;
            this.saveClass.panStereo = panStereo;
            this.saveClass.time = time;

            StartCoroutine(this.PlayClipCoroutine(cnt, audio, panStereo, volume, pitch, time, audioHandler));
        }

        IEnumerator PlayClipCoroutine(int cnt, AudioClip audio, float panStereo, float volume, float pitch, float time, AudioHandler audioHandler)
        {
            AudioSource audioSource = audioHandler.GetAudioSource();

            if (cnt == -1)
            {
                audioSource.pitch = pitch;
                audioSource.volume = volume;
                audioSource.panStereo = panStereo;
                audioSource.clip = audio;
                audioSource.loop = true;
                audioSource.Play();
                audioSource.time = time;
            }
            else
            {
                audioSource.pitch = pitch;
                audioSource.volume = volume;
                audioSource.panStereo = panStereo;
                audioSource.clip = audio;
                audioSource.loop = false;
                for (int i = 0; i < cnt; i++)
                {
                    audioSource.Play();
                    audioSource.time = time;
                    time = 0f;

                    while (audioSource.isPlaying || this.saveClass.isPaused)
                    {
                        this.saveClass.time = audioSource.time;
                        yield return new WaitForSeconds(0.08f);
                    }

                    this.saveClass.cnt--;
                }

                audioSource.Stop();
                audioSource.clip = null;

                Destroy(this);
            }
            yield break;
        }

        public void Pause()
        {
            this.saveClass.isPaused = true;
        }

        public void UnPause()
        {
            this.saveClass.isPaused = false;
        }
    }
}
