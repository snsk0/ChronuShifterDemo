using UnityEngine;

using Player.Behaviours;
using Player.Structure;


namespace UnityView.Player.Behaviours
{
    public class PlayerLook : MonoBehaviour, IPlayerLock
    {
        //コンポーネント
        [SerializeField] private new Rigidbody rigidbody;

        //調整用パラメータ
        [SerializeField] private float timeStep;

        //回転保持変数
        private Quaternion rotation;



        public LookDirection GetDirection()
        {
            Vector3 vec = gameObject.transform.forward;
            return new LookDirection(vec.x, vec.z);
        }

        public void Look(LookDirection direction)
        {
            //回転Quartanionの計算
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
            //回転操作
            rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, timeStep * Time.deltaTime));
            //transform.rotation = Quaternion.Lerp(transform.rotation, q, timeStep * Time.deltaTime);
        }
    }
}
