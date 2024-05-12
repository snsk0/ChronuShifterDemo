using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StageGimmick {
    namespace ForcedMoveGimmick {
        public class ForcedMoveFloor : PossibilityUseSwitchGimmick {

            private List<GameObject> allObjs = new List<GameObject>();//今までに乗ったオブジェクトのリスト
            private List<Rigidbody>
                 onRigid = new List<Rigidbody>(), //今乗っているオブジェクトの物理のリスト
                 allRigid = new List<Rigidbody>();//今までに乗ったオブジェクトの物理のリスト

            private List<int> removeObjNum = new List<int>();//除外予定のオブジェクト

            [SerializeField] private float 
                speed, //スピード
                lastingTime;//床から離れても動く時間

            private Vector3 moveAmount = Vector3.zero;//移動量ベクトル

            protected override bool IsNotFromBeginning() { return true; }//最初からコルーチンを実行するわけではない
            bool doingMoveMotion = false;//コルーチンが実行中

            protected override void VirtualStart() {
                float pi = Mathf.PI / 180;

                //各角度からそれぞれの方向の移動量を計算
                moveAmount.x = Mathf.Sin(transform.eulerAngles.y * pi);
                moveAmount.y = Mathf.Sin(-transform.eulerAngles.x * pi);
                moveAmount.z = Mathf.Cos(transform.eulerAngles.y * pi);
                moveAmount = moveAmount.normalized * speed;

                //最初から動くなら移動可能状態にする
                if (isFromBeginning) isMoving = true;
            }

            public override void OpenMotion() {
                isMoving = true;
                //オブジェクトが乗っているならコルーチンを実行
                if (onRigid.Count > 0) StartCoroutine(MoveMotion());
            }

            public override void CloseMotion() {
                isMoving = false;
            }

            private void OnTriggerEnter(Collider other) {
                GameObject otherObj = other.gameObject;
                bool haveOtherObj = false;
                //otherが既に登録されているか検索
                for (int i = 0; i < allObjs.Count; i++) {

                    //otherが既に登録いたら
                    if (allObjs[i] == otherObj) {
                        //除外予定のオブジェクトかどうかを検索
                        for (int j = 0; j < removeObjNum.Count; j++) {
                            //除外予定ならば取りやめ
                            if (removeObjNum[j] == i) {
                                removeObjNum.Remove(i);
                            }
                        }
                        haveOtherObj = true;
                        //一度加わっている力をリセット＆重力無効
                        allRigid[i].velocity = Vector3.zero;
                        allRigid[i].useGravity = false;
                        //乗っているオブジェクトに登録
                        onRigid.Add(allRigid[i]);
                    }
                }
                //otherが登録されていない
                if (!haveOtherObj) {
                    if (other.GetComponent<Rigidbody>() != null) {
                        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
                        //一度加わっている力をリセット＆重力無効
                        otherRigid.velocity = Vector3.zero;
                        otherRigid.useGravity = false;
                        //各リストに登録
                        allRigid.Add(otherRigid);
                        onRigid.Add(otherRigid);
                        allObjs.Add(otherObj);
                    }
                    else { Debug.LogError("Error: " + otherObj.name + " doesn\'t have RigidBody"); }
                }

                //オブジェクトは乗っているけど、コルーチンが実行されていなくて、移動可能状態か最初から動くなら、コルーチンを起動
                if (onRigid.Count > 0 && !doingMoveMotion && (isMoving || isFromBeginning)) {
                    isMoving = true;
                    StartCoroutine(MoveMotion());
                }
            }

            private void OnTriggerExit(Collider other) {
                GameObject otherObj = other.gameObject;
                //otherを検索
                for (int i = 0; i < allObjs.Count; i++) {
                    if (allObjs[i] == otherObj) {
                        //除外の予約
                        StartCoroutine(ExitObj(i));
                    }
                }

            }

            IEnumerator ExitObj(int i) {
                //除外予定のオブジェクトとして登録
                removeObjNum.Add(i);
                float t = 0;
                bool isRemove = false;
                //lastingTime分待つ
                for (t = 0; t < lastingTime; t += Time.deltaTime) {
                    yield return null;
                    //待っている間に除外予定のリストから抜けたら、コルーチンを終了
                    isRemove = false;
                    foreach (int n in removeObjNum) { if (n == i) isRemove = true; }
                    if (!isRemove) { yield break; }
                }
                //コルーチンが終了していなければオブジェクトを除外する
                removeObjNum.Remove(i);
                allRigid[i].useGravity = true;
                onRigid.Remove(allRigid[i]);
                //何も乗っていなければMoveMotionを終了
                if (onRigid.Count == 0) { isMoving = false; doingMoveMotion = false; }
            }


            protected override IEnumerator MoveMotion() {
                doingMoveMotion = true;
                //移動可能状態の間は実行
                while (isMoving) {
                    //乗っている全てのオブジェクトを移動
                    int obj_count = onRigid.Count, i;
                    for (i = 0; i < obj_count; i++) {
                        if (!isMoving) break;
                        Vector3 pos = onRigid[i].gameObject.transform.position;
                        onRigid[i].MovePosition(moveAmount + pos);
                    }
                    yield return new WaitForFixedUpdate();
                }
            }


        }
    }
}