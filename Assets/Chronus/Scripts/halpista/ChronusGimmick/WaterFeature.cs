using Chronus.ChronusDecoration;
using Chronus.ChronuShift;
using Chronus.UI.InGame.Interact;
using DG.Tweening;
using UnityEngine;

namespace Chronus.Gimmick
{
    public class WaterFeature : MonoBehaviour, IChronusTarget
    {
        // ���ՃM�~�b�N

        // ���Ղ��ߋ��Ŗ�������Ă��邩
        public bool isInPastWatered { get; private set; }

        // ���Ղ������Ԏ��Ŗ�������Ă��邩
        public bool isWatered { get; private set; }
        private bool lastValue;

        // ���Ղ̐���
        [SerializeField] private GameObject waterSurface;
        private float standbyHeight = -0.03f;
        private float targetHeight = 0.05f;
        private float duration = 0.6f;

        // ����
        [SerializeField] private WaterPlants waterPlants;

        private Collider prevCollider;

        private void Awake()
        {
            SetWaterLevel(standbyHeight, 0f);
            isInPastWatered = false;
        }

        public void OnShift(ChronusState state)
        {
            // �����̕\���؂�ւ�
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
            // ���Ղ��ߋ��Ŗ�������Ă���Ƃ��A���݂̐��ʂ��X�V���Ȃ�
            if (ChronusStateManager.Instance.chronusState.Value == ChronusState.Current && isInPastWatered) return;

            // ��Ԃ��ς�����琅�ʂ�؂�ւ�
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
            // OnTriggerStay�֐�������̏�ԏ���������҂�
        }

        private void OnTriggerStay(Collider other)
        {
            // ���Ղ��ߋ��Ŗ�������Ă���Ƃ��A���݂̐��ʂ��X�V���Ȃ�
            if (ChronusStateManager.Instance.chronusState.Value == ChronusState.Current && isInPastWatered) return;

            // ���A�C�e���ł���Ώ�ԏ�������
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