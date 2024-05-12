using FSM.Parameter;

using Player.Structure;


namespace Player.States
{
    public static class PlayerParameterContainerExtend
    {
        //パラメータを初期化する
        //ローカル
        public static void InitializePlayerLocalParameter(this IParameterContainer parameterContainer)
        {
            parameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
            parameterContainer.SetParameter(PlayerParameterKey.isSprintLock, false);
        }


        //グローバル
        public static void InitializePlayerGlobalParameter(this IParameterContainer parameterContainer)
        {
            parameterContainer.SetParameter(PlayerParameterKey.moveInput, new LookDirection(0, 0));
            parameterContainer.SetParameter(PlayerParameterKey.jumpInput, 0f);
            parameterContainer.SetParameter(PlayerParameterKey.sprintInput, false);
        }



        //Sprintをlock
        public static void SetSprintLock(this IParameterContainer parameterContainer, bool isLock)
        {
            if (isLock)
            {
                //sprintをfalseにしてロックする
                parameterContainer.SetParameter(PlayerParameterKey.isSprintLock, true);
                parameterContainer.SetParameter(PlayerParameterKey.isSprint, false);
            }

            else
            {
                //ロックを解除する
                parameterContainer.SetParameter(PlayerParameterKey.isSprintLock, false);
            }
        }
    }
}
