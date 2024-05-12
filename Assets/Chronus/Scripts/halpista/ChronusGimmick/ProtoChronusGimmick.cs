using Chronus.ChronuShift;
using Chronus.Direction;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace Chronus.Gimmick
{
    [RequireComponent(typeof(PlayableDirector))]
    public class ProtoChronusGimmick : MonoBehaviour, IChronusTarget
    {
        private PlayableDirector director;

        [SerializeField] private TimelineAsset forwardTimeline;
        [SerializeField] private TimelineAsset backwardTimeline;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        public void OnShift(ChronusState state)
        {
            if (state == ChronusState.Past)
            {

            }
            else if (state == ChronusState.Forward)
            {
                director.playableAsset = forwardTimeline;
                TimelinePlayer.SetTimeline(director);
            }
            else if (state == ChronusState.Current)
            {

            }
            else
            {
                director.playableAsset = backwardTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }
    }
}