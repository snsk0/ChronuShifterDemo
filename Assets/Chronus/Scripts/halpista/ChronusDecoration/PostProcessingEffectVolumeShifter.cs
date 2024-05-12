using Chronus.ChronuShift;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Chronus.ChronusDecoration
{
    public class PostProcessingEffectVolumeShifter : MonoBehaviour, IChronusTarget
    {
        [SerializeField] private Volume volume;
        private ChromaticAberration chromaticAberration;
        private Vignette vignette;

        private float chromaticAberrationIntencity = 1f;
        private float vignetteIntencity = 0.3f;
        private float shiftDuration;

        CancellationToken token;

        private void Awake()
        {
            try
            {
                volume.profile.TryGet(out chromaticAberration);
                volume.profile.TryGet(out vignette);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }

            token = this.GetCancellationTokenOnDestroy();

            shiftDuration = ChronusStateManager.Instance.shiftDuration;
        }

        public void OnShift(ChronusState state)
        {
            if (state == ChronusState.Past)
            {
                chromaticAberration.active = false;
                vignette.intensity.value = vignetteIntencity;
            }       
            else if (state == ChronusState.Forward)
            {
                chromaticAberration.active = true;
                EaseChromaticAberrationIntensityAsync(0, chromaticAberrationIntencity, shiftDuration, token).Forget();

                EaseVignetteIntensityAsync(vignetteIntencity, 0f, shiftDuration, token).Forget();
            }
            else if (state == ChronusState.Current)
            {
                chromaticAberration.active = false;
                vignette.active = false;
            }
            else
            {
                chromaticAberration.active = true;
                EaseChromaticAberrationIntensityAsync(0, chromaticAberrationIntencity, shiftDuration, token).Forget();

                vignette.active = true;
                EaseVignetteIntensityAsync(0f, vignetteIntencity, shiftDuration, token).Forget();
            }
        }

        private async UniTask EaseVignetteIntensityAsync(float startIntensity, float goalIntensity, float duration, CancellationToken token)
        {
            float time = 0;

            while(time < duration)
            {
                time += Time.deltaTime;
                vignette.intensity.value = startIntensity + ((goalIntensity - startIntensity) * EaseOutSine(time / duration));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            vignette.intensity.value = goalIntensity;
        }

        private async UniTask EaseChromaticAberrationIntensityAsync(float startIntensity, float maxIntensity, float duration, CancellationToken token)
        {
            float time = 0;
            duration = duration / 2f;

            while (time < duration)
            {
                time += Time.deltaTime;
                chromaticAberration.intensity.value = startIntensity + ((maxIntensity - startIntensity) * EaseOutSine(time / duration));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            time = 0;

            while(time < duration)
            {
                time += Time.deltaTime;
                chromaticAberration.intensity.value = maxIntensity + ((startIntensity - maxIntensity) * EaseOutSine(time / duration));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            chromaticAberration.intensity.value = startIntensity;
        }

        private float EaseOutSine(float x)
        {
            return (float)Math.Sin((x * Math.PI) / 2);
        }
    }
}