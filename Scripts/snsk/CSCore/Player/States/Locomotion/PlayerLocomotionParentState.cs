using FSM.Core.Hierarchical;

using Player.Behaviours;


namespace Player.States.Locomotion
{
    public class PlayerLocomotionParentState : ParentStateBase<PlayerFSMOwner, PlayerEvent>
    {
        //コンポーネント
        private IPlayerInteract interact;


        //初期化
        protected override void OnInitialize()
        {
            interact = owner.objectContainer.GetObject<IPlayerInteract>();
        }


        protected override void OnSelfStart()
        {
            //sprintを無効化
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
        }


        protected override void OnSelfUpdate()
        {
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
            if (triggerEvent != PlayerEvent.Interact) return true;

            if (interact.CanInteract()) return true;
            else return false;
        }
    }
}
