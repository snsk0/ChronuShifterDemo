using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using UniRx;

namespace Chronus.LegacyChronuShift
{
    public class PostEffectProfileSwitcher : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;
        [SerializeField] Volume volume;
        [SerializeField] VolumeProfile pastVolumeProfile;
        [SerializeField] VolumeProfile currentVolumeProfile;

        void ChangeProfile(bool isPast)
        {
            if (isPast)
                volume.profile = pastVolumeProfile;
            else
                volume.profile = currentVolumeProfile;
        }

        void Start()
        {
            chronusManager.isPast.Subscribe(isPast => ChangeProfile(isPast)).AddTo(this);
        }
    }
}