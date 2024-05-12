using UnityEngine;
using Cysharp.Threading.Tasks;
using Player;
using Player.Behaviours;
using Player.Structure;

namespace UnityView.Player.Utils
{
    //ChronusItem�A���p
    public interface IChronusItemController
    {
        public void OnHandle(bool isPickUp);
        public UniTask CallItemAnimation(bool isPickUp);
        public float DropYOffset { get; }
    }

    //IInteractable�A��
    public interface IChronusGimmickController
    {
        public void OnInteract(MonoBehaviour owner);
        public bool IsInteracting { get; }
    }

    //�R���e�i������o���Ƃ��ɓ���C���^�[�t�F�[�X���K�v
    public interface ITempObject
    {
        public bool IsItem { get; }
    }

    //���N���X ������PlayeteHandledItem���Ȃ�
    public class ItemTemp : IPlayerItemObject, ITempObject
    {
        private readonly GameObject _gameObject;
        private readonly IChronusItemController _controller;

        public GameObject GameObject => _gameObject;
        public IChronusItemController Controller => _controller;
        public bool IsItem => true;

        public ItemTemp(GameObject gameObject, IChronusItemController controller)
        {
            _gameObject = gameObject;
            _controller = controller;
        }
    }

    //���N���X �M�~�b�N�Ƌ����ɂȂ�
    public class GimmickTemp : IPlayerGimmickObject, ITempObject
    {
        private readonly GameObject _gameObject;
        private readonly IChronusGimmickController _controller;

        public GameObject GameObject => _gameObject;
        public IChronusGimmickController Controller => _controller;
        public bool IsItem => false;

        public GimmickTemp(GameObject gameObject, IChronusGimmickController controller)
        {
            _gameObject = gameObject;
            _controller = controller;
        }
    }

    //�ˑ����t�]�p�̉��C���^�[�t�F�[�X
    public interface IContainerRapper
    {
        public T GetParameter<T>(string name);
    }

    public interface IItemDropOffLimitter
    {
        public bool TryGetDropOffPosition(ItemTemp temp, out Vector3 position);
    }
}
