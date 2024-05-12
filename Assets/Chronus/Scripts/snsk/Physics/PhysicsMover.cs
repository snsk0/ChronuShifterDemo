using UnityEngine;

namespace Chronus.KinematicPhysics
{
    //PhysicsSystemより先に更新
    [DefaultExecutionOrder(-1001)]
    public class PhysicsMover : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        [SerializeField] private float _duration;

        private float _lastMovingT;

        private Vector3 _position;
        private Vector3 _prePosition;

        internal Vector3 DeltaPosition => _position - _prePosition;

        private float _timeCount = 0f;
        private float _lastFixedTime = 0f;


        private void Awake()
        {
            _position = _collider.transform.position;
            _prePosition = _position;

            PhysicsSystem.RegisterDynamicCollider(_collider);
        }

        private void FixedUpdate()
        {
            _prePosition = _position;
            _collider.transform.position = _position;

            // 補間位置計算
            var t = Mathf.PingPong(_timeCount / _duration, 2) - 0.5f;
            Mathf.Clamp(t, 0, 1);

            // 補間位置を反映
            _position = Vector3.Lerp(_start.position, _end.position, t);
            _collider.transform.position = _position;

            _lastFixedTime = Time.time;
        }

        private void Update()
        {
            _timeCount += Time.deltaTime;

            _collider.transform.position = _prePosition + DeltaPosition * ((Time.time - _lastFixedTime) / Time.fixedDeltaTime);
        }
    }
}
