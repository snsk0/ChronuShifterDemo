using UnityEngine;
using Cysharp.Threading.Tasks;
using Player;
using Player.Behaviours;
using Player.Structure;

namespace UnityView.Player.Utils
{
    //ChronusItem連結用
    public interface IChronusItemController
    {
        public void OnHandle(bool isPickUp);
        public UniTask CallItemAnimation(bool isPickUp);
        public float DropYOffset { get; }
    }

    //IInteractable連結
    public interface IChronusGimmickController
    {
        public void OnInteract(MonoBehaviour owner);
        public bool IsInteracting { get; }
    }

    //コンテナから取り出すときに統一インターフェースが必要
    public interface ITempObject
    {
        public bool IsItem { get; }
    }

    //仮クラス 強引にPlayeteHandledItemをつなぐ
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

    //仮クラス ギミックと強引につなぐ
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

    //依存性逆転用の仮インターフェース
    public interface IContainerRapper
    {
        public T GetParameter<T>(string name);
    }

    public interface IItemDropOffLimitter
    {
        public bool TryGetDropOffPosition(ItemTemp temp, out Vector3 position);
    }
}
