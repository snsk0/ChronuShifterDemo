using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace AffectPlayerGimmick {
        public class AffectPlayerFloorGimmick : GimmickMotion {
            //�ŏ����瓮���Ă��邩
            [SerializeField, Header("�ŏ����瓮����")] protected bool isFromBeginning = false;

            protected bool isMoving = false;

            void Start() {
                //�Ή�����X�C�b�`���Ȃ���΍ŏ�������s����悤�ɂ��Ă����i�Ƃ肠�����j
                if (GetComponent<SwitchDoor.SwitchDoor>() == null) { isFromBeginning = true; }
                if (isFromBeginning) { isMoving = true; }
                VirtualStart();
            }

            protected virtual void VirtualStart() { }

            public override void OpenMotion() {
                isMoving = true;
            }

            public override void CloseMotion() {
                isMoving = false;
            }

            private void OnTriggerEnter(Collider other) {
                if (isMoving) VirtualOnTriggerEnter(other);
            }

            protected virtual void VirtualOnTriggerEnter(Collider other) {

            }
            private void OnTriggerExit(Collider other) {
                if (isMoving) VirtualOnTriggerExit(other);
            }

            protected virtual void VirtualOnTriggerExit(Collider other) {

            }
        }
    }
}