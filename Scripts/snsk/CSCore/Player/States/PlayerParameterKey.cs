namespace Player.States
{
    public static class PlayerParameterKey
    {
        //外部Input
        public const string moveInput = "moveInput";
        public const string jumpInput = "jumpInput";
        public const string sprintInput = "sprintInput";


        //ローカル
        //ダッシュビヘイビアにまとめてもいいかも
        public const string isSprint = "isSprint";
        public const string isSprintLock = "isSprintLock";

        //最後にダッシュしていたか
        public const string isLastSprinted = "isLastSprinted";
    }
}
