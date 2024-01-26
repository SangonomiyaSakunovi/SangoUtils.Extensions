using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

namespace SangoUtils_Extensions_UnityEngine.Utils
{
    public class VideoPlayerController
    {
        private GameObject _videoObject;

        private VideoPlayer _videoPlayer;
        private AudioSource _audioSource;
        private RawImage _videoRawImage;
        private RectTransform _videoRawImageRectTrans;
        private RenderTexture _videoRenderTexture;

        private string _videoPath = string.Empty;
        private VideoClip? _videoClip = null;

        private Button? _playOrPauseBtn;
        private Button? _fullScreenBtn;
        private Button? _muteBtn;

        private Slider? _videoProgressSlider;
        private Slider? _audioVolumeSlider;
        private TMP_Text? _videoFullTimeTMPText;
        private TMP_Text? _videoCurrentTimeTMPText;

        private float _defaultAudioVolume;
        private Vector2 _normalScreenRectTransValue;
        private Vector2 _fullScreenRectTransValue;

        private bool _isGetVideoInfo = false;
        private bool _isFullScreen = false;
        private bool _isMouseUp = true;

        private bool _isUpdatingProgress = false;
        private bool _isForcePlaying = false;

        private Action<bool>? OnPlayOrPauseCallBack;
        private Action<bool>? OnFullOrNormalScreenCallBack;
        private Action<bool>? OnMuteOrUnMuteCallBack;

        private EventTrigger? _videoProgressSliderEventTrigger;

        private int _tempHour, _tempMin;
        private float _time;
        private float _timeCount;
        private float _timeCurrent;

        public VideoPlayerController(VideoPlayerConfig config)
        {
            _videoRawImage = config.VideoRawImage;
            _videoRenderTexture = config.VideoRenderTexture;
            _videoObject = _videoRawImage.gameObject;

            _playOrPauseBtn = config.PlayOrPauseBtn;
            _fullScreenBtn = config.FullScreenBtn;
            _muteBtn = config.MuteBtn;

            _videoProgressSlider = config.VideoProgressSlider;
            _audioVolumeSlider = config.AudioVolumeSlider;

            _videoFullTimeTMPText = config.VideoFullTimeTMPText;
            _videoCurrentTimeTMPText = config.VideoCurrentTimeTMPText;

            _defaultAudioVolume = config.DefaultAudioVolume;
            _normalScreenRectTransValue = config.NormalScreenRectTransValue;
            _fullScreenRectTransValue = config.FullScreenRectTransValue;

            OnPlayOrPauseCallBack = config.OnPlayOrPauseCallBack;
            OnFullOrNormalScreenCallBack = config.OnFullOrNormalScreenCallBack;
            OnMuteOrUnMuteCallBack = config.OnMuteOrUnMuteCallBack;

            _videoRawImageRectTrans = _videoRawImage.GetComponent<RectTransform>();

            _audioSource = _videoObject.GetComponent<AudioSource>() ?? _videoObject.AddComponent<AudioSource>();
            _videoPlayer = _videoObject.GetComponent<VideoPlayer>() ?? _videoObject.AddComponent<VideoPlayer>();
            _videoPlayer.targetTexture = _videoRenderTexture;

            _videoPlayer.playOnAwake = false;
            _videoPlayer.isLooping = true;
            _audioSource.playOnAwake = false;
            _audioSource.volume = _defaultAudioVolume;

            AddEvent();
            ResetController();
        }





        private void ResetController()
        {
            _videoRenderTexture.Release();
            _isGetVideoInfo = false;
            _isFullScreen = false;
            _isMouseUp = true;
        }

        private void AddEvent()
        {
            //_playOrPauseBtn?.onClick.AddListener(OnPlayOrPauseBtnClicked);
            //_fullScreenBtn?.onClick.AddListener(OnFullScreenBtnClicked);
            //_muteBtn?.onClick.AddListener(OnMuteBtnClicked);
            //_audioVolumeSlider?.onValueChanged.AddListener(OnAudioVolumeSliderValueChanged);
            //if (_videoProgressSlider != null)
            //{
            //    _videoProgressSlider.onValueChanged.AddListener(OnVideoProgressSliderValueChanged);
            //    SetGameObjectDragBeginListener(_videoProgressSlider.gameObject, OnSliderDragBegin);
            //    SetGameObjectDragEndListener(_videoProgressSlider.gameObject, OnSliderDragEnd);
            //}
        }
    }
}
