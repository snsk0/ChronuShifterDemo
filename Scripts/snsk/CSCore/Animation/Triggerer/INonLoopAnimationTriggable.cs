namespace Animation.Triggerer
{
    public interface INonLoopAnimationTriggable<T> : IAnimationTriggable<T>
    {
        //終了通知を受け取る
        public void OnEndAnimation();
    }
}
