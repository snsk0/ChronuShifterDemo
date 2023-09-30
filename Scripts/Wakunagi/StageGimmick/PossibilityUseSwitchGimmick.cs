using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    public class PossibilityUseSwitchGimmick : GimmickMotion {
        //最初から動いているか
        [SerializeField, Header("最初から動くか")] protected bool isFromBeginning = false;

        protected bool isMoving = false;
        protected virtual bool IsNotFromBeginning() { return false; }//最初からコルーチンを起動するわけではない

        void Start() {
            //対応するスイッチがなければ最初から実行するようにしておく（とりあえず）
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