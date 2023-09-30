using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace AffectPlayerGimmick {
        public class AffectPlayerFloorGimmick : GimmickMotion {
            //最初から動いているか
            [SerializeField, Header("最初から動くか")] protected bool isFromBeginning = false;

            protected bool isMoving = false;

            void Start() {
                //対応するスイッチがなければ最初から実行するようにしておく（とりあえず）
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