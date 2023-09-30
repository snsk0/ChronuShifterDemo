using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Chronus;

public class WaterCapsule : MonoBehaviour
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
        if(chronusObject != null)
        {
            if(chronusObject.GetCarriedState() != lastValue)
            {
                lastValue = chronusObject.GetCarriedState();
                waterFall.SetActive(!lastValue);
            }
        }
    }
}