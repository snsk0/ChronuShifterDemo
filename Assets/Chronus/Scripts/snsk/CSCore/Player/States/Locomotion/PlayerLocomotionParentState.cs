using FSM.Core.Hierarchical;
using Player.Behaviours;
using Player.Structure;

namespace Player.States.Locomotion
{
    public class PlayerLocomotionParentState : ParentStateBase<PlayerFSMOwner, PlayerEvent>
    {
        //�R���|�[�l���g
        private IPlayerItemHandler _handler;
        private IPlayerInteractableSearcher _InteractableSearcher;
        private IPlayerItemDropPositionSeracher _dropPositionSeacher;

        //������
        protected override void OnInitialize()
        {
            _handler = owner.objectContainer.GetObject<IPlayerItemHandler>();
            _InteractableSearcher = owner.objectContainer.GetObject<IPlayerInteractableSearcher>();
            _dropPositionSeacher = owner.objectContainer.GetObject<IPlayerItemDropPositionSeracher>();

            //���R�[�h�Ńp�����[�^��ݒ�(�G���[���p)
            localParameterContainer.SetParameter(PlayerParameterKey.IsExitObject, false);
        }

        protected override void OnSelfStart()
        {
            //sprint�𖳌���
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

            //Lock���Ȃ�ݒ肵�Ȃ�
            bool sprintLock = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprintLock);
            if (sprintLock) return;

            //SprintInput���������g��
            bool sprintInput = globalParameterContainer.GetParameter<bool>(PlayerParameterKey.sprintInput);
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, sprintInput);
        }

        protected override void OnSelfEnd()
        {
            //sprinted���X�V
            bool isSprint = localParameterContainer.GetParameter<bool>(PlayerParameterKey.isSprint);
            localParameterContainer.SetParameter(PlayerParameterKey.isLastSprinted, isSprint);

            //sprint�𖳌���
            localParameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
        }

        //�C���^���N�g�����s�ł���Ȃ�J�ڂ�������
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
