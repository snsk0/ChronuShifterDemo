namespace Player.States
{
    public static class PlayerParameterKey
    {
        //�O��Input
        public const string moveInput = "moveInput";
        public const string jumpInput = "jumpInput";
        public const string sprintInput = "sprintInput";


        //���[�J��
        //�_�b�V���r�w�C�r�A�ɂ܂Ƃ߂Ă���������
        public const string isSprint = "isSprint";
        public const string isSprintLock = "isSprintLock";

        //�Ō�Ƀ_�b�V�����Ă�����
        public const string isLastSprinted = "isLastSprinted";
    }
}
