using UnityEngine;
using UnityView.Player.Utils;
using Chronus.ChronusGimmick;

namespace Chronus.PlayerEx
{
    public class GimmickController : MonoBehaviour, IChronusGimmickController
    {
        private IInteractableGimmick _interactableGimmick;

        public bool IsInteracting => _interactableGimmick.IsInteracting;

        private void Awake()
        {
            _interactableGimmick = GetComponent<IInteractableGimmick>();
        }

        public void OnInteract(MonoBehaviour owner)
        {
            _interactableGimmick.Interact(owner);
        }
    }
}
