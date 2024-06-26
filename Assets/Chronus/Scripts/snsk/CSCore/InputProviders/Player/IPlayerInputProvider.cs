using System;


namespace InputProviders.Player
{
    public interface IPlayerInputProvider
    {
        //入力の可否
        public void SetInputActive(bool active);

        //移動入力
        public IObservable<MoveInputData> onMoveInput { get; }

        //ダッシュ
        public IObservable<SprintInputData> onSprintInput { get; }

        //ジャンプ入力
        public IObservable<JumpInputData> onJumpInput { get; }

        //インタラクト入力
        public IObservable<bool> onInteractInput { get; }


        //タイムシフト入力


        //エスケープ入力

    }
}
