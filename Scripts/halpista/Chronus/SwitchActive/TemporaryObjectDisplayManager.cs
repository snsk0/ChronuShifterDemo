using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus
{
    public class TemporaryObjectDisplayManager : MonoBehaviour
    {
        [SerializeField] ObjectChronusType chronusType;
        bool isPastSideObject = false;

        GameObject temporaryObject;
        [SerializeField] ChronusObject chronusObjectComponent;
        bool carried;

        public void SwitchDisplay(bool isPast)
        {
            carried = chronusObjectComponent.GetCarriedState();

            if(!carried) temporaryObject.SetActive(isPast == isPastSideObject);
        }

        void Awake()
        {
            temporaryObject = chronusObjectComponent.gameObject;
            chronusObjectComponent.chronusType = this.chronusType;

            if(chronusType == ObjectChronusType.past) isPastSideObject = true;
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if(temporaryObject == null) return;

            if(chronusType == ObjectChronusType.past)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.cyan;
            }

            Gizmos.DrawWireCube(temporaryObject.transform.position, Vector3.one * 0.3f);
        }
        #endif
    }
}