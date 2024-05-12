using Chronus.ChronusGimmick.Weather;
using Chronus.ChronuShift;
using Chronus.Direction;
using Kamifuta.Attribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Chronus.ChronusGimmick.Water
{
    public class WaterSurface : MonoBehaviour, IChronusTarget
    {
        [SerializeField] private bool viewGizmos;
        [SerializeField] private WeatherChanger weatherChanger;

        [SerializeField] private float maxHeight;
        [SerializeField, NonEditable] private float minHeight;

        [SerializeField] private TimelineAsset riseTimeline;
        [SerializeField] private TimelineAsset rewindTimeline;
        [SerializeField] private TimelineAsset iceTimeline;
        [SerializeField] private TimelineAsset meltTimeline;
        [SerializeField] private PlayableDirector playableDirector;

        [SerializeField] private Collider[] defaultColliders;
        [SerializeField] private Collider[] rainyColliders;
        [SerializeField] private Collider[] snowyColliders;

        private Vector3 rosePosition => new Vector3(transform.position.x, maxHeight, transform.position.z);
        private Vector3 loweredPosition => new Vector3(transform.position.x, minHeight, transform.position.z);

        private bool IsRisable => weatherChanger.currentWeather == WeatherType.Rainy;
        private bool CanFreeze => weatherChanger.currentWeather == WeatherType.Snowy;

        private bool HadIced = false;
        private bool IsLow = false;

        private void Start()
        {
            switch (weatherChanger.currentWeather)
            {
                case WeatherType.Sunny:
                case WeatherType.Snowy:
                    IsLow = true;
                    transform.position = loweredPosition;
                    break;
                case WeatherType.Rainy:
                    IsLow = false;
                    transform.position = rosePosition;
                    break;
            }
        }

        private void OnValidate()
        {
            minHeight = transform.position.y;
            if (maxHeight < minHeight)
            {
                maxHeight = minHeight;
            }
        }

        private void OnDrawGizmos()
        {
            if (!viewGizmos)
                return;

            Gizmos.color = new Color(0, 1f, 0, 0.2f);
            var scale = new Vector3(transform.localScale.x * 10, 0.01f, transform.localScale.z * 10);
            Gizmos.DrawCube(rosePosition, scale);
        }

        public void OnShift(ChronusState state)
        {
            switch (state)
            {
                case ChronusState.Forward:
                    if (IsRisable)
                    {
                        SetEnableCollider(defaultColliders, false);
                        SetEnableCollider(rainyColliders, true);
                        //Rise()‚ÌŒÄ‚Ño‚µ
                        playableDirector.playableAsset = riseTimeline;
                    }
                    else if (CanFreeze)
                    {
                        SetEnableCollider(defaultColliders, false);
                        SetEnableCollider(snowyColliders, true);
                        HadIced = true;
                        playableDirector.playableAsset = iceTimeline;
                    }
                    else
                    {
                        if (IsLow)
                            break;

                        SetEnableCollider(defaultColliders, true);
                        SetEnableCollider(rainyColliders, false);
                        SetEnableCollider(snowyColliders, false);
                        //Lower()‚ÌŒÄ‚Ño‚µ
                        playableDirector.playableAsset = rewindTimeline;
                    }
                    TimelinePlayer.SetTimeline(playableDirector);
                    break;
                case ChronusState.Backward:
                    SetEnableCollider(defaultColliders, true);
                    SetEnableCollider(rainyColliders, false);
                    SetEnableCollider(snowyColliders, false);
                    if (HadIced)
                    {
                        HadIced = false;
                        playableDirector.playableAsset = meltTimeline;
                    }
                    else
                    {
                        if (IsLow)
                            break;

                        //Lower()‚ÌŒÄ‚Ño‚µ
                        playableDirector.playableAsset = rewindTimeline;
                    }

                    Debug.Log("dddd");
                    TimelinePlayer.SetTimeline(playableDirector);
                    break;
                case ChronusState.Past:
                    SetEnableCollider(defaultColliders, true);
                    SetEnableCollider(rainyColliders, false);
                    SetEnableCollider(snowyColliders, false);
                    break;
                case ChronusState.Current:
                    break;
                default:
                    break;
            }
        }

        public void Rise(float animationTime)
        {
            IsLow = false;
            StartCoroutine(ChangeHeightAnimationCoroutine(rosePosition, animationTime));
        }

        public void Rewind(float animationTime)
        {
            IsLow = true;
            StartCoroutine(ChangeHeightAnimationCoroutine(loweredPosition, animationTime));
        }

        private void SetEnableCollider(Collider[] colliders, bool value)
        {
            foreach(Collider collider in colliders)
            {
                collider.enabled = value;
            }
        }

        private IEnumerator ChangeHeightAnimationCoroutine(Vector3 endPosition, float animationTime)
        {
            float elaspedTime = 0f;
            Vector3 startPosition = transform.position;

            while (elaspedTime <= animationTime)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elaspedTime / animationTime);

                yield return null;
                elaspedTime += Time.deltaTime;
            }
        }
    }
}

