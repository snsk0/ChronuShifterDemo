using Chronus.ChronusItem;
using Chronus.UI.InGame.Interact;
using UnityEngine;

namespace Chronus.OrbGimmick
{
    public class OrbSwitch : MonoBehaviour
    {
        /*判定*/

        // オーブが設置されているか否か
        public bool isOrbExist { get; private set; }
        private bool prevState;

        // 設置されているオーブ
        private GameObject itemObj;
        private IChronusItem chronusItem;

        // 開始時にオーブを生成するか否か
        [SerializeField] private bool generateOnAwake = false;
        [SerializeField] private GameObject orbPrefab;

        // 設置位置
        [SerializeField] private Transform orbTransform;

        // 設置時に再生するパーティクル
        [SerializeField] private ParticleSystem orbParticle;

        [SerializeField] private bool stopOrbDissolve = false;

        private void Awake()
        {
            isOrbExist = generateOnAwake;

            if (isOrbExist)
            {
                itemObj = GameObject.Instantiate(orbPrefab);
                itemObj.transform.position = orbTransform.position;
                SetOrb();
            }
            else
            {
                ClearOrb();
            }
        }

        private void FixedUpdate()
        {
            if(isOrbExist != prevState)
            {
                prevState = isOrbExist;

                if (isOrbExist)
                {
                    SetOrb();
                }
                else
                {
                    ClearOrb();
                }
            }

            isOrbExist = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == itemObj)
            {
                isOrbExist = true;
                return;
            }

            if(other.CompareTag(Tags.TagType.Item.ToString()))
            {
                InteractionItem orb = other.GetComponent<InteractionItem>();

                if(orb != null && orb.itemType == InteractionDatabase.ItemType.Orb)
                {
                    itemObj = other.gameObject;
                    isOrbExist = true;
                }
            }
        }

        private void SetOrb()
        {
            if(stopOrbDissolve)
            {
                chronusItem = itemObj.GetComponent<IChronusItem>();
                if(chronusItem != null)
                {
                    chronusItem.SetPermanence(true);
                }
            }
            orbParticle.Play();
        }

        private void ClearOrb()
        {
            itemObj = null;
            if (chronusItem != null)
            {
                chronusItem.SetPermanence(false);
                chronusItem = null;
            }
            orbParticle.Stop();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if(generateOnAwake)
                Gizmos.DrawSphere(orbTransform.position + Vector3.up * 0.4f, 0.4f);
        }
#endif
    }
}