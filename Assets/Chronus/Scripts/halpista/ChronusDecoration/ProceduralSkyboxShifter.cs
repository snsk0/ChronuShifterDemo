using Chronus.ChronuShift;
using Chronus.Direction;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Chronus.ChronusDecoration
{
    [RequireComponent(typeof(PlayableDirector))]
    public class ProceduralSkyboxShifter : MonoBehaviour, IChronusTarget
    {
        PlayableDirector director;

        [SerializeField] private TimelineAsset forwardTimeline;
        [SerializeField] private TimelineAsset backwardTimeline;

        [SerializeField] private bool changeCloudSpeed;
        private Material skybox;
        private string cloudSpeedPropertyName = "_CloudSpeed";
        private float shiftDuration;

        CancellationToken token;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();

            skybox = new Material(RenderSettings.skybox);
            RenderSettings.skybox = skybox;

            token = this.GetCancellationTokenOnDestroy();

            shiftDuration = ChronusStateManager.Instance.shiftDuration;
        }

        public void OnShift(ChronusState state)
        {
            if (state == ChronusState.Past)
            {
                EaseCloudSpeed(skybox.GetFloat(cloudSpeedPropertyName), 0.2f, shiftDuration / 2, token).Forget();
            }
            else if (state == ChronusState.Forward)
            {
                EaseCloudSpeed(skybox.GetFloat(cloudSpeedPropertyName), 300.0f, shiftDuration / 2, token).Forget();
                director.playableAsset = forwardTimeline;
                TimelinePlayer.SetTimeline(director);
            }
            else if (state == ChronusState.Current)
            {
                EaseCloudSpeed(skybox.GetFloat(cloudSpeedPropertyName), 0.16f, shiftDuration / 2, token).Forget();
            }
            else
            {
                EaseCloudSpeed(skybox.GetFloat(cloudSpeedPropertyName), -300.0f, shiftDuration / 2, token).Forget();
                director.playableAsset = backwardTimeline;
                TimelinePlayer.SetTimeline(director);
            }
        }

        private void SetCloudSpeed(float speed)
        {
            try
            {
                skybox.SetFloat(cloudSpeedPropertyName, speed);
            }
            catch (Exception e)
            { 
                Debug.LogException(e);
            }
        }

        private async UniTaskVoid EaseCloudSpeed(float startSpeed, float goalSpeed, float duration, CancellationToken token)
        {
            if (!changeCloudSpeed) return;

            float time = 0;
            while (time < duration)
            {
                time += Time.deltaTime;
                SetCloudSpeed(startSpeed + ((goalSpeed - startSpeed) * EaseOutSine(time / duration)));
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            SetCloudSpeed (goalSpeed);
        }

        private float EaseOutSine(float x)
        {
            return (float)Math.Sin((x * Math.PI) / 2);
        }
    }
}