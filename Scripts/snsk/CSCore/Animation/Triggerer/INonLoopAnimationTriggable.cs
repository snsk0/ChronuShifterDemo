namespace Animation.Triggerer
{
    public interface INonLoopAnimationTriggable<T> : IAnimationTriggable<T>
    {
        //�I���ʒm���󂯎��
        public void OnEndAnimation();
    }
}
