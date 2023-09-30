using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    public class PossibilityUseSwitchGimmick : GimmickMotion {
        //�ŏ����瓮���Ă��邩
        [SerializeField, Header("�ŏ����瓮����")] protected bool isFromBeginning = false;

        protected bool isMoving = false;
        protected virtual bool IsNotFromBeginning() { return false; }//�ŏ�����R���[�`�����N������킯�ł͂Ȃ�

        void Start() {
            //�Ή�����X�C�b�`���Ȃ���΍ŏ�������s����悤�ɂ��Ă����i�Ƃ肠�����j
            if (GetComponent<SwitchDoor.SwitchDoor>() == null) { isFromBeginning = true; }
            if (isFromBeginning && !IsNotFromBeginning()) { isMoving = true; StartCoroutine(MoveMotion()); }
            VirtualStart();
        }

        protected virtual void VirtualStart() { }

        public override void OpenMotion() {
            isMoving = true;
            StartCoroutine(MoveMotion());
        }

        public override void CloseMotion() {
            isMoving = false;
        }

        protected virtual IEnumerator MoveMotion() {
            yield break;
        }
    }
}