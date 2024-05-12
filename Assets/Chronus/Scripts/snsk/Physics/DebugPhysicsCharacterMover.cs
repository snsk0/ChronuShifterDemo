using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.KinematicPhysics
{
    [DefaultExecutionOrder(-100)]
    public class DebugPhysicsCharacterMover : MonoBehaviour
    {
        [SerializeField] private CharacterBody characterBody;

        Vector3 input = Vector3.zero;

        [SerializeField] private float maxVelocity;
        [SerializeField] private float moveForceMultipler;
        [SerializeField] private float jumpPower;

        [SerializeField] private Camera _camera;

        private bool jumpInput = false;

        private int count = 5;
        private int c = 0;

        private void Update()
        {
            if(!jumpInput) jumpInput = Input.GetKeyDown(KeyCode.Space);
            input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        private void FixedUpdate()
        {
            c++;

            if (characterBody.IsGround)
            {
                var forward = _camera.transform.forward;
                forward.y = 0;
                var right = _camera.transform.right;
                right.y = 0;
                var dir = (forward.normalized * input.z + right.normalized * input.x).normalized * maxVelocity;

                var magnitude = dir.magnitude;
                dir = Vector3.ProjectOnPlane(dir, characterBody.GroundNormal).normalized * magnitude;

                //Debug.Log($"input:{dir}");
                characterBody.AddForce(moveForceMultipler * (dir - characterBody.Velocity));
            }

            if (jumpInput)
            {
                Debug.Log("jump");
                characterBody.SetIsStickGround(false);
                characterBody.AddForceImpulse(Vector3.up * jumpPower);
                jumpInput = false;
                c = 0;
            }

            if (!characterBody.IsGround)
            {
                var forward = _camera.transform.forward;
                forward.y = 0;
                var right = _camera.transform.right;
                right.y = 0;
                var dir = (forward.normalized * input.z + right.normalized * input.x).normalized * maxVelocity;

                characterBody.AddForce(UnityEngine.Physics.gravity);
                characterBody.AddForce(dir);
            }

            if (c > count) characterBody.SetIsStickGround(true);

            Debug.Log($"velocity:{characterBody.Velocity.magnitude}");
        }
    }
}
