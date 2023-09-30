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


        public readonly T type;                 //�A�j���[�V��������
        public readonly float speed;            //�Đ����x
        public readonly BlendParameter blend;   //Blend�p�����[�^
    }





    //�u�����h�p�����[�^
    public struct BlendParameter
    {
        //0�p�����[�^
        public static BlendParameter None = new BlendParameter(0, 0);


        public BlendParameter(float x, float y)
        {
            this.x = x;
            this.y = y;
        }



        public readonly float x;    //X����
        public readonly float y;    //Y����
    }
}
