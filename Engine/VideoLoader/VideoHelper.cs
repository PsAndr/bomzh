using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Engine.WorkWithRectTransform;

namespace Engine
{
    //дописать спавн картинки которая будет отображать видео
    public class VideoHelper : MonoBehaviour
    {
        [System.Serializable]
        public class SaveClass
        {
            [HideInInspector] public string nameHelper;
            [HideInInspector] public string[] nameVideos;

            [HideInInspector] public int cnt;
            [HideInInspector] public int indexVideo;

            [HideInInspector] public float volume;

            [HideInInspector] public float playbackSpeed;

            [HideInInspector] public float panStereo;

            [HideInInspector] public float time;

            [HideInInspector] public bool isPaused;
            [HideInInspector] public bool isDeleted;

            [HideInInspector] public float startWait;
            [HideInInspector] public float betweenWait;

            [HideInInspector] public RectTransformSaveValuesSerializable rectTransformVideo;
        }

        private SaveClass saveClass;
        private bool isEndPlay;

        public void Init(string nameHelper, string[] nameVideo, int cnt, float volume, float playbackSpeed, float panStereo, float time, float startWait, float betweenWait,
            int indexVideo, VideoHandler videoHandler, Transform toSpawnVideo, RectTransformSaveValuesSerializable rectTransformVideo, params VideoClip[] video)
        {
            this.saveClass = new SaveClass();
            this.saveClass.nameHelper = nameHelper;
            this.saveClass.nameVideos = nameVideo;
            this.saveClass.cnt = cnt;
            this.saveClass.volume = volume;
            this.saveClass.playbackSpeed = playbackSpeed;
            this.saveClass.panStereo = panStereo;
            this.saveClass.time = time;
            this.saveClass.betweenWait = betweenWait;
            this.saveClass.startWait = startWait;
            this.saveClass.indexVideo = indexVideo;

            this.saveClass.isPaused = false;
            this.saveClass.isDeleted = false;
            this.isEndPlay = false;

            this.saveClass.rectTransformVideo = rectTransformVideo;

            StartCoroutine(this.PlayClipCoroutine(cnt, video, panStereo, volume, playbackSpeed, time, startWait, betweenWait, indexVideo, videoHandler, toSpawnVideo, rectTransformVideo));
        }

        IEnumerator PlayClipCoroutine(int cnt, VideoClip[] video, float panStereo, float volume, float playbackSpeed, float time, float startWait, float betweenWait,
            int indexVideo, VideoHandler videoHandler, Transform toSpawnVideo, RectTransformSaveValuesSerializable rectTransformVideo)
        {
            VideoPlayer videoSource = videoHandler.GetVideoPlayer();
            AudioSource audioSource = videoHandler.GetAudioSource();

            Image image = new GameObject(saveClass.nameHelper).AddComponent<Image>();
            image.gameObject.transform.SetParent(toSpawnVideo);
            rectTransformVideo.UpdateRectTransform(image.gameObject.GetComponent<RectTransform>());

            int k = 1;

            if (cnt == -1)
            {
                k = 0;
                cnt = 1;
            }

            this.saveClass.startWait = startWait;
            while (this.saveClass.startWait > 0f)
            {
                if (!this.saveClass.isPaused)
                {
                    this.saveClass.startWait -= 0.05f;
                    yield return new WaitForSeconds(0.05f);
                }

                if (this.saveClass.isDeleted)
                {
                    DestroyIt(videoSource, audioSource, image);
                    yield break;
                }
            }
            this.saveClass.startWait = 0f;

            for (int i = 0; i < cnt; i += k)
            {
                for (int j = indexVideo; j < video.Length; j++)
                {
                    this.isEndPlay = false;
                    this.saveClass.indexVideo = j;
                    indexVideo = 0;

                    this.saveClass.startWait = betweenWait;
                    while (this.saveClass.startWait > 0f)
                    {
                        if (!this.saveClass.isPaused)
                        {
                            this.saveClass.startWait -= 0.05f;
                            yield return new WaitForSeconds(0.05f);
                        }

                        if (this.saveClass.isDeleted)
                        {
                            DestroyIt(videoSource, audioSource, image);
                            yield break;
                        }
                    }
                    this.saveClass.startWait = 0f;

                    RenderTexture renderTexture = new RenderTexture((int)video[j].width, (int)video[j].height, 32);

                    videoSource.playbackSpeed = playbackSpeed;
                    videoSource.SetTargetAudioSource(0, audioSource);
                    audioSource.volume = volume;
                    audioSource.panStereo = panStereo;
                    videoSource.clip = video[j];
                    videoSource.isLooping = false;
                    videoSource.Play();
                    videoSource.time = time;
                    videoSource.playOnAwake = false;

                    videoSource.targetTexture = renderTexture;
                    
                    RenderTexture.active = renderTexture;

                    image.material = new Material(image.material);

                    image.material.mainTexture = renderTexture;

                    time = 0f;

                    videoSource.loopPointReached += this.End;

                    while (!isEndPlay)
                    {
                        this.saveClass.time = (float)videoSource.time;

                        yield return new WaitForSeconds(0.08f);

                        if (this.saveClass.isPaused)
                        {
                            videoSource.Pause();
                        }
                        else
                        {
                            videoSource.Play();
                        }

                        if (this.saveClass.isDeleted)
                        {
                            DestroyIt(videoSource, audioSource, image);
                            yield break;
                        }
                    }
                }

                if (this.saveClass.cnt != -1)
                {
                    this.saveClass.cnt--;
                }
            }

            DestroyIt(videoSource, audioSource, image);

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

        public void Delete()
        {
            this.saveClass.isDeleted = true;
        }

        public SaveClass GetSave()
        {
            return this.saveClass;
        }

        private void DestroyIt(VideoPlayer videoPlayer, AudioSource audioSource, Image image)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;

            Destroy(this);
            Destroy(audioSource.gameObject);
            Destroy(image.gameObject);
        }

        private void End(VideoPlayer videoPlayer)
        {
            isEndPlay = true;
        }
    }
}
