using UnityEngine;

using UnityEditor;

namespace Chronus.LegacyChronuShift
{
    public class AlwaysObjectDisplayManager : MonoBehaviour
    {
        GameObject pastObject;
        [SerializeField] ChronusObject pastObjectComponent;
        bool pastObjectCarried = false;

        GameObject currentObject;
        [SerializeField] ChronusObject currentObjectComponent;
        bool currentObjectCarried = false;

        [SerializeField] int pairState = 0;

        [Tooltip("Make this current object un-carriable")][SerializeField] bool isFixed = false;

        public void SwitchDisplay(bool isPast)
        {
            // 持ち運び状態を確認
            pastObjectCarried = pastObjectComponent.GetCarriedState();

            if(!isFixed)
            {
                currentObjectCarried = currentObjectComponent.GetCarriedState();
            }

            if(pastObjectCarried)
            {
                // ペアの状態を遷移
                if(pairState == -1) pairState = 0;
                else if(pairState == 0) pairState = -1;
                else if(pairState == 1) pairState = 2;
                else if(pairState == 2) pairState = 1;
            }
            else if(currentObjectCarried)
            {
                // ペアの状態を遷移
                if(pairState == 0) pairState = 1;
                else if(pairState == 1) pairState = 0;
            }
            else if(!isPast && pairState == 0)
            {
                // 過去オブジェクトの位置を現在オブジェクトへ反映
                currentObject.transform.position = pastObject.transform.position;
                currentObject.transform.rotation = pastObject.transform.rotation;
            }

            if(isPast)
            {
                switch(pairState)
                {
                    case -1:
                        pastObject.SetActive(false);
                        currentObject.SetActive(false);
                        break;
                    case 0:
                        pastObject.SetActive(true);
                        currentObject.SetActive(false);
                        break;
                    case 1:
                        pastObject.SetActive(true);
                        currentObject.SetActive(true);
                        break;
                    case 2:
                        pastObject.SetActive(false);
                        currentObject.SetActive(false);
                        break;
                    default:
                        break;
                }
            } 
            else
            {
                switch(pairState)
                {
                    case -1:
                        pastObject.SetActive(true);
                        currentObject.SetActive(false);
                        break;
                    case 0:
                        pastObject.SetActive(false);
                        currentObject.SetActive(true);
                        break;
                    case 1:
                        pastObject.SetActive(false);
                        currentObject.SetActive(false);
                        break;
                    case 2:
                        pastObject.SetActive(true);
                        currentObject.SetActive(false);
                        break;
                    default:
                        break;
                }
            }     
        }

        void Awake()
        {
            pastObject = pastObjectComponent.gameObject;
            pastObjectComponent.chronusType = ObjectChronusType.past;

            if(!isFixed)
            {
                currentObject = currentObjectComponent.gameObject;
                currentObjectComponent.chronusType = ObjectChronusType.current;
            }

            pastObject.SetActive(true);
            currentObject.SetActive(false);
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if(pastObject == null || currentObject == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pastObject.transform.position, currentObject.transform.position);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(pastObject.transform.position, Vector3.one * 0.3f);
            Gizmos.color = Color.cyan;
           
            if(isFixed)
            {
                Gizmos.DrawCube(currentObject.transform.position, Vector3.one * 0.3f);    
            }
            else
            {
                Gizmos.DrawWireCube(currentObject.transform.position, Vector3.one * 0.3f);
            }

            Handles.Label(pastObject.transform.position, "State: " + pairState);
            Handles.Label(currentObject.transform.position, "State: " + pairState);
        }
        #endif
    }
}