using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Chronus.Scenes
{
    public class LoadingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private Image currentFillterImage;

        private const string LoadingString = "Now Loading";
        private const int UpdateTextIntervalFrame = 30;

        private const float clockAnimationTime = 0.5f; 
        private const float clockAnimationInterval = 0.5f; 

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();

            ViewLoadingAsync(token).Forget();
            PlayClockAnimationAsync(token).Forget();
        }

        /// <summary>
        /// 一定フレーム毎にテキストを更新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async UniTaskVoid ViewLoadingAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                loadingText.text = $"{LoadingString}";
                await UniTask.DelayFrame(UpdateTextIntervalFrame, cancellationToken: token);
                loadingText.text = $"{LoadingString}.";
                await UniTask.DelayFrame(UpdateTextIntervalFrame, cancellationToken: token);
                loadingText.text = $"{LoadingString}..";
                await UniTask.DelayFrame(UpdateTextIntervalFrame, cancellationToken: token);
                loadingText.text = $"{LoadingString}...";
                await UniTask.DelayFrame(UpdateTextIntervalFrame, cancellationToken: token);
            }
        }

        private async UniTaskVoid PlayClockAnimationAsync(CancellationToken token)
        {
            while (true)
            {
                float elaspedTime = 0f;

                while (elaspedTime <= clockAnimationTime)
                {
                    currentFillterImage.fillAmount = 1 - (elaspedTime / clockAnimationTime);

                    await UniTask.Yield(cancellationToken: token);
                    elaspedTime += Time.deltaTime;
                }

                currentFillterImage.fillAmount = 0;
                await UniTask.Delay(TimeSpan.FromSeconds(clockAnimationInterval), cancellationToken: token);

                elaspedTime = 0f;
                while (elaspedTime <= clockAnimationTime)
                {
                    currentFillterImage.fillAmount = elaspedTime / clockAnimationTime;

                    await UniTask.Yield(cancellationToken: token);
                    elaspedTime += Time.deltaTime;
                }

                currentFillterImage.fillAmount = 1f;
                await UniTask.Delay(TimeSpan.FromSeconds(clockAnimationInterval), cancellationToken: token);
            }
        }
    }
}

