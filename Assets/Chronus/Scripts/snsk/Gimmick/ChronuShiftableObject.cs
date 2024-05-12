using UnityEngine;
using UnityEngine.Playables;
using Chronus.ChronuShift;
using Chronus.Direction;

namespace Chronus.ChronusGimmick
{
    public class ChronuShiftableObject : MonoBehaviour, IChronusTarget, IInteractableGimmick
    {
        [SerializeField] private PlayableDirector _director;

        private bool _initialized = false;
        private bool _isInteractable = false;

        public bool IsInteractable => _isInteractable;
        public bool IsInteracting => _isInteractable;

        public void Interact<T>(T interactor) where T : MonoBehaviour
        {
            _isInteractable = ChronusStateManager.Instance.ChronuShift();
        }

        public void OnShift(ChronusState state)
        {
            if (!_initialized)
            {
                _initialized = true;
                return;
            }

            if (state == ChronusState.Current || state == ChronusState.Past)
            {
                _isInteractable = false;
            }
            else
            {
                TimelinePlayer.SetTimeline(_director);
            }
        }
    }
}
