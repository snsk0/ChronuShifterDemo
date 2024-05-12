using System.Collections.Generic;
using UnityEngine;

namespace Chronus.KinematicPhysics
{
    [RequireComponent (typeof (CapsuleCollider))]
    public class CharacterBody : MonoBehaviour
    {
        private const int MaxBufferSize = 16;

        [SerializeField] private float _maxSlopeAngle;

        private CapsuleCollider _collider;

        private Vector3 _position;
        private Vector3 _prePosition;
        private Vector3 _velocity;
        private Vector3 _acceleration;

        private Vector3 _groundNormal;

        private bool _isStickGround;
        private bool _isGround;
        private bool _isGrounded;

        private RaycastHit[] _raycastHits = new RaycastHit[MaxBufferSize];

        public Vector3 Velocity => _velocity;
        public Vector3 GroundNormal => _groundNormal;
        public bool IsGround => _isGround;

        private void Awake()
        {
            _collider = GetComponent<CapsuleCollider>();

            _position = _collider.transform.position;
            _velocity = Vector3.zero;
            _acceleration = Vector3.zero;

            _groundNormal = Vector3.zero;

            _isStickGround = true;
            _isGround = true;

            PhysicsSystem.Register(this);
        }

        private void OnDestroy()
        {
            PhysicsSystem.Unregister(this);
        }

        public void AddForce(Vector3 force)
        {
            _acceleration += force;
        }

        public void AddForceImpulse(Vector3 force)
        {
            _velocity += force;
        }

        public void MovePosition(Vector3 position)
        {
            _prePosition = position;
            _position = position;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;
        }

        public void SetIsStickGround(bool isStickGround)
        {
            _isStickGround = isStickGround;
        }

        public void Stop()
        {
            _velocity = Vector3.zero;
            _acceleration = Vector3.zero;

            _prePosition = _collider.transform.position;
            _position = _collider.transform.position;
        }

        /// <summary>
        /// �������Z,Static�X�V
        /// </summary>
        internal void InternalPhysicsStaticUpdate(float deltaTime)
        {
            //�p�����[�^�̏�����
            _collider.transform.position = _position;
            _prePosition = _position;
            _isGrounded = _isGround;

            //�����Փ˂̉���
            SolveOverlap();

            //�����̒n�ʒT��
            if (_isStickGround)
            {
                ProbeGround(false);
            }
            else
            {
                _groundNormal = Vector3.zero;
                _isGround = false;
            }

            Move(deltaTime);

            _collider.transform.position = _position;
        }

        /// <summary>
        /// �������Z�ADynamic�����̉��Z ������
        /// </summary>
        internal void InternalPhysicsDynamicUpdate(IEnumerable<Collider> dynamicColliders)
        {
            //�����Փ˂̉���
            Vector3 solutionDirection;
            float solutionDistance;

            foreach (Collider collider in dynamicColliders)
            {
                if (Physics.ComputePenetration(
                    _collider, _position, _collider.transform.rotation,
                    collider, collider.transform.position, collider.transform.rotation,
                    out solutionDirection, out solutionDistance))
                {
                    _position += solutionDirection * solutionDistance;

                    ProjectionVelocity(solutionDirection);
                }
            }

            //�ڒn����
            if (_isStickGround && !_isGround)
            {
                ProbeGround(true);
            }

            if (_isGround)
            {
                _velocity = ProjectionImmutableMagnitude(_velocity, _groundNormal);
            }
        }

        /// <summary>
        /// ���t���[���X�V
        /// </summary>
        internal void InternalInterpolateUpdate(float rate)
        {
            Vector3 deltaPosition = (_position - _prePosition) * rate;

            _collider.transform.position = _prePosition + deltaPosition;
        }

        /// <summary>
        /// �L�����N�^�[�𓮂����֐�
        /// </summary>
        private void Move(float deltaTime)
        {
            _velocity += _acceleration * deltaTime;
            _acceleration = Vector3.zero;

            if (_isGround)
            {
                _velocity = ProjectionImmutableMagnitude(_velocity, _groundNormal);
            }

            Vector3 castDirection = _velocity.normalized;
            float castDistance = (_velocity * deltaTime).magnitude;

            while (castDistance > PhysicsSetting.CollisionOffset)
            {
                if (ColliderCast(castDirection, castDistance, -PhysicsSetting.CollisionOffset, out RaycastHit closestResult, _raycastHits) > 0)
                {
                    _position += castDirection * closestResult.distance;
                    castDistance -= closestResult.distance;

                    float preVelocityMagnitude = _velocity.magnitude;

                    ProjectionVelocity(closestResult.normal);

                    castDistance *= _velocity.magnitude / preVelocityMagnitude;
                    castDirection = _velocity.normalized;
                }
                else
                {
                    _position += castDirection * castDistance;
                    castDistance = 0;
                }

                //Offset���Փ˂���������
                SolveOverlap();
            }
        }

