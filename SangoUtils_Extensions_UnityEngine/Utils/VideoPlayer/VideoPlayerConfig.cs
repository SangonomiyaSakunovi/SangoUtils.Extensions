using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils_Extensions_UnityEngine.Utils
{
    public class VideoPlayerConfig
    {
        public VideoPlayerConfig(RawImage videoRawImage, RenderTexture videoRenderTexture)
        {
            VideoRawImage = videoRawImage;
            VideoRenderTexture = videoRenderTexture;
        }

        public RawImage VideoRawImage { get; private set; }
        public RenderTexture VideoRenderTexture { get; private set; }
        public float DefaultAudioVolume { get; set; } = 0.5f;

        public Button? PlayOrPauseBtn { get; set; }
        public Button? FullScreenBtn { get; set; }
        public Button? MuteBtn { get; set; }
        public Slider? VideoProgressSlider { get; set; }
        public Slider? AudioVolumeSlider { get; set; }
        
        public TMP_Text? VideoFullTimeTMPText { get; set; }
        public TMP_Text? VideoCurrentTimeTMPText { get; set; }

        public Vector2 NormalScreenRectTransValue { get; set; } = Vector2.zero;
        public Vector2 FullScreenRectTransValue { get; set; } = Vector2.zero;

        public Action<bool>? OnPlayOrPauseCallBack { get; set; }
        public Action<bool>? OnFullOrNormalScreenCallBack { get; set; }
        public Action<bool>? OnMuteOrUnMuteCallBack { get; set; }
    }
}