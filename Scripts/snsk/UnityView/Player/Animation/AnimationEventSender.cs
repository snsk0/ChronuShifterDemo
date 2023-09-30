using System.Collections.Generic;

using UnityEngine;


namespace UnityView.Player.Animation
{
    //���C���^�[�t�F�[�X
    public interface IAnimationEventHandler
    {
        public void OnAnimationEvent(string code);
    }



    public class AnimationEventSender : MonoBehaviour
    {
        //�ʒm�Ώۃ��X�g
        private List<IAnimationEventHandler> animationEventHandlerList;


        private void Awake()
        {
            //���R�[�h �e����̂ݎ擾
            animationEventHandlerList = new List<IAnimationEventHandler>(GetComponentsInParent<IAnimationEventHandler>());
        }



        public void OnAnimationEvent(string code)
        {
            foreach(IAnimationEventHandler handler in animationEventHandlerList)
            {
                handler.OnAnimationEvent(code);
            }
        }
    }
}
