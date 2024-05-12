using Chronus.ChronuShift;
using Chronus.Direction;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Chronus.Gimmick
{
    public class GrowingTree : MonoBehaviour, IChronusTarget
    {
        private bool isGrowing;

        [SerializeField] private WaterFeature waterFeature;

        [SerializeField] private GameObject pastTree;
        [SerializeField] private GameObject currentGrowingTree;
        [SerializeField] private GameObject currentDeadTree;

        [SerializeField] private PlayableDirector director;

        [SerializeField] private TimelineAsset forwardGrowingTimeline;
        [SerializeField] private TimelineAsset backwardGrowingTimeline;
        [SerializeField] private TimelineAsset forwardDyingTimeline;
        [SerializeField] private TimelineAsset backwardDyingTimeline;

        public void OnShift(ChronusState state)
        {
            isGrowing = waterFeature.isInPastWatered;

            if (state == ChronusState.Past)
            {
                pastTree.SetActive(true);
                currentGrowingTree.SetActive(false);
                currentDeadTree.SetActive(false);
            }
            else if (state == ChronusState.Forward)
            {
                if (isGrowing) GrowTree(true);
                else KillTree(true);
            }
            else if (state == ChronusState.Current)
            {
                pastTree.SetActive(false);
                currentGrowingTree.SetActive(isGrowing);
                currentDeadTree.SetActive(!isGrowing);
            }
            else
            {
                if(isGrowing) GrowTree(false);
                else KillTree(false);
            }
        }

        private void GrowTree(bool isForward)
        {
            if(isForward)
            {
                director.playableAsset = forwardGrowingTimeline;
                TimelinePlayer.SetTimeline(director);
            }
            else
            {
                director.playableAsset = backwardGrowingTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }

        private void KillTree(bool isForward)
        {
            if(isForward)
            {
                director.playableAsset = forwardDyingTimeline;
                TimelinePlayer.SetTimeline(director);
            }
            else
            {
                director.playableAsset = backwardDyingTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }
    }
}