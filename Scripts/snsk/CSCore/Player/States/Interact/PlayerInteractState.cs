using FSM.Core;

using Player.Behaviours;


namespace Player.States.Interact
{
    public class PlayerInteractState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //コンポーネント
        private IPlayerInteract interact;



        //初期化
        protected override void OnInitialize()
        {
            interact = owner.objectContainer.GetObject<IPlayerInteract>();
        }




        protected override void OnStart()
        {
            //sprintをしていたか取得
            bool isLastSprinted = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isLastSprinted);

            //interactを実行
            interact.Interact(isLastSprinted);
        }


        //終了したらEndを発行
        protected override void OnUpdate()
        {
            if (!interact.isInteracting) sendEvent(PlayerEvent.End);
        }


        //End時に実行中だったら中断呼び出し
        protected override void OnEnd()
        {
            if (interact.isInteracting) interact.Abort(); 
        }
    }
}
