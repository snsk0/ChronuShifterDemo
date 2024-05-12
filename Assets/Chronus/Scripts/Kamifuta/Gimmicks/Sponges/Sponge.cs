using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kamifuta.Attribute;
using Chronus.ChronuShift;
using Chronus.UI.InGame.Interact;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Chronus.Direction;
using System.Linq;
using Chronus.Tags;
using Chronus.Utils;
using Chronus.ChronusGimmick.Weather;

namespace Chronus.ChronusGimmick.Sponges
{
    [ExecuteAlways]
    public class Sponge : MonoBehaviour, IChronusTarget
    {
        private enum SpongeState
        {
            Normal,
            Expanded,
            Shrinked,
        }

        [SerializeField] private bool viewGizmos;
        [SerializeField] private Vector3 maxSize;
        [SerializeField] private Vector3 midSize;
        [SerializeField, NonEditable] private Vector3 minSize;

        [SerializeField] private Vector3 pivot;

        [SerializeField] private TimelineAsset RewindTimeline;
        [SerializeField] private TimelineAsset ExpandTimeline;
        [SerializeField] private TimelineAsset ShrinkTimeline;

        [SerializeField] private PlayableDirector playableDirector;
        [SerializeField] private WeatherChanger weatherChanger;

        private Cube maxCube;
        private Cube midCube;
        private Cube minCube;

        private SpongeState currentState = SpongeState.Normal;

        private bool IsExpandable = false;
        private bool IsShrinkable = false;

        private class Cube
        {
            public Cube(Transform transform)
            {
                UpdateTopPoint(transform);
            }

            public Cube(Vector3 minTop, Vector3 maxTop)
            {
                this.minTop = minTop;
                this.maxTop = maxTop;
            }

            /// <summary>
            /// 左後下の頂点
            /// </summary>
            public Vector3 minTop { get; private set; }
            /// <summary>
            /// 右前上の頂点
            /// </summary>
            public Vector3 maxTop { get; private set; }

            public Vector3 Center => minTop + ((maxTop - minTop) / 2f);


            public void UpdateTopPoint(Transform transform)
            {
                minTop = transform.position + new Vector3(-transform.localScale.x / 2f, -transform.localScale.y / 2f, -transform.localScale.z / 2f);
                maxTop = transform.position + new Vector3(+transform.localScale.x / 2f, +transform.localScale.y / 2f, +transform.localScale.z / 2f);
            }
        }

        public void OnDrawGizmos()
        {
            if (!viewGizmos)
                return;

            if (!Application.isPlaying)
            {
                maxCube = CreateCube(maxSize);
                midCube = CreateCube(midSize);
            }

            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(maxCube.Center, maxSize);

            Gizmos.color = new Color(0, 0, 1, 0.3f);
            Gizmos.DrawCube(midCube.Center, midSize);
        }

        private Cube CreateCube(Vector3 size)
        {
            var cube = new Cube(transform);
            var pivotPoint = transform.position + new Vector3(pivot.x * transform.localScale.x, pivot.y * transform.localScale.y, pivot.z * transform.localScale.z);

            //新しい立方体の左後下の位置を計算
            var minVec = cube.minTop - pivotPoint;
            minVec.x = minVec.x * (size.x / transform.localScale.x);
            minVec.y = minVec.y * (size.y / transform.localScale.y);
            minVec.z = minVec.z * (size.z / transform.localScale.z);
            var minTop = pivotPoint + minVec;

            //新しい立方体の右前上の位置を計算
            var maxVec = cube.maxTop - pivotPoint;
            maxVec.x = maxVec.x * (size.x / transform.localScale.x);
            maxVec.y = maxVec.y * (size.y / transform.localScale.y);
            maxVec.z = maxVec.z * (size.z / transform.localScale.z);
            var maxTop = pivotPoint + maxVec;

            var resultCube = new Cube(minTop, maxTop);
            return resultCube;
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                maxCube = CreateCube(maxSize);
                midCube = CreateCube(midSize);
                minCube = CreateCube(minSize);

                transform.position = midCube.Center;
                transform.localScale = midSize;
            }
        }

