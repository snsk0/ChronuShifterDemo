using FSM.Core.Hierarchical;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion
{
    public class PlayerLocomotionParentState : ParentStateBase<PlayerFSMOwner, PlayerEvent>
    {
        //コンポーネント
        private IPlayerItemHandler _handler;
        private IPlayerInteractableSearcher _InteractableSearcher;
        private IPlayerItemDropPositionSeracher _dropPositionSeacher;

        //初期化
        protected override void OnInitialize()
        {
            _handler = owner.objectContainer.GetObject<IPlayerItemHandler>();
            _InteractableSearcher = owner.objectContainer.GetObject<IPlayerInteractableSearcher>();
            _dropPositionSeacher = owner.objectContainer.GetObject<IPlayerItemDropPositionSeracher>();

            //仮コードでパラメータを設定(エラー回避用)
            localParameterContainer.SetParameter(PlayerParameterKey.IsExitObject, false);
        }

        protected override void OnSelfStart()
        {
            //sprintを無効化
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
        }

        protected override void OnSelfUpdate()
        {
            IPlayerInteractableObject interactable = _InteractableSearcher.SearchObject();
            localParameterContainer.SetParameter(PlayerParameterKey.IsExitObject, interactable != null);
            localParameterContainer.SetParameter(PlayerParameterKey.SearchedObject, interactable);

            if (_handler.HasItem())
            {
                if (_dropPositionSeacher.TryGetDropPosition(_handler.GetItem(), out Position position))
                {
                    localParameterContainer.SetParameter(PlayerParameterKey.IsExitDropPosition, true);
                    localParameterContainer.SetParameter(PlayerParameterKey.SearchedPosition, position);
                }
                else
                {
                    localParameterContainer.SetParameter(PlayerParameterKey.IsExitDropPosition, false);
                }
            }

            //Lock中なら設定しない
            bool sprintLock = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprintLock);
            if (sprintLock) return;

            //SprintInputを取ったら使う
            bool sprintInput = globalParameterContainer.GetParameter<bool>(PlayerParameterKey.sprintInput);
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, sprintInput);
        }

        protected override void OnSelfEnd()
        {
            //sprintedを更新
            bool isSprint = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);
            localParameterContainer.SetParameter(PlayerParameterKey.isLastSprinted, isSprint);

            //sprintを無効化
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
        }

        //インタラクトが実行できるなら遷移を許可する
        protected override bool SelfGuardEvent(PlayerEvent triggerEvent)
        {
            if (triggerEvent != PlayerEvent.Interact)
            {
                return true;
            }

            if (localParameterContainer.GetParameter<bool>(PlayerParameterKey.IsExitObject))
            {
                return true;
            }

            if(_handler.HasItem() && localParameterContainer.GetParameter<bool>(PlayerParameterKey.IsExitDropPosition))
            {
                return true;
            }

            return false;
        }
    }
}
