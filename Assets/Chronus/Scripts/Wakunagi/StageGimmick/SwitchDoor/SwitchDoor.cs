using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace SwitchDoor {
        //床スイッチのスイッチの入力を管轄するプログラム
        public class SwitchDoor : MonoBehaviour {
            [SerializeField, Header("動いたままにするか")] private bool isPermanent;
            bool isOpened = false;

            //複数スイッチに対応させるためのリスト化
            [SerializeField] private List<SwitchPart> switches = new List<SwitchPart>(1);

            [SerializeField]
            private int s_count,        //スイッチの数
                isPushed = 0;   //スイッチが押されている数

            [SerializeField] private List<bool> isPushedOnj = new List<bool>();

            [SerializeField] private GimmickMotion gMotion;

            private void Start() {
                s_count = switches.Count;

                for (int i = 0; i < s_count; i++) isPushedOnj.Add(false);

                //gMotionが設定されていなければ取得する
                if (gMotion == null) gMotion = gameObject.GetComponent<GimmickMotion>();

                //そもそもアタッチされていなければエラー
                if (gMotion == null) Debug.Log("Error : Can\'t get GimmickMotion");

                //各スイッチにドア（自分）を登録
                foreach (SwitchPart s in switches) {
                    s.SetUp(this);
                }
            }

            public void ObjOnSwich(SwitchPart sp) {
                if (isOpened && isPermanent) return;

                for (int i = 0; i < s_count; i++) if (sp == switches[i]) isPushedOnj[i] = true;

                foreach (bool b in isPushedOnj) if (!b) return;

                isPushed++;
                //スイッチの押されている数が、スイッチの数と同じか？
                //if (isPushed == s_count) {
                isOpened = true;
                gMotion.OpenMotion();
                //}
                //else if (isPushed > s_count) Debug.LogError("Error : Pushed switches is over s_count.");
            }

            public void ObjOutSwich(SwitchPart sp) {
                if (isOpened && isPermanent) return;

                for (int i = 0; i < s_count; i++) if (sp == switches[i]) isPushedOnj[i] = false;

                bool isNotEqual = false;
                foreach (bool b in isPushedOnj) if (!b) isNotEqual = true;
                if (!isNotEqual) return;

                isPushed--;
                //スイッチの押されている数が、スイッチの数と違うか？
                //if (isPushed != s_count) {
                isOpened = false;
                gMotion.CloseMotion();
                //}
            }
        }
    }
}