        private void OnValidate()
        {
            //ピボットの値表示の更新
            if (pivot.x < -0.5f || 0.5f < pivot.x)
            {
                pivot.x = Mathf.Clamp(pivot.x, -0.5f, 0.5f);
            }

            if (pivot.y < -0.5f || 0.5f < pivot.y)
            {
                pivot.y = Mathf.Clamp(pivot.y, -0.5f, 0.5f);
            }

            if (pivot.z < -0.5f || 0.5f < pivot.z)
            {
                pivot.z = Mathf.Clamp(pivot.z, -0.5f, 0.5f);
            }

            minSize = transform.localScale;
        }

        private void FixedUpdate()
        {
            if (!IsExpandable && !IsShrinkable)
                return;

            var hitObjects = Physics.BoxCastAll(transform.position, new Vector3(transform.localScale.x * 0.75f, transform.localScale.y * 0.75f, transform.localScale.z * 0.75f), Vector3.up, Quaternion.identity, 1f);
            if (hitObjects.Length == 0)
                return;

            var itemObjects = hitObjects.Where(x => x.collider.CompareTag(TagType.Item)).Select(x => x.collider.gameObject);
            var existFire = itemObjects.Any(x => x.GetComponent<InteractionItem>().itemType == InteractionDatabase.ItemType.Fire);
            var existWater = itemObjects.Any(x => x.GetComponent<InteractionItem>().itemType == InteractionDatabase.ItemType.Water);

            IsShrinkable = existFire;
            IsExpandable = existWater;
        }

        public void OnShift(ChronusState state)
        {
            switch (state)
            {
                case ChronusState.Forward:
                    if (weatherChanger != null && weatherChanger.currentWeather == WeatherType.Rainy)
                    {
                        //Expand()の実行
                        playableDirector.playableAsset = ExpandTimeline;
                        TimelinePlayer.SetTimeline(playableDirector);
                        break;
                    }

                    if (IsExpandable)
                    {
                        //Expand()の実行
                        playableDirector.playableAsset = ExpandTimeline;
                        TimelinePlayer.SetTimeline(playableDirector);
                    }
                    else if (IsShrinkable)
                    {
                        //Shrink()の実行
                        playableDirector.playableAsset = ShrinkTimeline;
                        TimelinePlayer.SetTimeline(playableDirector);
                    }
                    break;
                case ChronusState.Backward:
                    if (currentState == SpongeState.Normal)
                        return;

                    //Rewind()の実行
                    playableDirector.playableAsset = RewindTimeline;
                    TimelinePlayer.SetTimeline(playableDirector);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 初期状態に戻す
        /// </summary>
        public void Rewind(float animationTime)
        {
            if (currentState == SpongeState.Normal)
                return;

            StartCoroutine(ChangeScaleAnimationCoroutine(midCube.Center, midSize, animationTime));
            currentState = SpongeState.Normal;
        }

        /// <summary>
        /// 膨張状態にする
        /// </summary>
        public void Expand(float animationTime)
        {
            if (currentState == SpongeState.Expanded)
                return;

            StartCoroutine(ChangeScaleAnimationCoroutine(maxCube.Center, maxSize, animationTime));
            currentState = SpongeState.Expanded;
        }

        /// <summary>
        /// 収縮状態にする
        /// </summary>
        public void Shrink(float animationTime)
        {
            if (currentState == SpongeState.Shrinked)
                return;

            StartCoroutine(ChangeScaleAnimationCoroutine(minCube.Center, minSize, animationTime));
            currentState = SpongeState.Shrinked;
        }

        /// <summary>
        /// 拡縮のアニメーション
        /// </summary>
        /// <param name="endPosition"></param>
        /// <param name="endScale"></param>
        /// <returns></returns>
        private IEnumerator ChangeScaleAnimationCoroutine(Vector3 endPosition, Vector3 endScale, float animationTime)
        {
            float elaspedTime = 0f;
            Vector3 startPosition = transform.position;
            Vector3 startScale = transform.localScale;

            while (elaspedTime < animationTime)
            {
                var rate = elaspedTime / animationTime;
                var position = Vector3.Lerp(startPosition, endPosition, rate);
                var scale = Vector3.Lerp(startScale, endScale, rate);

                transform.position = position;
                transform.localScale = scale;

                yield return null;
                elaspedTime += Time.deltaTime;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //近くにギミック成立のアイテムがあるかを判定する。
            if(other.TryGetComponent<InteractionItem>(out var item))
            {
                IsExpandable = item.itemType == InteractionDatabase.ItemType.Water;
                IsShrinkable = item.itemType == InteractionDatabase.ItemType.Fire;
            }
        }
    }
}


