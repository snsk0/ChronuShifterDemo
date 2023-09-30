using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Chronus
{
    public class StageActiveManager : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;
        [SerializeField] ObjectChronusType chronusType;
        [SerializeField] GameObject stageObject;

        bool isPastSideObject = false;

        public void SwitchActive(bool isPast)
        {
            stageObject.SetActive(isPast == isPastSideObject);
        }

        void Start()
        {
            if(chronusType == ObjectChronusType.past) isPastSideObject = true;

            chronusManager.isPast.Subscribe(isPast => SwitchActive(isPast)).AddTo(this);
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if(stageObject == null) return;

            if(chronusType == ObjectChronusType.past)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.cyan;
            }

            Gizmos.DrawCube(stageObject.transform.position, Vector3.one * 0.3f);
        }
        #endif
    }
}