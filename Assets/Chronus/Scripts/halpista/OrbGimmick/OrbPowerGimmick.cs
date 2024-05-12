using StageGimmick;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chronus.OrbGimmick
{
    public class OrbPowerGimmick : MonoBehaviour
    {
        // �M�~�b�N�N����Ԃ��X�C�b�`�ɂ���Đ؂�ւ���

        // �N�����
        private bool isMove;
        private bool prevState;

        // �N����Ԃ�؂�ւ���orbSwitch
        [SerializeField] private List<OrbSwitch> orbSwitches;

        // GimmickMotion����������I�u�W�F�N�g�i��̓I�ȃM�~�b�N�j
        [SerializeField] private GimmickMotion gimmickMotion;

        private void Awake()
        {
            if(gimmickMotion == null)
            {
                Debug.Log("GimmickMotion is not registered.");
            }

            if (CheckSwitch())
            {
                gimmickMotion.OpenMotion();
            }
            else
            {
                gimmickMotion.CloseMotion();
            }
        }

        private void Update()
        {
            prevState = isMove;
            isMove = CheckSwitch();
            
            if(isMove != prevState)
            {
                if(isMove)
                {
                    gimmickMotion.OpenMotion();
                }
                else
                    gimmickMotion.CloseMotion();
            }
        }

        private bool CheckSwitch()
        {
            return orbSwitches.All(orbSwitch => orbSwitch.isOrbExist == true);
        }    }
}