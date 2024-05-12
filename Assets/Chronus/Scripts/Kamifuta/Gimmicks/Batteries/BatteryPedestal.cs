using Chronus.Tags;
using Chronus.UI.InGame.Interact;
using Chronus.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chronus.ChronusGimmick.Battery
{
    public class BatteryPedestal : MonoBehaviour
    {
        [SerializeField] private float fieldRadius = 1f;

        public bool IsSet { get; private set; }

        private void OnValidate()
        {
            var diameter = fieldRadius * 2f;
            var scale = new Vector3(diameter, transform.localScale.y, diameter);
            transform.localScale = scale;
        }

        private void FixedUpdate()
        {
            if (!IsSet)
                return;

            var hitObjects=Physics.SphereCastAll(transform.position, fieldRadius, Vector3.up, 1f);
            if (hitObjects.Length == 0)
                return;

            var existBattery = hitObjects.Where(x => x.collider.CompareTag(TagType.Item)).Any(x => x.collider.gameObject.GetComponent<InteractionItem>().itemType == InteractionDatabase.ItemType.Battery);
            if (!existBattery)
            {
                IsSet = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(TagType.Item))
            {
                return;
            }

            if (other.TryGetComponent<InteractionItem>(out var item))
            {
                if(item.itemType == InteractionDatabase.ItemType.Battery)
                {
                    IsSet = true;
                }
            }
        }
    }
}

