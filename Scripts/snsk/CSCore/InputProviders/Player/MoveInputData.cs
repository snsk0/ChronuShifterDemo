namespace InputProviders.Player
{
    public struct MoveInputData
    {
        //�t�B�[���h
        public readonly float x;        //������
        public readonly float y;        //�c����


        //�R���X�g���N�^
        public MoveInputData(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        //�ړ����Ȃ����ǂ����̃`�F�b�N
        public bool isNone()
        {
            return x == 0 && y == 0;
        }
    }
}
