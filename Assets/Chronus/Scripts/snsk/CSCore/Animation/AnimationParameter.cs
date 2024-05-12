namespace Animation
{
    public struct AnimationParameter<T>
    {
        public AnimationParameter(T type, float speed)
        {
            this.type = type;
            this.speed = speed;
            blend = BlendParameter.None;
        }
        public AnimationParameter(T type, float speed, BlendParameter blend)
        {
            this.type = type;
            this.speed = speed;
            this.blend = blend;
        }


        public readonly T type;                 //アニメーション識別
        public readonly float speed;            //再生速度
        public readonly BlendParameter blend;   //Blendパラメータ
    }





    //ブレンドパラメータ
    public struct BlendParameter
    {
        //0パラメータ
        public static BlendParameter None = new BlendParameter(0, 0);


        public BlendParameter(float x, float y)
        {
            this.x = x;
            this.y = y;
        }



        public readonly float x;    //X成分
        public readonly float y;    //Y成分
    }
}
