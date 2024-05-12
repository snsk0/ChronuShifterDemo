using UnityEngine;

namespace Chronus.OrbGimmick
{
    public class FloorChecker : MonoBehaviour
    {
        //コライダーの子オブジェクトにすると複合コライダーとして判定されExitが呼ばれないので別の空のオブジェクトの子にする
        [SerializeField] private Transform _onFloorPlayerParent;

        private bool _playerEntered = false;
        private Transform _playerDefaultParent;

        private void OnTriggerEnter(Collider other)
        {
            //Collider other = collision.collider;
            if(other.CompareTag("Player"))
            {
                _playerDefaultParent = other.transform.parent;
                other.transform.parent = _onFloorPlayerParent;

                _playerEntered = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(_playerDefaultParent != null && other.CompareTag("Player"))
            {
                other.transform.parent = _playerDefaultParent;
                _playerDefaultParent = null;

                _playerEntered = false;
            }
        }
    }
}