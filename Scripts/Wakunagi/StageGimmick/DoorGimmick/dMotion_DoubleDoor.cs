using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace StageGimmick {
    namespace DoorGimmick {
        //�e���J���h�A�̊J�����̐e�N���X
        public class dMotion_DoubleDoor : DoorMotion {

            [SerializeField] protected GameObject right_door, left_door;
            [SerializeField] protected int move_speed;

            //�ړ��̕ω���
            [SerializeField] private float slide_amount;

            protected float door_move = 0;//���݂̃h�A�̕ω���
            protected int isMinus = 1; //�ω�������

            protected float
                old_difference_value = -1,  //1�t���[���O��difference_value
                difference_value = -1;      //�ړI�̒l�ƌ��݂̕ω��ʂ̍�
            protected float desired_value;  //�ړI�̒l�i�󂯓n�����߁j
            [SerializeField] private SESingleton SE = null;

            public override void OpenMotion() {
                base.OpenMotion();
                isOpen = 1;//�J���Ă��邱�Ƃ���
                StartCoroutine(DoorMotion(slide_amount));
            }

            public override void CloseMotion() {
                base.CloseMotion();
                isOpen = 2;//�܂��Ă��邱�Ƃɂ���
                StartCoroutine(DoorMotion(0));
            }


            //���[�V����
            IEnumerator DoorMotion(float dv) {//�ړI�̒l
                if (desired_value == dv)yield break;
                desired_value = dv;
                int myIsOpen = isOpen;//���s���ɊJ�����܂邩��o�^���Ă���

                //�e�l��������
                isMinus = 1;
                if (desired_value < door_move) isMinus = -1;
                old_difference_value = -1; difference_value = -1;

                if(SE == null)
                {
                    GameObject seManager = GameObject.Find("SEManager");
                    if(seManager != null)
                    SE = seManager.GetComponent<SESingleton>();
                }
                if(SE!=null)SE.DoorSE();

                //���[�V����
                while (true) {
                    //�o�^���Ēu�����l�ƈقȂ�ΏI��
                    if (myIsOpen != isOpen) break;

                    //�ω��ʂ��v�Z
                    float dt = Time.deltaTime;
                    door_move += move_speed * slide_amount * dt * isMinus;

                    
                    //���[�V�����̎��s����
                    MotionPart();

                    //1�t���[���O���傫����Βʂ�߂����Ƃ������ƂŏI��
                    old_difference_value = difference_value;
                    difference_value = Mathf.Abs(desired_value - door_move);
                    if (old_difference_value != -1 && old_difference_value < difference_value) break;

                    yield return null;
                }

                //�������I�����Ă���ΏI�����������s
                if (myIsOpen == isOpen) {
                    isOpen = 0;
                    LastPart();
                }
            }

            //���[�V���������s����
            protected virtual void MotionPart() {

            }

            //�I������
            protected virtual void LastPart() {

            }

        }

    }
}