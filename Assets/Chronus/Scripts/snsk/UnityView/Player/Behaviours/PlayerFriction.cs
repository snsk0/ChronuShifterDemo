using Player.Behaviours;

using UnityEngine;


namespace UnityView.Player.Behaviours
{
    [DefaultExecutionOrder(int.MaxValue)]
    public class PlayerFriction : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private float friction;
        private IPlayerOnGrounded grounded;



        private void Awake()
        {
            grounded = GetComponent<IPlayerOnGrounded>();
        }


        private void FixedUpdate()
        {
            if (grounded.IsOnGrounded())
            {
                //Vector3 velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
                //Debug.DrawRay(transform.position, -rigidbody.velocity * 5, Color.blue, 1);
                rigidbody.AddForce((friction * -rigidbody.velocity), ForceMode.Acceleration);
            }
        }
    }
}
