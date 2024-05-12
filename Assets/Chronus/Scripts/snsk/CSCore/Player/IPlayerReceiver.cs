namespace Player
{
    public interface IPlayerEventReceiver
    {
        public bool SendEvent(PlayerEvent triggerEvent);
    }
}
