using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Chronus.ChronusGimmick
{
    public class GimmickEventPlayer : MonoBehaviour
    {
        [SerializeField] private Image fadePanel;

        private CancellationToken token;

        private const float fadeTime = 1f;

        private void Start()
        {
            token = this.GetCancellationTokenOnDestroy();
        }

        public async UniTaskVoid FadeOutAsync(Action callback = null, bool toFadeIn=true)
        {
            await Fade(0f);
            callback?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            if (toFadeIn)
            {
                await Fade(1f);
            }
        }

        public async UniTaskVoid FadeInAsync(Action callback = null, bool toFadeOut=true)
        {
            await Fade(1f);
            callback?.Invoke();

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);

            if (toFadeOut)
            {
                await Fade(0f);
            }
        }

        private async UniTask Fade(float purposeAlpha)
        {
            var color = fadePanel.color;
            float alpha = fadePanel.color.a;
            float elaspedTime = 0f;

            while (true)
            {
                color.a = Mathf.Lerp(alpha, purposeAlpha, elaspedTime / fadeTime);
                fadePanel.color = color;

                if (elaspedTime >= fadeTime)
                    break;

                await UniTask.Yield(cancellationToken: token);
                elaspedTime += Time.deltaTime;
            }
        }
    }
}

