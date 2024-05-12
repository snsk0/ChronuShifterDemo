using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace SwitchDoor {
        //�X�C�b�`�{�̂ɑ΂�����͂�����
        public class SwitchPart : MonoBehaviour {

            //�Ǘ��v���O������o�^
            SwitchDoor mySwitchDoor;
            public void SetUp(SwitchDoor sd) {
                mySwitchDoor = sd;
                onObjList = OnObjListScript.Instance.onObjList;
            }

            [SerializeField] int isInsideObj = 0;
            [SerializeField] private List<GameObject> onObjs = new List<GameObject>();
            [SerializeField] private List<Collider> onCollider = new List<Collider>();
            bool isMove = false;

            [SerializeField] private List<GameObject> onObjList = new List<GameObject>();

            private void OnTriggerEnter(Collider other) {
                bool isObjInList = false;
                foreach (GameObject obj in onObjList) if (other.gameObject == obj) isObjInList = true;
                if (!isObjInList) return;

                if (!isMove) { isMove = true; StartCoroutine(CheckFide()); }
                isInsideObj++;
                onObjs.Add(other.gameObject);
                onCollider.Add(other);
                //�I�u�W�F�N�g��1�ȏ����Ă���ΊǗ��v���O�����ɏ���Ă��鎖��`����
                if (isInsideObj >= 1) mySwitchDoor.ObjOnSwich(this);

            }

            IEnumerator CheckFide() {
                while (isMove) {
                    bool isExit = false;
                    GameObject obj = null;
                    Collider collider = null;
                    foreach (GameObject _obj in onObjs) {
                        if (!_obj.activeInHierarchy) { 
                            obj = _obj; isExit = true;
                            collider = obj.GetComponent<Collider>();
                            break;
                        }
                    }
                    foreach(Collider col in onCollider) {
                        if (!col.enabled) {
                            obj = col.gameObject;
                            collider = col;
                            isExit = true;
                            break;
                        }
                    }
                    if (isExit) ExitObj(obj,collider);
                    yield return null;
                }

            }

            private void OnTriggerExit(Collider other) {
                ExitObj(other.gameObject,other);
            }

            void ExitObj(GameObject obj, Collider collider) {
                bool isObjInList = false;
                foreach (GameObject objInList in onObjList) if (obj.gameObject == objInList) isObjInList = true;
                if (!isObjInList) return;

                isInsideObj--;
                onObjs.Remove(obj.gameObject);
                onCollider.Remove(collider);

                //�I�u�W�F�N�g���S������Ă��Ȃ���ΊǗ��v���O�����ɏ���Ă��Ȃ�����`����
                if (isInsideObj == 0) { mySwitchDoor.ObjOutSwich(this); isMove = false; }
            }

        }
    }
}