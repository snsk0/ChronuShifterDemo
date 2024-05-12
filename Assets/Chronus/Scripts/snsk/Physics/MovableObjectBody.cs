using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.KinematicPhysics
{
    public class MovableObjectBody : MonoBehaviour
    {
        private const int MaxBufferSize = 4;

        private CapsuleCollider _collider;

        private Vector3 _position;
        private Vector3 _velocity;
        private Vector3 _acceleration;

        private RaycastHit[] _raycastHits = new RaycastHit[MaxBufferSize];

        internal void InternalPhysicsUpdate(float deltaTime)
        {

        }
    }
}
