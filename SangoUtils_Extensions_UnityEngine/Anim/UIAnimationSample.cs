using System;

namespace SangoUtils.Extensions_Unity.Anim
{
    internal class UIAnimationSample
    {
        private SangoUIAnimator _sangoUIAnimator = new SangoUIAnimator();

        public void OnInit()
        {
            _sangoUIAnimator.Init();
        }

        private void Update()
        {
            _sangoUIAnimator.UpdateAnimator();
        }

        public void OnDispose()
        {
            _sangoUIAnimator.Clear();
        }

        public void AddAnimation(string id, SangoUIBaseAnimation sangoUIAnimation, Action completeCallBack = null, Action cancelCallBack = null)
        {
            SangoUIAnimationPack pack = new SangoUIAnimationPack(id, sangoUIAnimation, completeCallBack, cancelCallBack);
            _sangoUIAnimator.AddAnimation(pack);
        }

        public void PlayAnimation(string id, params string[] commands)
        {
            _sangoUIAnimator.PlayAnimationImmediately(id, commands);
        }

        public void PlayAnimationAsync(string id, params string[] commands)
        {
            _sangoUIAnimator.PlayAnimation(id, commands);
        }
    }
}
