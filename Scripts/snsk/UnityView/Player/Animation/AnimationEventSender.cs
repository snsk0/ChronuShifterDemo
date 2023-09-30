using System.Collections.Generic;

using UnityEngine;


namespace UnityView.Player.Animation
{
    //仮インターフェース
    public interface IAnimationEventHandler
    {
        public void OnAnimationEvent(string code);
    }



    public class AnimationEventSender : MonoBehaviour
    {
        //通知対象リスト
        private List<IAnimationEventHandler> animationEventHandlerList;


        private void Awake()
        {
            //仮コード 親からのみ取得
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
