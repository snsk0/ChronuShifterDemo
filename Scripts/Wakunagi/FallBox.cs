using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FallBox : ChronusChangeGimmick {

    [SerializeField] private BoxOnTree myBoxOnTree;
    [SerializeField] private Rigidbody myRigidbody;

    [SerializeField] private GameObject box, core;

    bool isActive_box, isActive_core;
    [SerializeField] bool isBoxFall = false, isBoxBroke = false, isMotionBreak = true,isOnTree=false;

    [SerializeField] GameObject treeTopObj;
    [SerializeField] float distance_max;    //boxとcurrentTreeとの距離

    ChronusGimmickState oldBoxOnTreeState = ChronusGimmickState.current;

    //BoxOnTree側より遅く処理したいのでコルーチンで1フレーム待つ
    protected override void ChangeGimmick(bool isPast) {
        isBoxFall = false;
        myRigidbody.isKinematic = isPast;
        if (isPast) {
            box.SetActive(false);
            core.SetActive(false);
        }
        else if (isBoxBroke) {
            box.SetActive(false);
            core.SetActive(true);
        }
        else {
            StartCoroutine(WaitChangeGimmick(isPast));
        }
    }

    IEnumerator WaitChangeGimmick(bool isPast) {
        //1フレームだけ待つ
        yield return null;

        isActive_box = false;
        isActive_core = false;

        if (!isPast) {
            isActive_box = true;
            isActive_core = true;

            if (myBoxOnTree.boxOnTreeState == ChronusGimmickState.current) {
                myRigidbody.isKinematic = false;

                if (!isMotionBreak) isBoxFall = true;
                else {
                    if (isOnTree&&oldBoxOnTreeState == ChronusGimmickState.changed_current) {
                        StartCoroutine(FallMotion());
                    }
                }

                oldBoxOnTreeState = ChronusGimmickState.current;
            }
            if (myBoxOnTree.boxOnTreeState == ChronusGimmickState.changed_current) {
                Vector3 
                    boxPos = Change2D(gameObject.transform.position),
                    treePos = Change2D(myBoxOnTree.currentTree.transform.position);

                float distance = Vector3.SqrMagnitude(boxPos - treePos);

                //boxが範囲内にある場合
                if (distance <= distance_max * distance_max) {

                    gameObject.transform.position = treeTopObj.transform.position;
                    myRigidbody.isKinematic = true;

                    isMotionBreak = true;
                    isOnTree = true;
                }

                oldBoxOnTreeState = ChronusGimmickState.changed_current;
            }
        }

        box.SetActive(isActive_box);
        core.SetActive(isActive_core);
    }

    IEnumerator FallMotion() {
        isMotionBreak = false;
        isBoxFall = true;

        float fallTime = 1.0f;

        while (true) {
            yield return null;

            if (isMotionBreak) yield break;
            if (!isBoxFall) continue;

            fallTime -= Time.deltaTime;

            if (isBoxBroke) {
                if (fallTime > 0) isBoxBroke = false;
                else break;
            }
        }

        box.SetActive(false);
        core.SetActive(true);

    }

    private void OnCollisionEnter(Collision collision) {
        if (!isMotionBreak) {
            isBoxBroke = true;
        }
    }

    Vector3 Change2D(Vector3 value) {
        Vector3 vec = new Vector3(value.x, 0, value.z);
        return vec;
    }
}