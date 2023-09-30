using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Chronus
{
    public class ChronusObject : MonoBehaviour, IChronusObject
    {
        public ObjectChronusType chronusType{ get; set;}
        bool carried = false;

        Vector3 defaultPosition;
        Quaternion defaultRotation;

        Vector3 defaultScale;
        Vector3 carriedScale;

        Collider objectCollider;

        void Start()
        {
            transform.GetPositionAndRotation(out defaultPosition, out defaultRotation);
            objectCollider = gameObject.GetComponent<Collider>();
        }

        void Update()
        {
            if(transform.position.y < -20)
            {
                transform.SetPositionAndRotation(defaultPosition, defaultRotation);
            }
        }

        public void ToggleCarriedState()
        {
            carried = !carried;
            
            ToggleCollider();
        }

        public void ToggleCarriedState(bool carried)
        {
            this.carried = carried;

            ToggleCollider();
        }

        public bool GetCarriedState()
        {
            return carried;
        }

        void ToggleCollider()
        {
            if(carried)
            {
                DOVirtual.DelayedCall(0.3f, () => {objectCollider.enabled = false;}, false);
            }
            else
            {
                DOVirtual.DelayedCall(0.3f, () => {objectCollider.enabled = true;}, false);
            }
        }
    }
}