using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace MoveFloorGimmick {
        public class MoveFloor : PossibilityUseSwitchGimmick {
            [SerializeField] private GameObject moveObj;
            [SerializeField] private float speed;

            //��x�ړI�n�ɒ����Ƃ���Ŏ~�܂�B
            [SerializeField] private bool oneTime = false;

            //�s��̈ʒu�̃��X�g
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
                    Vector3 oldPos = moveObj.transform.position,    //�����O�̈ʒu
                        nextPos = movePos[mp_num],                  //�ړ���
                        moveVec = nextPos - oldPos,                 //�ړ���Ƃ̋���
                        dPos = Vector3.zero,                        //�����ʒu����̈ړ������̍��v
                        moveVecNlz = moveVec.normalized * speed;    //�ړ���
                    float moveVectorSize = moveVec.sqrMagnitude;    //�����ʒu�ƈړ���̋����̐��l��

                    //�����ʒu�ƈړ��悪�����Ȃ�I��
                    while (oldPos != nextPos) {
                        //�ړ����Ă��Ȃ���ԂȂ�ΏI��
                        if (!isMoving) break;

                        //���v�l���Čv�Z
                        float dt = Time.deltaTime;
                        dPos += moveVecNlz * dt;

                        //���ړ��ʂ̍��v�����������l����ړ���̋�������艓���Ȃ�I��
                        if (dPos.sqrMagnitude > moveVectorSize) break;

                        //���ۂ̈ړ�
                        moveObj.transform.position= dPos + oldPos;

                        yield return new WaitForFixedUpdate();
                    }
                    //�ړ����Ă��Ȃ���ԂȂ�ΏI��
                    if (!isMoving) break;

                    //�ʒu���C�����Ď��̏ꏊ��ݒ�
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