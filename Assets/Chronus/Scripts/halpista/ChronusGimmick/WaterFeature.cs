using Chronus.ChronusDecoration;
using Chronus.ChronuShift;
using Chronus.UI.InGame.Interact;
using DG.Tweening;
using UnityEngine;

namespace Chronus.Gimmick
{
    public class WaterFeature : MonoBehaviour, IChronusTarget
    {
        // 水盤ギミック

        // 水盤が過去で満たされているか
        public bool isInPastWatered { get; private set; }

        // 水盤が現時間軸で満たされているか
        public bool isWatered { get; private set; }
        private bool lastValue;

        // 水盤の水面
        [SerializeField] private GameObject waterSurface;
        private float standbyHeight = -0.03f;
        private float targetHeight = 0.05f;
        private float duration = 0.6f;

        // 水草
        [SerializeField] private WaterPlants waterPlants;

        private Collider prevCollider;

        private void Awake()
        {
            SetWaterLevel(standbyHeight, 0f);
            isInPastWatered = false;
        }

        public void OnShift(ChronusState state)
        {
            // 水草の表示切り替え
            if(state == ChronusState.Forward && isInPastWatered)
            {
                if(waterPlants != null) waterPlants.SetPlantsActive(true);
            }
        }

        private void SetWaterLevel(float height, float duration)
        {
            waterSurface.transform.DOLocalMoveY(height, duration);
        }

        private void FixedUpdate()
        {
            // 水盤が過去で満たされているとき、現在の水位を更新しない
            if (ChronusStateManager.Instance.chronusState.Value == ChronusState.Current && isInPastWatered) return;

            // 状態が変わったら水位を切り替え
            if (lastValue != isWatered)
            {
                lastValue = isWatered;
                if(ChronusStateManager.Instance.chronusState.Value == ChronusState.Past)
                {
                    isInPastWatered = isWatered;
                }

                if(isWatered)
                {
                    SetWaterLevel(targetHeight, duration);
                }
                else
                {
                    SetWaterLevel(standbyHeight, duration);
                }

            }

            isWatered = false;
            // OnTriggerStay関数内からの状態書き換えを待つ
        }

        private void OnTriggerStay(Collider other)
        {
            // 水盤が過去で満たされているとき、現在の水位を更新しない
            if (ChronusStateManager.Instance.chronusState.Value == ChronusState.Current && isInPastWatered) return;

            // 水アイテムであれば状態書き換え
            if(other == prevCollider)
            {
                isWatered = true;
            }
            else if(other.CompareTag(Tags.TagType.Item.ToString()))
            {
                if(other.GetComponent<InteractionItem>().itemType == InteractionDatabase.ItemType.Water)
                {
                    prevCollider = other;
                    isWatered = true;
                }
                
            }
        }
    }
}