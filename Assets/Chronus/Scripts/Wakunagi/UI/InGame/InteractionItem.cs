using UnityEngine;

namespace Chronus.UI.InGame.Interact {
    public class InteractionItem : MonoBehaviour {
        [field: SerializeField] public InteractionDatabase.ItemType itemType { private set; get; }
        [field:SerializeField] public InteractionDatabase.InteractionType interactionType { private set; get; }
    }
}