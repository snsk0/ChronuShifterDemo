using UnityEngine;

using Player.Behaviours;


namespace UnityView.Player.Behaviours
{
    public class PlayerStaticLand : MonoBehaviour
    {
        //�K�v�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;
        private IPlayerOnGrounded grounded;
        private IGroundNormalProvider normalProvider;

        //�t�B�[���h
        private bool preGrounded = true;



        //������
        private void Awake()
        {
            grounded = GetComponent<IPlayerOnGrounded>();
            normalProvider = GetComponent<IGroundNormalProvider>();
        }



        //Landing����
        private void FixedUpdate()
        {
            //���n���s�����ꍇ
            if(!preGrounded && grounded.IsOnGrounded() && rigidbody.velocity.y < 0)
            {
                //���������̈ړ����Ȃ������ꍇ
                Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), rigidbody.velocity);
                if (horizontalVelocity.magnitude < Physics.sleepThreshold)
                {
                    //Y���x����Ɛ����̐����ɕϊ�����
                    Vector3 vector = normalProvider.slopeNormal.normalized * rigidbody.velocity.y;
                    rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                    rigidbody.velocity += vector;
                }
            }


            //�O�t���[����grounded���X�V
            preGrounded = grounded.IsOnGrounded();
        }
    }
}
