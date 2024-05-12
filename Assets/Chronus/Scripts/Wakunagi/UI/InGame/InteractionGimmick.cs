using UnityEngine;

namespace Chronus.UI.InGame.Interact {
    public class InteractionGimmick : MonoBehaviour {
        [field: SerializeField] public InteractionDatabase.GimmickType itemType { private set; get; }
        [field: SerializeField] public InteractionDatabase.InteractionType interactionType { private set; get; }
    }
}