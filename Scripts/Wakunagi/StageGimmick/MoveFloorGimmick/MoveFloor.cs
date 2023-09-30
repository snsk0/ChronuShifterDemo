using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace MoveFloorGimmick {
        public class MoveFloor : PossibilityUseSwitchGimmick {
            [SerializeField] private GameObject moveObj;
            [SerializeField] private float speed;

            //一度目的地に着くとそれで止まる。
            [SerializeField] private bool oneTime = false;

            //行先の位置のリスト
            [SerializeField] private List<Vector3> movePos = new List<Vector3>();
            int mp_num = 0;

            private Rigidbody mObjeRigid;

            protected override void VirtualStart() {
                //mObjeRigid =moveObj.GetComponent<Rigidbody>();
                //mObjeRigid.isKinematic=true;
            }

            protected override IEnumerator MoveMotion() {
                //yield return null;
                while (isMoving) {
                    Vector3 oldPos = moveObj.transform.position,    //動く前の位置
                        nextPos = movePos[mp_num],                  //移動先
                        moveVec = nextPos - oldPos,                 //移動先との距離
                        dPos = Vector3.zero,                        //初期位置からの移動距離の合計
                        moveVecNlz = moveVec.normalized * speed;    //移動量
                    float moveVectorSize = moveVec.sqrMagnitude;    //初期位置と移動先の距離の数値化

                    //初期位置と移動先が同じなら終了
                    while (oldPos != nextPos) {
                        //移動していない状態ならば終了
                        if (!isMoving) break;

                        //合計値を再計算
                        float dt = Time.deltaTime;
                        dPos += moveVecNlz * dt;

                        //＜移動量の合計＞が＜初期値から移動先の距離＞より遠いなら終了
                        if (dPos.sqrMagnitude > moveVectorSize) break;

                        //実際の移動
                        moveObj.transform.position= dPos + oldPos;

                        yield return new WaitForFixedUpdate();
                    }
                    //移動していない状態ならば終了
                    if (!isMoving) break;

                    //位置を修正して次の場所を設定
                    moveObj.transform.position = nextPos;
                    mp_num = (mp_num + 1) % movePos.Count;

                    if (oneTime) {
                        isMoving = false; break;
                    }
                
                }
            }
        }
    }
}