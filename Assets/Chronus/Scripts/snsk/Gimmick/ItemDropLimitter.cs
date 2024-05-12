using UnityEngine;
using UnityView.Player.Utils;
using Chronus.UI.InGame.Interact;

namespace Chronus.ChronusGimmick
{
    public class ItemDropLimitter : MonoBehaviour, IItemDropOffLimitter
    {
        [SerializeField] private Transform _dropOffTransform;
        [SerializeField] private InteractionDatabase.ItemType _droppableItemType;

        public bool TryGetDropOffPosition(ItemTemp item, out Vector3 position)
        {
            GameObject itemGameObject = item.GameObject;

            if(itemGameObject.GetComponent<InteractionItem>().itemType == _droppableItemType)
            {
                position = _dropOffTransform.position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }
    }
}
