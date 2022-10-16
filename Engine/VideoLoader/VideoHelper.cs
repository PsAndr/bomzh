using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Engine.WorkWithRectTransform;
using System;

namespace Engine
{
    public class VideoHelper : MonoBehaviour
    {
        [System.Serializable]
        public class SaveClass : IComparable
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

            [HideInInspector] public int hierarchyPosition;

            [HideInInspector] public RectTransformSaveValuesSerializable rectTransformVideo;

            public int CompareTo(object x)
            {
                object y = this;

                if (x == null || y == null)
                {
                    throw new Exception("Compare VideoHelper.SaveClass with null");
                }
                if (((SaveClass)x).hierarchyPosition < ((SaveClass)y).hierarchyPosition)
                {
                    return 1;
                }
                else if (((SaveClass)x).hierarchyPosition > ((SaveClass)y).hierarchyPosition)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }

            public override string ToString()
            {
                string toReturn = string.Empty;

                toReturn += '(' + nameHelper.ToString() + ", " + cnt.ToString() + ", " + indexVideo.ToString() + ", " + volume.ToString()
                    + ", " + playbackSpeed.ToString() + ", " + panStereo.ToString() + ", " + time.ToString() + ", " + isPaused.ToString()
                    + ", " + isDeleted.ToString() + ", " + startWait.ToString() + ", " + betweenWait.ToString() + ", " + hierarchyPosition.ToString() + ')';

                return toReturn;
            }
        }

        private SaveClass saveClass;
        private bool isEndPlay;

        public void Init(string nameHelper, string[] nameVideo, int cnt, float volume, float playbackSpeed, float panStereo, float time, float startWait, float betweenWait,
            int indexVideo, VideoHandler videoHandler, Transform toSpawnVideo, RectTransformSaveValuesSerializable rectTransformVideo, int hierarchyPosition, params VideoClip[] video)
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

            this.saveClass.hierarchyPosition = hierarchyPosition;

            this.saveClass.isPaused = false;
            this.saveClass.isDeleted = false;
            this.isEndPlay = false;

            this.saveClass.rectTransformVideo = rectTransformVideo;

            StartCoroutine(this.PlayClipCoroutine(cnt, video, panStereo, volume, playbackSpeed, time, startWait, betweenWait, indexVideo, videoHandler, toSpawnVideo, rectTransformVideo, hierarchyPosition));
        }

        IEnumerator PlayClipCoroutine(int cnt, VideoClip[] video, float panStereo, float volume, float playbackSpeed, float time, float startWait, float betweenWait,
            int indexVideo, VideoHandler videoHandler, Transform toSpawnVideo, RectTransformSaveValuesSerializable rectTransformVideo, int hierarchyPosition)
        {
            Global_control global_Control = FindObjectOfType<Global_control>();

            VideoPlayer videoSource = videoHandler.GetVideoPlayer();
            AudioSource audioSource = videoHandler.GetAudioSource();

            videoSource.renderMode = VideoRenderMode.RenderTexture;
            videoSource.audioOutputMode = VideoAudioOutputMode.Direct;

            Image image = new GameObject(saveClass.nameHelper + "___video").AddComponent<Image>();
            image.gameObject.transform.SetParent(toSpawnVideo);

            image.transform.SetSiblingIndex(hierarchyPosition);

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

                    this.saveClass.hierarchyPosition = image.transform.GetSiblingIndex();

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

                            this.saveClass.hierarchyPosition = image.transform.GetSiblingIndex();

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
                    renderTexture.name = $"texture of video: {video[j].name}";
                    renderTexture.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat;

                    videoSource.playbackSpeed = playbackSpeed;
                    videoSource.SetDirectAudioVolume(0, volume * (global_Control.settings.Volume / 100f));
                    audioSource.panStereo = panStereo;
                    videoSource.clip = video[j];
                    videoSource.isLooping = false;
                    videoSource.Play();
                    videoSource.time = time;
                    videoSource.playOnAwake = false;
                    videoSource.SetTargetAudioSource(0, audioSource);

                    videoSource.targetTexture = renderTexture;
                    
                    RenderTexture.active = renderTexture;

                    image.material = new Material(image.material);

                    image.material.mainTexture = renderTexture;
                    image.material.name = $"material of video: {video[j].name}";

                    time = 0f;

                    videoSource.loopPointReached -= this.End;
                    videoSource.loopPointReached += this.End;

                    while (!isEndPlay)
                    {
                        this.saveClass.time = (float)videoSource.time;

                        this.saveClass.hierarchyPosition = image.transform.GetSiblingIndex();

                        videoSource.SetDirectAudioVolume(0, volume * (global_Control.settings.Volume / 100f));

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
