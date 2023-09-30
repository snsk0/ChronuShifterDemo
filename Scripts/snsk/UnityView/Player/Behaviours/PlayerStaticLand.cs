using UnityEngine;

using Player.Behaviours;


namespace UnityView.Player.Behaviours
{
    public class PlayerStaticLand : MonoBehaviour
    {
        //必要コンポーネント
        [SerializeField] private new Rigidbody rigidbody;
        private IPlayerOnGrounded grounded;
        private IGroundNormalProvider normalProvider;

        //フィールド
        private bool preGrounded = true;



        //初期化
        private void Awake()
        {
            grounded = GetComponent<IPlayerOnGrounded>();
            normalProvider = GetComponent<IGroundNormalProvider>();
        }



        //Landing処理
        private void FixedUpdate()
        {
            //着地を行った場合
            if(!preGrounded && grounded.IsOnGrounded() && rigidbody.velocity.y < 0)
            {
                //水平方向の移動がなかった場合
                Vector3 horizontalVelocity = Vector3.Scale(new Vector3(1, 0, 1), rigidbody.velocity);
                if (horizontalVelocity.magnitude < Physics.sleepThreshold)
                {
                    //Y速度を坂と垂直の成分に変換する
                    Vector3 vector = normalProvider.slopeNormal.normalized * rigidbody.velocity.y;
                    rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                    rigidbody.velocity += vector;
                }
            }


            //前フレームのgroundedを更新
            preGrounded = grounded.IsOnGrounded();
        }
    }
}
