using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    namespace SwitchDoor {
        //���X�C�b�`�̃X�C�b�`�̓��͂��Ǌ�����v���O����
        public class SwitchDoor : MonoBehaviour {
            [SerializeField, Header("�������܂܂ɂ��邩")] private bool isPermanent;
            bool isOpened = false;

            //�����X�C�b�`�ɑΉ������邽�߂̃��X�g��
            [SerializeField] private List<SwitchPart> switches = new List<SwitchPart>(1);

            [SerializeField]
            private int s_count,        //�X�C�b�`�̐�
                isPushed = 0;   //�X�C�b�`��������Ă��鐔

            [SerializeField] private List<bool> isPushedOnj = new List<bool>();

            [SerializeField] private GimmickMotion gMotion;

            private void Start() {
                s_count = switches.Count;

                for (int i = 0; i < s_count; i++) isPushedOnj.Add(false);

                //gMotion���ݒ肳��Ă��Ȃ���Ύ擾����
                if (gMotion == null) gMotion = gameObject.GetComponent<GimmickMotion>();

                //���������A�^�b�`����Ă��Ȃ���΃G���[
                if (gMotion == null) Debug.Log("Error : Can\'t get GimmickMotion");

                //�e�X�C�b�`�Ƀh�A�i�����j��o�^
                foreach (SwitchPart s in switches) {
                    s.SetUp(this);
                }
            }

            public void ObjOnSwich(SwitchPart sp) {
                if (isOpened && isPermanent) return;

                for (int i = 0; i < s_count; i++) if (sp == switches[i]) isPushedOnj[i] = true;

                foreach (bool b in isPushedOnj) if (!b) return;

                isPushed++;
                //�X�C�b�`�̉�����Ă��鐔���A�X�C�b�`�̐��Ɠ������H
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
                //�X�C�b�`�̉�����Ă��鐔���A�X�C�b�`�̐��ƈႤ���H
                //if (isPushed != s_count) {
                isOpened = false;
                gMotion.CloseMotion();
                //}
            }
        }
    }
}