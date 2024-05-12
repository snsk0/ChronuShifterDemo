using UnityEngine;

namespace UnityView.Player.Utils
{
    /// <summary>
    /// �O������p�����[�^�R���e�i���Q�Ƃ��邽�߂̈ˑ����t�]�p�N���X
    /// </summary>
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerContainerReader : MonoBehaviour
    {
        private PlayerCore core;

        private void Awake()
        {
            core = GetComponent<PlayerCore>();
        }

        public T GetParameter<T>(string name)
        {
            return core.Container.GetParameter<T>(name);
        }
    }
}
