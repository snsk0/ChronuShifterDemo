namespace InputProviders.Player
{
    public struct MoveInputData
    {
        //フィールド
        public readonly float x;        //横入力
        public readonly float y;        //縦入力


        //コンストラクタ
        public MoveInputData(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        //移動がないかどうかのチェック
        public bool isNone()
        {
            return x == 0 && y == 0;
        }
    }
}
