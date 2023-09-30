using UnityEngine;

using Player.Behaviours;
using Player.Structure;


namespace UnityView.Player.Behaviours
{
    public class PlayerLook : MonoBehaviour, IPlayerLock
    {
        //�R���|�[�l���g
        [SerializeField] private new Rigidbody rigidbody;

        //�����p�p�����[�^
        [SerializeField] private float timeStep;

        //��]�ێ��ϐ�
        private Quaternion rotation;



        public LookDirection GetDirection()
        {
            Vector3 vec = gameObject.transform.forward;
            return new LookDirection(vec.x, vec.z);
        }

        public void Look(LookDirection direction)
        {
            //��]Quartanion�̌v�Z
            Vector3 vec = new Vector3(direction.x, 0, direction.y);
            vec.y = 0;
            rotation = Quaternion.LookRotation(vec, Vector3.up);
        }


        private void Awake()
        {
            rotation = transform.rotation;
        }
        private void Update()
        {
            //��]����
            rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, timeStep * Time.deltaTime));
            //transform.rotation = Quaternion.Lerp(transform.rotation, q, timeStep * Time.deltaTime);
        }
    }
}
