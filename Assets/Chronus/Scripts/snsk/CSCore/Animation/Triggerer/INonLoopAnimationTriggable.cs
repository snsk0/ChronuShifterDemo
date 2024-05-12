namespace Animation.Triggerer
{
    public interface INonLoopAnimationTriggable<T> : IAnimationTriggable<T>
    {
        //I—¹’Ê’m‚ğó‚¯æ‚é
        public void OnEndAnimation();
    }
}
