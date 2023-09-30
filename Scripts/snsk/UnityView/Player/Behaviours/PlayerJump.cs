using UnityEngine;
using UniRx;

using Player.Behaviours;
using System;

namespace UnityView.Player.Behaviours
{
    //���C���^�[�t�F�[�X
    public interface IPlayerJumpPowerChanger
    {
        public void SetJumpMultiply(float multiply);
    }



    public class PlayerJump : MonoBehaviour, IPlayerJump, IPlayerJumpPowerChanger, ILeaveGroundAction
    {
        //�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;

        //�����p�p�����[�^
        [SerializeField] private float jumpMultipler;


        //�t�B�[���h
        private float jumpMultiply = 1.0f;


        //���͂��L���b�V������
        private float jumpMagnitude;


        public Subject<int> leaveSubject = new Subject<int>();
        public IObservable<int> leaveAction => leaveSubject;





        //�W�����v���`�F�b�N(�ړ����x��������Ȃ�true��,jump����������Ă��Ȃ��ꍇ���W�����v��)
        public bool isJumping()
        {
            if (jumpMagnitude > 0) return true;
            return false;
        }


        //�W�����v�̓���
        public void Jump(float strength)
        {
            jumpMagnitude = strength;
        }

        //�{���ύX
        public void SetJumpMultiply(float multiply)
        {
            jumpMultiply = multiply;
        }


        private void FixedUpdate()
        {
            if (jumpMagnitude > 0)
            {
                ////velocity�𒼐ڏ���������(�����ɔ��f���邽�߁A������s���Ȃ�GroundCheck�𑦍��ɔ��肵�Ȃ����K�v������)
                //Vector3 velocity = Vector3.up * jumpMagnitude * jumpMultipler * jumpMultiply;
                //rigidbody.velocity = velocity;

                //���t���[���ɔ��f
                //Debug.Log("Pre:" + rigidbody.velocity.y);
                leaveSubject.OnNext(10);
                Vector3 velocity = new Vector3(rigidbody.velocity.x, jumpMagnitude * jumpMultipler * jumpMultiply, rigidbody.velocity.z);
                rigidbody.velocity = velocity;
                //rigidbody.AddForce(Vector3.up * jumpMagnitude * jumpMultipler * jumpMultiply, ForceMode.VelocityChange);
                jumpMagnitude = 0;
                //Debug.Log("Aft:" + rigidbody.velocity.y);
            }
        }
    }
}