        /// <summary>
        /// �@���Ɋ�Â���velocity�̓��e���s��
        /// </summary>
        private void ProjectionVelocity(Vector3 normal)
        {
            if (_isStickGround)
            {
                if (IsStableGroundNormal(normal))
                {
                    _groundNormal = normal;

                    //���n����
                    if (!_isGround)
                    {
                        _velocity = Vector3.ProjectOnPlane(_velocity, _collider.transform.up);
                        _isGround = true;
                    }

                    _velocity = ProjectionImmutableMagnitude(_velocity, _groundNormal);
                }
                else
                {
                    if (_isGround)
                    {
                        _velocity = Vector3.ProjectOnPlane(_velocity, GetProjectionNormal(normal));
                    }
                    else
                    {
                        _velocity = Vector3.ProjectOnPlane(_velocity, normal);
                    }
                }
            }
            else
            {
                _velocity = Vector3.ProjectOnPlane(_velocity, normal);
            }
        }

        /// <summary>
        /// �n�ʂ�T������
        /// </summary>
        private void ProbeGround(bool isDynamic)
        {
            Vector3 direction = -_collider.transform.up;
            float distance = PhysicsSetting.CollisionOffset * 2;

            //�O�t���[���ڒn���Ă�����
            if (_isGrounded)
            {
                distance = _collider.radius;
            }

            int count = GroundCast(direction, distance, PhysicsSetting.CollisionOffset, out RaycastHit closestResult, _raycastHits, isDynamic);
            if (count > 0)
            {
                if (IsStableGroundNormal(closestResult.normal))
                {
                    _position += direction * (closestResult.distance - PhysicsSetting.CollisionOffset);
                    _groundNormal = closestResult.normal;

                    //���n����
                    if (!_isGround)
                    {
                        _velocity = Vector3.ProjectOnPlane(_velocity, _collider.transform.up);
                        _isGround = true;
                    }
                    return;
                }
                else
                {
                    for(int i = 0; i < count; i++)
                    {
                        if(Mathf.Abs(closestResult.distance - _raycastHits[i].distance) < PhysicsSetting.CollisionOffset && IsStableGroundNormal(_raycastHits[i].normal))
                        {
                            _position += direction * (_raycastHits[i].distance - PhysicsSetting.CollisionOffset);
                            _groundNormal = _raycastHits[i].normal;
                            
                            //���n����
                            if (!_isGround)
                            {
                                _velocity = Vector3.ProjectOnPlane(_velocity, _collider.transform.up);
                                _isGround = true;
                            }
                            return;
                        }
                    }
                }
            }

            if (!isDynamic)
            {
                _groundNormal = Vector3.zero;
                _isGround = false;
            }
        }

        /// <summary>
        /// �Փ˂���������
        /// </summary>
        private void SolveOverlap()
        {
            //�����Փ˂̉���
            Collider[] results = new Collider[MaxBufferSize];
            int count = ColliderOverlap(results);

            Vector3 solutionDirection;
            float solutionDistance;

            for (int i = 0; i < count; ++i)
            {
                if (results[i] == _collider)
                {
                    continue;
                }

                if(Physics.ComputePenetration(
                    _collider, _position, _collider.transform.rotation,
                    results[i], results[i].transform.position, results[i].transform.rotation,
                    out solutionDirection, out solutionDistance))
                {
                    _position += solutionDirection * solutionDistance;
                }
            }
        }

        /// <summary>
        /// �n�ʂ̖@�������肵�Ă���Ƃ݂Ȃ���邩���肷��
        /// </summary>
        private bool IsStableGroundNormal(Vector3 normal)
        {
            return _maxSlopeAngle > Vector3.Angle(Vector3.up, normal);
        }

        /// <summary>
        /// ���x�𓊉e����@�����擾����
        /// </summary>
        private Vector3 GetProjectionNormal(Vector3 normal)
        {
            if (!IsStableGroundNormal(normal) && _isGround)
            {
                return new Vector3(normal.x, 0, normal.z).normalized;
            }

            return normal;
        }

        /// <summary>
        /// �x�N�g���̑傫����ς����ɓ��e���s��
        /// </summary>
        private Vector3 ProjectionImmutableMagnitude(Vector3 vector, Vector3 normal)
        {
            float resultMagnitude = vector.magnitude;
            Vector3 result = Vector3.ProjectOnPlane(vector, normal).normalized;

            return result * resultMagnitude;
        }

