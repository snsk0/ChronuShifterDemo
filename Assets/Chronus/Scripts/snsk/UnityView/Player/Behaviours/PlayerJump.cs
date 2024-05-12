using UnityEngine;
using Player.Behaviours;
using Chronus.KinematicPhysics;
using Cysharp.Threading.Tasks;

namespace UnityView.Player.Behaviours
{
    //仮インターフェース
    public interface IPlayerJumpPowerChanger
    {
        public void SetJumpMultiply(float multiply);
    }

    public class PlayerJump : MonoBehaviour, IPlayerJump, IPlayerJumpPowerChanger
    {
        [SerializeField] private CharacterBody _characterBody;
        [SerializeField] private float _jumpMultipler;
        [SerializeField] private int _StickingFixedFrame;

        private IPlayerOnGrounded _onGrounded;

        private bool _isJumpingFixedFrame;
        private float _jumpMultiply = 1.0f;

        private void Awake()
        {
            _onGrounded = GetComponent<IPlayerOnGrounded>();
        }

        //ジャンプ中チェック
        //Yvelocity判定すると接地した後に坂を上ってるとアウト
        public bool isJumping()
        {
            return !_onGrounded.IsOnGrounded() || _isJumpingFixedFrame;
        }

        //ジャンプの入力
        public async void Jump(float strength)
        {
            _characterBody.SetIsStickGround(false);

            Vector3 velocity = _characterBody.Velocity;
            velocity.y = strength * _jumpMultipler * _jumpMultiply;
            _characterBody.SetVelocity(velocity);

            _isJumpingFixedFrame = true;

            await UniTask.DelayFrame(_StickingFixedFrame, PlayerLoopTiming.FixedUpdate);

            _characterBody.SetIsStickGround(true);
            _isJumpingFixedFrame = false;
        }

        //倍率変更
        public void SetJumpMultiply(float multiply)
        {
            _jumpMultiply = multiply;
        }
    }
}
