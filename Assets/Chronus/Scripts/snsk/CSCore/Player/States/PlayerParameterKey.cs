namespace Player.States
{
    public static class PlayerParameterKey
    {
        //外部Input
        public const string moveInput = "moveInput";
        public const string jumpInput = "jumpInput";
        public const string sprintInput = "sprintInput";

        //ローカル
        public const string isSprint = "isSprint";
        public const string isSprintLock = "isSprintLock";

        //最後にダッシュしていたか
        public const string isLastSprinted = "isLastSprinted";

        //検索したアイテム
        public const string IsExitObject = "IsExitObject";
        public const string SearchedObject = "SearchedObject";

        //アイテム置き場検索
        public const string IsExitDropPosition = "IsExitDropPosition";
        public const string SearchedPosition = "SearchedPosition";
    }
}
