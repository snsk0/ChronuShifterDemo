namespace Player.States
{
    public static class PlayerParameterKey
    {
        //�O��Input
        public const string moveInput = "moveInput";
        public const string jumpInput = "jumpInput";
        public const string sprintInput = "sprintInput";

        //���[�J��
        public const string isSprint = "isSprint";
        public const string isSprintLock = "isSprintLock";

        //�Ō�Ƀ_�b�V�����Ă�����
        public const string isLastSprinted = "isLastSprinted";

        //���������A�C�e��
        public const string IsExitObject = "IsExitObject";
        public const string SearchedObject = "SearchedObject";

        //�A�C�e���u���ꌟ��
        public const string IsExitDropPosition = "IsExitDropPosition";
        public const string SearchedPosition = "SearchedPosition";
    }
}
