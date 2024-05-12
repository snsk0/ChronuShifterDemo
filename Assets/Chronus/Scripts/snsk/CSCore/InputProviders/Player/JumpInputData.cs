namespace InputProviders.Player
{
    public struct JumpInputData
    {
        public readonly float magnitude;    //ジャンプ力


        //コンストラクタ
        public JumpInputData(float magnitude)
        {
            this.magnitude = magnitude;
        }
    }
}
