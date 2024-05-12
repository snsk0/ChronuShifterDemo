using UnityEngine;

using Player.Behaviours;


namespace UnityView.Player.Behaviours
{
    [DefaultExecutionOrder(int.MaxValue)]
    public class PlayerGravity : MonoBehaviour
    {
        //�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;
        private IPlayerOnGrounded grounded;
        private IGroundNormalProvider groundNormalProvider;


        //�p�����[�^
        private float gravityScale;


        //�d�͊֘A
        private Vector3 initialGravity;
        public Vector3 gravity { get; private set; }


        //������
        private void Awake()
        {
            rigidbody.useGravity = false;
            groundNormalProvider = GetComponent<IGroundNormalProvider>();
            grounded = GetComponent<IPlayerOnGrounded>();

            gravityScale = Physics.gravity.magnitude;
            initialGravity = Vector3.down * gravityScale;
            gravity = initialGravity;
        }



        //�d�͍X�V
        private void FixedUpdate()
        {
            //if (groundNormalProvider.slopeNormal == Vector3.zero) gravity = initialGravity;
            //else gravity = -groundNormalProvider.slopeNormal.normalized * gravityScale;
            //rigidbody.AddForce(gravity, ForceMode.Acceleration);

            //�ڒn���Ă��Ȃ��������d�͂�������
            if (!grounded.IsOnGrounded())
            {
                //��Ƀx�N�g���𓊉e
                if(groundNormalProvider.slopeNormal != Vector3.zero)
                {
                    gravity = Vector3.ProjectOnPlane(Vector3.down, groundNormalProvider.slopeNormal) * Physics.gravity.magnitude;
                }
                else
                {
                    gravity = initialGravity;
                }

                rigidbody.AddForce(gravity, ForceMode.Acceleration);
            }

            Debug.DrawRay(transform.position, groundNormalProvider.slopeNormal, Color.blue, 1f);
            //Debug.Log(groundNormalProvider.slopeNormal);
        }
    }
}
