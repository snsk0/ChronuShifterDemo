using UnityEngine;

namespace Chronus.OrbGimmick
{
    public class FloorChecker : MonoBehaviour
    {
        //�R���C�_�[�̎q�I�u�W�F�N�g�ɂ���ƕ����R���C�_�[�Ƃ��Ĕ��肳��Exit���Ă΂�Ȃ��̂ŕʂ̋�̃I�u�W�F�N�g�̎q�ɂ���
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