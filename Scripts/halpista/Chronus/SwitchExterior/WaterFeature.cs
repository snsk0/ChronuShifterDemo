using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using Chronus;

public class WaterFeature : MonoBehaviour
{
    [SerializeField] ChronusManager chronusManager;

    [SerializeField] GameObject waterSurface;
    float minHeight = 1.2f;
    float maxHeight = 1.7f;

    WaterCapsule hitWaterCapsule;
    public bool isWatered;

    void ChangeWaterLevel()
    {
        if(isWatered)
        {
            waterSurface.transform.DOLocalMoveY(maxHeight, 0.6f);
        }
        else
        {
            waterSurface.transform.DOLocalMoveY(minHeight, 0.6f);
        }
    }

    bool lastValue;

    void FixedUpdate()
    {
        if(lastValue != isWatered)
        {
            lastValue = isWatered;
            ChangeWaterLevel();
        }
        
        isWatered = false;
    }

    Collider lastCollider;

    void OnTriggerStay(Collider other)
    {
        if(other != lastCollider)
        {
            lastCollider = other;
            hitWaterCapsule = other.gameObject.GetComponent<WaterCapsule>();
            if(hitWaterCapsule == null) return;
            
            isWatered = true;
        }
    }
}