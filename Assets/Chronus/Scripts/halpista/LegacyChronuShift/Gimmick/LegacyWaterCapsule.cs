using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Chronus.LegacyChronuShift
{
    public class LegacyWaterCapsule : MonoBehaviour
    {
        ChronusObject chronusObject;
        [SerializeField] GameObject waterFall;

        bool lastValue = false;

        void Start()
        {
            chronusObject = gameObject.GetComponent<ChronusObject>();
        }

        void Update()
        {
            if (chronusObject != null)
            {
                if (chronusObject.GetCarriedState() != lastValue)
                {
                    lastValue = chronusObject.GetCarriedState();
                    waterFall.SetActive(!lastValue);
                }
            }
        }
    }
}