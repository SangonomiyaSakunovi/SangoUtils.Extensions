using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace SangoUtils.Extensions_Unity.Utils
{
    internal class VideoPlayerSample
    {
        Dictionary<string, VideoPlayerController> _videoPlayerDict = new Dictionary<string, VideoPlayerController>();

        public bool AddVideoPlayer(string videoPlayerId, VideoPlayerConfig config)
        {
            bool res = false;
            config.VideoRawImage.texture = config.VideoRenderTexture;
            Transform videoPlayerTrans = config.VideoRawImage.transform;
            VideoPlayerController controller = videoPlayerTrans.GetComponent<VideoPlayerController>() ?? videoPlayerTrans.gameObject.AddComponent<VideoPlayerController>();
            controller.InitController(config);
            if (!_videoPlayerDict.ContainsKey(videoPlayerId))
            {
                _videoPlayerDict.Add(videoPlayerId, controller);
                res = true;
            }
            return res;
        }

        public bool LoadVideo(string videoPlayerId, string videoPath)
        {
            bool res = false;
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                res = controller.LoadVideo(videoPath);
            }
            return res;
        }

        public bool LoadVideo(string videoPlayerId, VideoClip videoClip)
        {
            bool res = false;
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                res = controller.LoadVideo(videoClip);
            }
            return res;
        }

        public bool PlayVideo(string videoPlayerId)
        {
            bool res = false;
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                res = controller.PlayVideo();
            }
            return res;
        }

        public bool PauseVideo(string videoPlayerId)
        {
            bool res = false;
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                res = controller.PauseVideo();
            }
            return res;
        }

        public bool StopVideo(string videoPlayerId)
        {
            bool res = false;
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                res = controller.StopVideo();
            }
            return res;
        }

        public void AddVideoLoopPointReached(string videoPlayerId, VideoPlayer.EventHandler callBack)
        {
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                controller.AddVideoLoopPointReached(callBack);
            }
        }

        public void RemoveVideoLoopPointReached(string videoPlayerId, VideoPlayer.EventHandler callBack)
        {
            if (_videoPlayerDict.TryGetValue(videoPlayerId, out VideoPlayerController controller))
            {
                controller.RemoveVideoLoopPointReached(callBack);
            }
        }
    }
}
