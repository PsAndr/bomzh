using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Engine.WorkWithRectTransform;

namespace Engine
{
    /// <summary>
    /// Control and handle video on scene
    /// </summary>
    public class VideoHandler : MonoBehaviour
    {
        public void PlayVideo(string name, int cnt, float volume, float playbackSpeed, float panStereo, float startWait, float betweenWait, 
            Transform toSpawn, RectTransformSaveValuesSerializable rectTransformVideo, int hierarchyPosition, params VideoClip[] video)
        {
            string[] names = new string[video.Length];
            for (int i = 0; i < video.Length; i++)
            {
                names[i] = video[i].name.Split(' ')[0];
            }

            gameObject.AddComponent<VideoHelper>().Init(name, names, cnt, volume, playbackSpeed, panStereo, 0f, startWait, betweenWait, 0, this, 
                toSpawn, rectTransformVideo, hierarchyPosition, video);
        }

        public void PlayVideo(Global_control globalControl, VideoHelper.SaveClass videoHelper)
        {
            VideoHelper videoHelperNew = gameObject.AddComponent<VideoHelper>();

            VideoClip[] videoClips = new VideoClip[videoHelper.nameVideos.Length];
            for (int i = 0; i < videoClips.Length; i++)
            {
                videoClips[i] = globalControl.videoLoader.videoSources[videoHelper.nameVideos[i]];
            }

            videoHelperNew.Init(videoHelper.nameHelper, videoHelper.nameVideos, videoHelper.cnt, videoHelper.volume, videoHelper.playbackSpeed, 
                videoHelper.panStereo, videoHelper.time, videoHelper.startWait, videoHelper.betweenWait, videoHelper.indexVideo, this, 
                globalControl.toSpawnVideos, videoHelper.rectTransformVideo, videoHelper.hierarchyPosition, videoClips);
        }

        public VideoPlayer GetVideoPlayer()
        {
            VideoPlayer videoPlayer = null;

            foreach (VideoPlayer video in gameObject.GetComponents<VideoPlayer>())
            {
                if (video.clip == null)
                {
                    videoPlayer = video;
                    break;
                }
            }

            if (videoPlayer == null)
            {
                videoPlayer = gameObject.AddComponent<VideoPlayer>();
            }

            return videoPlayer;
        }

        public AudioSource GetAudioSource()
        {
            return new GameObject("___helperVideoToPlayAudio___").AddComponent<AudioSource>();
        }

        public VideoHelper GetVideoHelper(string name)
        {
            VideoHelper[] videoHelpers = gameObject.GetComponents<VideoHelper>();

            foreach (VideoHelper videoHelper in videoHelpers)
            {
                if (videoHelper.GetSave().nameHelper == name)
                {
                    return videoHelper;
                }
            }

            return null;
        }

        public void DeleteVideo(string name)
        {
            VideoHelper videoHelper = GetVideoHelper(name);

            if (videoHelper != null)
            {
                videoHelper.Delete();
            }
        }

        public void StopAll()
        {
            VideoHelper[] videoHelpers = gameObject.GetComponents<VideoHelper>();

            foreach (VideoHelper videoHelper in videoHelpers)
            {
                videoHelper.Delete();
            }
        }
    }
}
