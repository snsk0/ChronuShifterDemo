using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Chronus.Direction
{
    public static class TimelinePlayer
    {
        private readonly static GameObject _directorOwner;
        private readonly static List<PlayableDirector> _playableDirectors = new List<PlayableDirector>();

        static TimelinePlayer()
        {
            _directorOwner = new GameObject(nameof(TimelinePlayer));
            Object.DontDestroyOnLoad(_directorOwner);
        }

        public static void SetTimeline(PlayableDirector director)
        {
            _playableDirectors.Add(director);
        }

        public static async UniTask PlayTimelineAll(DirectorWrapMode mode)
        {
            Debug.Log("PlayTimelines");
            foreach(PlayableDirector director in _playableDirectors)
            {
                director.extrapolationMode = mode;
                director.Play();
            }

            await UniTask.WhenAll(_playableDirectors.Select(async director => await UniTask.WaitWhile(() => director.state == PlayState.Playing)));

            _playableDirectors.Clear();
            Debug.Log("FinishTimeline");
        }
    }
}
