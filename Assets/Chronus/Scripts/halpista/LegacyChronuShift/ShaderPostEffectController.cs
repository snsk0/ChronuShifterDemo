using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using DG.Tweening;

namespace Chronus.LegacyChronuShift
{
    public class ShaderPostEffectController : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;

        [SerializeField] Material distortionPostEffect;

        void AnimateMaterial(bool isPast)
        {
            distortionPostEffect.SetFloat("_Distortion", 1f);

            DOTween.Sequence().
                Append(distortionPostEffect.DOFloat(0.04f, "_Power", 0.3f).SetEase(Ease.InSine)).
                Append(distortionPostEffect.DOFloat(0f, "_Power", 0.3f).SetEase(Ease.OutSine));

            distortionPostEffect.SetFloat("Distortion", 1f);
        }

        void Start()
        {
            chronusManager.isPast.Subscribe(isPast => AnimateMaterial(isPast));
        }
    }
}