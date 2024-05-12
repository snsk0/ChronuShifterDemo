using UnityEngine;

namespace Chronus.UI.InGame.Interact {
    public class InteractionItemGimmick : MonoBehaviour {
        [field: SerializeField] public bool isCheck { private set; get; } = false;
        [field: SerializeField] public InteractionDatabase.ItemType itemType { private set; get; }
        [field: SerializeField] public InteractionDatabase.InteractionType interactionType { private set; get; }
    }
}