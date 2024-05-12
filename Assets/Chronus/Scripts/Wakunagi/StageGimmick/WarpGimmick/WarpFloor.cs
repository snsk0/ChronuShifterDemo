using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace WarpGimmick {
        public class WarpFloor : PossibilityUseSwitchGimmick {

            //移動先の床
            [SerializeField] private GameObject nextObj;
            WarpFloor nextWarpFloor;

            [SerializeField] private float wateTime = 1.0f, sizeValue=0.6f;
            //移動してから次の移動が可能になるまでの時間
            private const float COOLTIME_WARP = 0.5f;
            float canWarp = 0;

            [SerializeField] private List<GameObject> warpEffectList, nomalEffectList;

            //乗ったオブジェクトが乗っているかの判定用class&List
            [System.Serializable]
            public class ExitManager {
               public bool isExit;
              public  GameObject onObj;
            }
            [SerializeField] private List<ExitManager> exitManager = new List<ExitManager>();

            protected override void VirtualStart() {
                if (nextObj.GetComponent<WarpFloor>() != null)
                    nextWarpFloor = nextObj.GetComponent<WarpFloor>();
                else Debug.LogError("Error: nextObj doesn\'t have WarpFloor.cs");

            }

            private void OnTriggerEnter(Collider other) {
                //動いていて、クールタイムを過ぎていればワープ
                if (isMoving && canWarp <= 0) {
                    ExitManager onObj = new ExitManager();
                    onObj.isExit = false;
                    onObj.onObj = other.gameObject;
                    exitManager.Add(onObj);

                    //ワープエフェクトを表示
                    ListObjctActiveSetter(warpEffectList, true);
                    ListObjctActiveSetter(nomalEffectList, false);

                    StartCoroutine(WarpStandby(other, exitManager.Count - 1));
                }
            }
            private void OnTriggerExit(Collider other) {
                bool isOnAnyObj=false;
                foreach (ExitManager em in exitManager) {
                    if (other.gameObject == em.onObj) em.isExit = true;
                    if (!em.isExit) isOnAnyObj = true;
                }
                //何も乗っていなければエフェクトを戻す
                if (!isOnAnyObj) {
                    ListObjctActiveSetter(warpEffectList, false);
                    ListObjctActiveSetter(nomalEffectList, true);
                }
            }

            IEnumerator WarpStandby(Collider other ,int num) {
                float dt = wateTime;
                while(dt > 0) {
                    if (exitManager[num].isExit) break;
                        dt -= Time.deltaTime;
                    yield return null;
                }

                if (!exitManager[num].isExit) {

                    float otherTall = other.transform.localScale.y / 2;
                    //ワープするオブジェクトがRigidbodyを持っているか
                    if (other.GetComponent<Rigidbody>() != null) {
                        yield return new WaitForFixedUpdate();
                        //nextObjのクールタイムをリセット
                        nextWarpFloor.NextWarpSafe(); 

                        //ワープ処理
                        other.gameObject.transform.position =
                            nextObj.transform.position + new Vector3(0, otherTall+ sizeValue, 0);

                        //エフェクトを戻す
                        ListObjctActiveSetter(warpEffectList, false);
                        ListObjctActiveSetter(nomalEffectList, true);
                    }
                }
                exitManager.RemoveAt(num);
            }

            public void NextWarpSafe() {
                //クールタイムをリセット
                canWarp = COOLTIME_WARP;
                StartCoroutine(CoolTime());
            }
            IEnumerator CoolTime() {
                //クールタイムを減少
                while (canWarp >= 0) {
                    yield return null;
                    canWarp -= Time.deltaTime;
                }
            }

            //リスト内のオブジェクトのアクティブ状態を一括で変更する関数
            void ListObjctActiveSetter(List<GameObject> objList, bool isActive) {
                int count = objList.Count;
                if(objList.Count==0) {
                    Debug.Log("List does not have objects!");
                    return;
                }
                for(int i = 0; i < count; i++) {
                    objList[i].SetActive(isActive);
                }
            }
        }
    }
}