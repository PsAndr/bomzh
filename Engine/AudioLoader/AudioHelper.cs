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
            [HideInInspector] public string[] nameAudio;

            [HideInInspector] public int cnt;
            [HideInInspector] public int indexAudio;

            [HideInInspector] public float volume;
            [HideInInspector] public float pitch;
            [HideInInspector] public float panStereo;

            [HideInInspector] public float time;

            [HideInInspector] public bool isPaused;

            [HideInInspector] public float startWait;
            [HideInInspector] public float betweenWait;
        }

        private SaveClass saveClass;

        public SaveClass GetSave()
        {
            return this.saveClass;
        }

        public void Init(string nameHelper, string[] nameAudio, int cnt, float volume, float pitch, float panStereo, float time, float startWait, float betweenWait, 
            int indexAudio, AudioHandler audioHandler, params AudioClip[] audio)
        {
            this.saveClass = new SaveClass();
            this.saveClass.nameHelper = nameHelper;
            this.saveClass.nameAudio = nameAudio;
            this.saveClass.cnt = cnt;
            this.saveClass.volume = volume;
            this.saveClass.pitch = pitch;
            this.saveClass.panStereo = panStereo;
            this.saveClass.time = time;
            this.saveClass.betweenWait = betweenWait;
            this.saveClass.startWait = startWait;
            this.saveClass.indexAudio = indexAudio;

            StartCoroutine(this.PlayClipCoroutine(cnt, audio, panStereo, volume, pitch, time, startWait, betweenWait, indexAudio, audioHandler));
        }

        IEnumerator PlayClipCoroutine(int cnt, AudioClip[] audio, float panStereo, float volume, float pitch, float time, float startWait, float betweenWait, 
            int indexAudio, AudioHandler audioHandler)
        {
            AudioSource audioSource = audioHandler.GetAudioSource();

            int k = 1;

            if (cnt == -1)
            {
                k = 0;
                cnt = 1;
            }

            this.saveClass.startWait = startWait;
            while (this.saveClass.startWait > 0f)
            {
                this.saveClass.startWait -= 0.05f;
                yield return new WaitForSeconds(0.05f);
            }
            this.saveClass.startWait = 0f;

            for (int i = 0; i < cnt; i += k)
            {
                for (int j = indexAudio; j < audio.Length; j++)
                {
                    this.saveClass.indexAudio = j;
                    indexAudio = 0;

                    this.saveClass.startWait = betweenWait;
                    while (this.saveClass.startWait > 0f)
                    {
                        this.saveClass.startWait -= 0.05f;
                        yield return new WaitForSeconds(0.05f);
                    }
                    this.saveClass.startWait = 0f;

                    audioSource.pitch = pitch;
                    audioSource.volume = volume;
                    audioSource.panStereo = panStereo;
                    audioSource.clip = audio[j];
                    audioSource.loop = false;
                    audioSource.Play();
                    audioSource.time = time;
                    time = 0f;

                    while (audioSource.isPlaying || this.saveClass.isPaused)
                    {
                        this.saveClass.time = audioSource.time;

                        yield return new WaitForSeconds(0.08f);

                        if (this.saveClass.isPaused)
                        {
                            audioSource.Pause();
                        }
                        else
                        {
                            audioSource.UnPause();
                        }
                    }
                }

                if (this.saveClass.cnt != -1)
                {
                    this.saveClass.cnt--;
                }
            }

            audioSource.Stop();
            audioSource.clip = null;

            Destroy(this);
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
