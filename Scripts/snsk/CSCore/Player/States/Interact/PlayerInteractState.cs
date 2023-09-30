using FSM.Core;

using Player.Behaviours;


namespace Player.States.Interact
{
    public class PlayerInteractState : StateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�R���|�[�l���g
        private IPlayerInteract interact;



        //������
        protected override void OnInitialize()
        {
            interact = owner.objectContainer.GetObject<IPlayerInteract>();
        }




        protected override void OnStart()
        {
            //sprint�����Ă������擾
            bool isLastSprinted = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isLastSprinted);

            //interact�����s
            interact.Interact(isLastSprinted);
        }


        //�I��������End�𔭍s
        protected override void OnUpdate()
        {
            if (!interact.isInteracting) sendEvent(PlayerEvent.End);
        }


        //End���Ɏ��s���������璆�f�Ăяo��
        protected override void OnEnd()
        {
            if (interact.isInteracting) interact.Abort(); 
        }
    }
}
