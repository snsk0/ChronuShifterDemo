using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StageGimmick {
    namespace ForcedMoveGimmick {
        public class ForcedMoveFloor : PossibilityUseSwitchGimmick {

            private List<GameObject> allObjs = new List<GameObject>();//���܂łɏ�����I�u�W�F�N�g�̃��X�g
            private List<Rigidbody>
                 onRigid = new List<Rigidbody>(), //������Ă���I�u�W�F�N�g�̕����̃��X�g
                 allRigid = new List<Rigidbody>();//���܂łɏ�����I�u�W�F�N�g�̕����̃��X�g

            private List<int> removeObjNum = new List<int>();//���O�\��̃I�u�W�F�N�g

            [SerializeField] private float 
                speed, //�X�s�[�h
                lastingTime;//�����痣��Ă���������

            private Vector3 moveAmount = Vector3.zero;//�ړ��ʃx�N�g��

            protected override bool IsNotFromBeginning() { return true; }//�ŏ�����R���[�`�������s����킯�ł͂Ȃ�
            bool doingMoveMotion = false;//�R���[�`�������s��

            protected override void VirtualStart() {
                float pi = Mathf.PI / 180;

                //�e�p�x���炻�ꂼ��̕����̈ړ��ʂ��v�Z
                moveAmount.x = Mathf.Sin(transform.eulerAngles.y * pi);
                moveAmount.y = Mathf.Sin(-transform.eulerAngles.x * pi);
                moveAmount.z = Mathf.Cos(transform.eulerAngles.y * pi);
                moveAmount = moveAmount.normalized * speed;

                //�ŏ����瓮���Ȃ�ړ��\��Ԃɂ���
                if (isFromBeginning) isMoving = true;
            }

            public override void OpenMotion() {
                isMoving = true;
                //�I�u�W�F�N�g������Ă���Ȃ�R���[�`�������s
                if (onRigid.Count > 0) StartCoroutine(MoveMotion());
            }

            public override void CloseMotion() {
                isMoving = false;
            }

            private void OnTriggerEnter(Collider other) {
                GameObject otherObj = other.gameObject;
                bool haveOtherObj = false;
                //other�����ɓo�^����Ă��邩����
                for (int i = 0; i < allObjs.Count; i++) {

                    //other�����ɓo�^������
                    if (allObjs[i] == otherObj) {
                        //���O�\��̃I�u�W�F�N�g���ǂ���������
                        for (int j = 0; j < removeObjNum.Count; j++) {
                            //���O�\��Ȃ�Ύ����
                            if (removeObjNum[j] == i) {
                                removeObjNum.Remove(i);
                            }
                        }
                        haveOtherObj = true;
                        //��x������Ă���͂����Z�b�g���d�͖���
                        allRigid[i].velocity = Vector3.zero;
                        allRigid[i].useGravity = false;
                        //����Ă���I�u�W�F�N�g�ɓo�^
                        onRigid.Add(allRigid[i]);
                    }
                }
                //other���o�^����Ă��Ȃ�
                if (!haveOtherObj) {
                    if (other.GetComponent<Rigidbody>() != null) {
                        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
                        //��x������Ă���͂����Z�b�g���d�͖���
                        otherRigid.velocity = Vector3.zero;
                        otherRigid.useGravity = false;
                        //�e���X�g�ɓo�^
                        allRigid.Add(otherRigid);
                        onRigid.Add(otherRigid);
                        allObjs.Add(otherObj);
                    }
                    else { Debug.LogError("Error: " + otherObj.name + " doesn\'t have RigidBody"); }
                }

                //�I�u�W�F�N�g�͏���Ă��邯�ǁA�R���[�`�������s����Ă��Ȃ��āA�ړ��\��Ԃ��ŏ����瓮���Ȃ�A�R���[�`�����N��
                if (onRigid.Count > 0 && !doingMoveMotion && (isMoving || isFromBeginning)) {
                    isMoving = true;
                    StartCoroutine(MoveMotion());
                }
            }

            private void OnTriggerExit(Collider other) {
                GameObject otherObj = other.gameObject;
                //other������
                for (int i = 0; i < allObjs.Count; i++) {
                    if (allObjs[i] == otherObj) {
                        //���O�̗\��
                        StartCoroutine(ExitObj(i));
                    }
                }

            }

            IEnumerator ExitObj(int i) {
                //���O�\��̃I�u�W�F�N�g�Ƃ��ēo�^
                removeObjNum.Add(i);
                float t = 0;
                bool isRemove = false;
                //lastingTime���҂�
                for (t = 0; t < lastingTime; t += Time.deltaTime) {
                    yield return null;
                    //�҂��Ă���Ԃɏ��O�\��̃��X�g���甲������A�R���[�`�����I��
                    isRemove = false;
                    foreach (int n in removeObjNum) { if (n == i) isRemove = true; }
                    if (!isRemove) { yield break; }
                }
                //�R���[�`�����I�����Ă��Ȃ���΃I�u�W�F�N�g�����O����
                removeObjNum.Remove(i);
                allRigid[i].useGravity = true;
                onRigid.Remove(allRigid[i]);
                //��������Ă��Ȃ����MoveMotion���I��
                if (onRigid.Count == 0) { isMoving = false; doingMoveMotion = false; }
            }


            protected override IEnumerator MoveMotion() {
                doingMoveMotion = true;
                //�ړ��\��Ԃ̊Ԃ͎��s
                while (isMoving) {
                    //����Ă���S�ẴI�u�W�F�N�g���ړ�
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