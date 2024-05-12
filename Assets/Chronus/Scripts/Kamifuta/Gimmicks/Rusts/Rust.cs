using Chronus.ChronuShift;
using Chronus.UI.InGame.Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronusGimmick.Rusts
{
    public class Rust : MonoBehaviour, IChronusTarget
    {
        [SerializeField] private Material material;

        private bool HadRusted;
        private bool IsInWater;

        public void OnShift(ChronusState state)
        {
            switch (state)
            {
                case ChronusState.Forward:
                    if (IsInWater)
                    {
                        BeRust();
                    }
                    break;
                case ChronusState.Backward:
                    break;
                default:
                    break;
            }
        }

        public void BeRust()
        {
            if (HadRusted)
                return;

            //TODO:�K�}�e���A���ɕύX
            HadRusted = true;
        }

        public void BeNonRust()
        {
            if (!HadRusted)
                return;

            //TODO:�ʏ�}�e���A���ɕύX
            HadRusted = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            //�߂��ɃM�~�b�N�����̃A�C�e�������邩�𔻒肷��B
            if (other.TryGetComponent<InteractionItem>(out var item))
            {
                IsInWater = item.itemType == InteractionDatabase.ItemType.Water;
            }
        }
    }
}

