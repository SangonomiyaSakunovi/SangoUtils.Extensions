using System;

namespace SangoUtils.Extensions_Unity.Anim
{
    public class SangoUIAnimationPack
    {
        public string Id { get; private set; }
        public bool IsPlaying { get; set; }
        public string[] Commands { get; set; }
        public SangoUIBaseAnimation SangoUIAnimation { get; set; }
        public Action OnAnimationPlayedCompleted { get; set; }
        public Action OnAnimationPlayCanceled { get; set; }

        public SangoUIAnimationPack(string id, SangoUIBaseAnimation sangoUIAnimation, Action completeAnimatorCallBack, Action cancelAnimatorCallBack)
        {
            Id = id;
            IsPlaying = false;
            SangoUIAnimation = sangoUIAnimation;
            OnAnimationPlayedCompleted = completeAnimatorCallBack;
            OnAnimationPlayCanceled = cancelAnimatorCallBack;
        }
    }
}
