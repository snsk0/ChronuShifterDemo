using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Chronus;

public class SkyboxSwitcher : MonoBehaviour
{
    [SerializeField] ChronusManager chronusManager;
    [SerializeField] Material pastSky;
    [SerializeField] Material currentSky;

    void ChangeSkybox(bool isPast)
    {
        if(isPast)
            RenderSettings.skybox = pastSky;
        else
            RenderSettings.skybox = currentSky;
    }

    void Start()
    {
        chronusManager.isPast.Subscribe(isPast => ChangeSkybox(isPast)).AddTo(this);
    }
}
