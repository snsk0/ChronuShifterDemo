using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace WarpGimmick {
        public class WarpFloor : PossibilityUseSwitchGimmick {

            //�ړ���̏�
            [SerializeField] private GameObject nextObj;
            WarpFloor nextWarpFloor;

            [SerializeField] private float wateTime = 1.0f, sizeValue=0.6f;
            //�ړ����Ă��玟�̈ړ����\�ɂȂ�܂ł̎���
            private const float COOLTIME_WARP = 0.5f;
            float canWarp = 0;

            [SerializeField] private List<GameObject> warpEffectList, nomalEffectList;

            //������I�u�W�F�N�g������Ă��邩�̔���pclass&List
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
                //�����Ă��āA�N�[���^�C�����߂��Ă���΃��[�v
                if (isMoving && canWarp <= 0) {
                    ExitManager onObj = new ExitManager();
                    onObj.isExit = false;
                    onObj.onObj = other.gameObject;
                    exitManager.Add(onObj);

                    //���[�v�G�t�F�N�g��\��
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
                //��������Ă��Ȃ���΃G�t�F�N�g��߂�
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
                    //���[�v����I�u�W�F�N�g��Rigidbody�������Ă��邩
                    if (other.GetComponent<Rigidbody>() != null) {
                        yield return new WaitForFixedUpdate();
                        //nextObj�̃N�[���^�C�������Z�b�g
                        nextWarpFloor.NextWarpSafe(); 

                        //���[�v����
                        other.gameObject.transform.position =
                            nextObj.transform.position + new Vector3(0, otherTall+ sizeValue, 0);

                        //�G�t�F�N�g��߂�
                        ListObjctActiveSetter(warpEffectList, false);
                        ListObjctActiveSetter(nomalEffectList, true);
                    }
                }
                exitManager.RemoveAt(num);
            }

            public void NextWarpSafe() {
                //�N�[���^�C�������Z�b�g
                canWarp = COOLTIME_WARP;
                StartCoroutine(CoolTime());
            }
            IEnumerator CoolTime() {
                //�N�[���^�C��������
                while (canWarp >= 0) {
                    yield return null;
                    canWarp -= Time.deltaTime;
                }
            }

            //���X�g���̃I�u�W�F�N�g�̃A�N�e�B�u��Ԃ��ꊇ�ŕύX����֐�
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