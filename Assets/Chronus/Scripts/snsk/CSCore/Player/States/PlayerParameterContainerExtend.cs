using FSM.Parameter;

using Player.Structure;


namespace Player.States
{
    public static class PlayerParameterContainerExtend
    {
        //�p�����[�^������������
        //���[�J��
        public static void InitializePlayerLocalParameter(this IParameterContainer parameterContainer)
        {
            parameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
            parameterContainer.SetParameter(PlayerParameterKey.isSprintLock, false);
        }


        //�O���[�o��
        public static void InitializePlayerGlobalParameter(this IParameterContainer parameterContainer)
        {
            parameterContainer.SetParameter(PlayerParameterKey.moveInput, new LookDirection(0, 0));
            parameterContainer.SetParameter(PlayerParameterKey.jumpInput, 0f);
            parameterContainer.SetParameter(PlayerParameterKey.sprintInput, false);
        }



        //Sprint��lock
        public static void SetSprintLock(this IParameterContainer parameterContainer, bool isLock)
        {
            if (isLock)
            {
                //sprint��false�ɂ��ă��b�N����
                parameterContainer.SetParameter(PlayerParameterKey.isSprintLock, true);
                parameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
            }

            else
            {
                //���b�N����������
                parameterContainer.SetParameter(PlayerParameterKey.isSprintLock, false);
            }
        }
    }
}
