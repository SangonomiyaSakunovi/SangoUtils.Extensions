using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SangoUtils.Behaviours_Unity.UGUIOPs
{
    /// <summary>
    /// ֡����
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class UGUISpriteFrameAnimation : MonoBehaviour
    {
        private Image? ImageSource;
        private int mCurFrame = 0;
        private float mDelta = 0;
        public float FPS = 5;
        public List<Sprite>? SpriteFrames;
        public bool IsPlaying = false;
        public bool Foward = true;
        public bool AutoPlay = false;
        public bool Loop = false;
        public int FrameCount
        {
            get
            {
                if (SpriteFrames != null)
                {
                    return SpriteFrames.Count;
                }
                return 0;
            }
        }

        void Awake()
        {
            ImageSource = GetComponent<Image>();
        }

        void Start()
        {
            if (AutoPlay)
            {
                Play();
            }
            else
            {
                IsPlaying = false;
            }
        }

        private void SetSprite(int idx)
        {
            if (ImageSource != null && SpriteFrames != null)
            {
                ImageSource.sprite = SpriteFrames[idx];
                //ImageSource.SetNativeSize();
            }
        }

        public void Play()
        {
            IsPlaying = true;
            Foward = true;
        }

        public void PlayReverse()
        {
            IsPlaying = true;
            Foward = false;
        }

        void Update()
        {
            if (!IsPlaying || 0 == FrameCount)
            {
                return;
            }
            mDelta += Time.deltaTime;
            if (mDelta > 1 / FPS)
            {
                mDelta = 0;
                if (Foward)
                {
                    mCurFrame++;
                }
                else
                {
                    mCurFrame--;
                }
                if (mCurFrame >= FrameCount)
                {
                    if (Loop)
                    {
                        mCurFrame = 0;
                    }
                    else
                    {
                        IsPlaying = false;
                        return;
                    }
                }
                else if (mCurFrame < 0)
                {
                    if (Loop)
                    {
                        mCurFrame = FrameCount - 1;
                    }
                    else
                    {
                        IsPlaying = false;
                        return;
                    }
                }
                SetSprite(mCurFrame);
            }
        }

        public void Pause()
        {
            IsPlaying = false;
        }

        public void Resume()
        {
            if (!IsPlaying)
            {
                IsPlaying = true;
            }
        }

        public void Stop()
        {
            mCurFrame = 0;
            SetSprite(mCurFrame);
            IsPlaying = false;
        }

        public void Rewind()
        {
            mCurFrame = 0;
            SetSprite(mCurFrame);
            Play();
        }
    }
}