        /// <summary>
        /// �J�v�Z���L���X�g�̃��b�p�[�֐�
        /// </summary>
        private int ColliderCast(Vector3 direction, float distance, float offSet, out RaycastHit closestResult, RaycastHit[] results)
        {
            closestResult = new RaycastHit();

            direction = direction.normalized;

            Vector3 upPosition = _position + (_collider.transform.up * (_collider.height / 2 - _collider.radius));
            Vector3 downPositon = _position - (_collider.transform.up * (_collider.height / 2 - _collider.radius));

            int count = Physics.CapsuleCastNonAlloc(
                upPosition + _collider.transform.up * offSet,
                downPositon - _collider.transform.up * offSet, 
                _collider.radius + offSet,
                direction, 
                results, 
                distance,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);
           
            int lastIndex = count;
            float hitDistance = float.MaxValue;

            for (int i = count - 1; i >= 0; i--)
            {
                //���g�Ə����ڐG�Ɠ��I�I�u�W�F�N�g���t�B���^�����O
                if (results[i].collider == _collider || results[i].distance == 0 || results[i].collider.IsDynamic())
                {
                    lastIndex--;
                    results[i] = results[lastIndex];
                    continue;
                }

                //�Ŋ��̌��ʂ���肷��
                if (hitDistance > results[i].distance)
                {
                    hitDistance = results[i].distance;
                    closestResult = results[i];
                }
            }
            return lastIndex;
        }

        /// <summary>
        /// �n�ʒT���p
        /// </summary>
        private int GroundCast(Vector3 direction, float distance, float stepOffset, out RaycastHit closestResult, RaycastHit[] results, bool isDynamic)
        {
            closestResult = new RaycastHit();

            direction = direction.normalized;

            Vector3 upPosition = _position + (_collider.transform.up * (_collider.height / 2 - _collider.radius));
            Vector3 downPositon = _position - (_collider.transform.up * (_collider.height / 2 - _collider.radius));

            int count = Physics.CapsuleCastNonAlloc(
                upPosition,
                downPositon + _collider.transform.up * stepOffset,
                _collider.radius,
                direction,
                results,
                distance + stepOffset,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore);

            int lastIndex = count;
            float hitDistance = float.MaxValue;

            for (int i = count - 1; i >= 0; i--)
            {
                if (results[i].collider == _collider || isDynamic != results[i].collider.IsDynamic())
                {
                    lastIndex--;
                    results[i] = results[lastIndex];
                    continue;
                }

                if (results[i].distance == 0)
                {
                    if(results[i].collider as MeshCollider || results[i].collider as TerrainCollider)
                    {
                        if (Physics.CapsuleCast(
                            upPosition,
                            downPositon + _collider.transform.up * stepOffset,
                            _collider.radius - PhysicsSetting.CollisionOffset,
                            direction,
                            out RaycastHit hit,
                            distance + stepOffset,
                            Physics.AllLayers,
                            QueryTriggerInteraction.Ignore
                            ))
                        {
                            if (hit.collider == results[i].collider)
                            {
                                results[i] = hit;
                            }
                            else
                            {
                                lastIndex--;
                                results[i] = results[lastIndex];
                                continue;
                            }
                        }
                        else
                        {
                            lastIndex--;
                            results[i] = results[lastIndex];
                            continue;
                        }
                    }
                    else
                    {
                        lastIndex--;
                        results[i] = results[lastIndex];
                        continue;
                    }
                }

                //�Ŋ��̌��ʂ���肷��
                if (hitDistance > results[i].distance)
                {
                    hitDistance = results[i].distance;
                    closestResult = results[i];
                }
            }
            return lastIndex;
        }

        /// <summary>
        /// �R���C�_�[�I�[�o�[���b�v�̃��b�p�[�֐�
        /// </summary>
        private int ColliderOverlap(Collider[] results)
        {
            Vector3 upPosition = _position + (_collider.transform.up * (_collider.height / 2 - _collider.radius));
            Vector3 downPositon = _position - (_collider.transform.up * (_collider.height / 2 - _collider.radius));

            int count = Physics.OverlapCapsuleNonAlloc(
                upPosition, 
                downPositon, 
                _collider.radius, 
                results,
                Physics.AllLayers, 
                QueryTriggerInteraction.Ignore);

            int lastIndex = count;

            for(int i = count - 1; i >= 0; i--)
            {
                //���g�Ɠ��I�R���C�_�[���t�B���^�����O����
                if (results[i] == _collider || results[i].IsDynamic())
                {
                    lastIndex--;
                    results[i] = results[lastIndex];
                    break;
                }
            }

            return lastIndex;
        }
    }
}
