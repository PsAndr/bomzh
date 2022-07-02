using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Engine
{
    public class VideoLoader
    {
        public MyDictionary<string, VideoClip> videoSources;
        public MyDictionary<int, string> videoNames;

        public VideoLoader()
        {
            videoSources = new MyDictionary<string, VideoClip>();
            videoNames = new MyDictionary<int, string>();

            VideoFinder videoFinder = new VideoFinder();

            for (int i = 0; i < videoFinder.pathsVideo.Count; i++)
            {
                VideoClip video = Resources.Load<VideoClip>(videoFinder.pathsVideo[i]);

                videoSources.Add(videoFinder.namesVideo[i], video);
                videoNames.Add(videoFinder.numbersVideo[i], videoFinder.namesVideo[i]);
            }
        }
    }
}
