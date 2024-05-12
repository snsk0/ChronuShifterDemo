using FSM.Core;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Interact
{
    public class PlayerInteractState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�R���|�[�l���g
        private IPlayerItemHandler _itemHandler;
        private IPlayerGimmickInteracter _gimmickInteracter;

        //������
        protected override void OnInitialize()
        {
            _itemHandler = owner.objectContainer.GetObject<IPlayerItemHandler>();
            _gimmickInteracter = owner.objectContainer.GetObject<IPlayerGimmickInteracter>();
        }

        protected override void OnStart()
        {
            if (localParameterContainer.GetParameter<bool>(PlayerParameterKey.IsExitObject))
            {
                IPlayerInteractableObject interactableObject = localParameterContainer.GetParameter<IPlayerInteractableObject>(PlayerParameterKey.SearchedObject);

                if (interactableObject is IPlayerGimmickObject)
                {
                    _gimmickInteracter.Interact(interactableObject as IPlayerGimmickObject);
                }
                else
                {
                    _itemHandler.PickUp(interactableObject as IPlayerItemObject);
                }
            }
            else if(_itemHandler.HasItem() && localParameterContainer.GetParameter<bool>(PlayerParameterKey.IsExitDropPosition))
            {
                bool isLastSprinted = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isLastSprinted);
                Position position = localParameterContainer.GetParameter<Position>(PlayerParameterKey.SearchedPosition);
                _itemHandler.DropOff(position, isLastSprinted);
            }
        }

        //�I��������End�𔭍s
        protected override void OnUpdate()
        {
            if (!_itemHandler.IsExecution && !_gimmickInteracter.IsInteracting)
            {
                sendEvent(PlayerEvent.End);
            }
        }

        //End���Ɏ��s���������璆�f�Ăяo��
        protected override void OnEnd()
        {
            if (_itemHandler.IsExecution)
            {
                _itemHandler.Abort();
            }
        }
    }
}
