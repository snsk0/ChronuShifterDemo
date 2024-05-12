using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Chronus.ChronuShift.Mock
{
    public class ChronusSwitchMock : MonoBehaviour
    {
        [SerializeField] TextMeshPro keyText;

        private void Awake()
        {
            if(keyText != null) keyText.alpha = 0f;
        }

        private void OnTriggerStay(Collider other)
        {
            if (keyText != null) keyText.DOFade(1f, 0.5f);

            if(other.gameObject.CompareTag("Player"))
            {
                if(Input.GetKeyDown(KeyCode.Q))
                {
                    ChronusStateManager.Instance.ChronuShift();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (keyText != null) keyText.DOFade(0f, 0.5f);
        }
    }
}