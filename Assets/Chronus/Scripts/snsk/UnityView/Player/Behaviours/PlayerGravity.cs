using UnityEngine;

using Player.Behaviours;


namespace UnityView.Player.Behaviours
{
    [DefaultExecutionOrder(int.MaxValue)]
    public class PlayerGravity : MonoBehaviour
    {
        //コンポーネント
        [SerializeField] private new Rigidbody rigidbody;
        private IPlayerOnGrounded grounded;
        private IGroundNormalProvider groundNormalProvider;


        //パラメータ
        private float gravityScale;


        //重力関連
        private Vector3 initialGravity;
        public Vector3 gravity { get; private set; }


        //初期化
        private void Awake()
        {
            rigidbody.useGravity = false;
            groundNormalProvider = GetComponent<IGroundNormalProvider>();
            grounded = GetComponent<IPlayerOnGrounded>();

            gravityScale = Physics.gravity.magnitude;
            initialGravity = Vector3.down * gravityScale;
            gravity = initialGravity;
        }



        //重力更新
        private void FixedUpdate()
        {
            //if (groundNormalProvider.slopeNormal == Vector3.zero) gravity = initialGravity;
            //else gravity = -groundNormalProvider.slopeNormal.normalized * gravityScale;
            //rigidbody.AddForce(gravity, ForceMode.Acceleration);

            //接地していない時だけ重力をかける
            if (!grounded.IsOnGrounded())
            {
                //坂にベクトルを投影
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
