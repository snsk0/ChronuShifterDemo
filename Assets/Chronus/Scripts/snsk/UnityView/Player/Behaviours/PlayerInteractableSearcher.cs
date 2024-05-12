using UnityEngine;
using Player.Behaviours;
using Player.Structure;
using UnityView.Player.Utils;
using Player;

namespace UnityView.Player.Behaviours
{
    public class PlayerInteractableSearcher : MonoBehaviour, IPlayerInteractableSearcher, IPlayerItemDropPositionSeracher
    {
        [SerializeField] private float _searchObjectRange;
        [SerializeField] private float _searchDropPositionRange;

        public IPlayerInteractableObject SearchObject()
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position, transform.forward * _searchObjectRange, Color.red, 1f);
#endif
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _searchObjectRange, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                IChronusItemController itemController = hit.collider.GetComponent<IChronusItemController>();
                if (itemController != null)
                {
                    return new ItemTemp(hit.collider.gameObject, itemController);
                }

                IChronusGimmickController gimmickController = hit.collider.GetComponent<IChronusGimmickController>();
                if(gimmickController != null)
                {
                    return new GimmickTemp(hit.collider.gameObject, gimmickController);
                }
            }
            return null;
        }

        public bool TryGetDropPosition(IPlayerItemObject item, out Position position)
        {
#if UNITY_EDITOR
            Debug.DrawRay(transform.position + transform.forward, -transform.up * _searchDropPositionRange, Color.red, 1f);
#endif
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _searchObjectRange, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                position = new Position(0, 0, 0);
                return false;
            }

            if (Physics.Raycast(transform.position + transform.forward * _searchDropPositionRange, - transform.up, out hit, _searchDropPositionRange, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                IItemDropOffLimitter limmiter = hit.collider.gameObject.GetComponent<IItemDropOffLimitter>();

                if(limmiter != null)
                {
                    if (limmiter.TryGetDropOffPosition(item as ItemTemp, out Vector3 vectorPosition))
                    {
                        position = new Position(vectorPosition.x, vectorPosition.y, vectorPosition.z);
                        return true;
                    }
                }
                else
                {
                    position = new Position(hit.point.x, hit.point.y, hit.point.z);
                    return true;
                }
            }

            position = new Position(0, 0, 0);
            return false;
        }
    }
}
