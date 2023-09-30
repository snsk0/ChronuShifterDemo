using UnityEngine;
using UniRx;

using Player.Behaviours;
using System;

namespace UnityView.Player.Behaviours
{
    //仮インターフェース
    public interface IPlayerJumpPowerChanger
    {
        public void SetJumpMultiply(float multiply);
    }



    public class PlayerJump : MonoBehaviour, IPlayerJump, IPlayerJumpPowerChanger, ILeaveGroundAction
    {
        //コンポーネント
        [SerializeField] private new Rigidbody rigidbody;

        //調整用パラメータ
        [SerializeField] private float jumpMultipler;


        //フィールド
        private float jumpMultiply = 1.0f;


        //入力をキャッシュする
        private float jumpMagnitude;


        public Subject<int> leaveSubject = new Subject<int>();
        public IObservable<int> leaveAction => leaveSubject;





        //ジャンプ中チェック(移動速度が上方向ならtrueに,jumpが処理されていない場合もジャンプ中)
        public bool isJumping()
        {
            if (jumpMagnitude > 0) return true;
            return false;
        }


        //ジャンプの入力
        public void Jump(float strength)
        {
            jumpMagnitude = strength;
        }

        //倍率変更
        public void SetJumpMultiply(float multiply)
        {
            jumpMultiply = multiply;
        }


        private void FixedUpdate()
        {
            if (jumpMagnitude > 0)
            {
                ////velocityを直接書き換える(即座に反映するため、これを行うならGroundCheckを即座に判定しなおす必要がある)
                //Vector3 velocity = Vector3.up * jumpMagnitude * jumpMultipler * jumpMultiply;
                //rigidbody.velocity = velocity;

                //次フレームに反映
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
