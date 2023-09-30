using System;
using System.Collections.Generic;

using FSM.Core;
using InputProviders.LifeCycle;
using RxUtils;


namespace FSMUtils
{
    //インターフェースより深いネームスペース層に置いたほうが良いかもしれない(FSMUtility.Managerのような)
    public class FSMUpdateManager : IFSMUpdator
    {
        //自身のインスタンス
        public static FSMUpdateManager instance { get; private set; }

        //初期化メソッド
        public static void Initialize(IInputUpdateProvider updateProvider)
        {
            
            //if (instance != null) return; シーンをまたいだ時に原因になる
            //やるならupdateProviderがnullかどうかとか
            instance = new FSMUpdateManager(updateProvider);
        }




        //UpdateInput
        private IInputUpdateProvider updateProvider;
        private IDisposable providerDisposable;

        //ステートマシンのアップデートメソッドリスト
        private List<Action> fsmTickList;




        //コンストラクタを非公開(updateProviderはDI
        private FSMUpdateManager(IInputUpdateProvider updateProvider)
        {
            this.updateProvider = updateProvider;
            providerDisposable  = this.updateProvider.onUpdate.Subscribe(_ => { UpdateAll(); });

            fsmTickList = new List<Action>();
        }

        //デコンストラクタでUpdate解除
        ~FSMUpdateManager()
        {
            providerDisposable.Dispose();
        }



        //一括Update
        private void UpdateAll()
        {
            foreach(Action fsmTick in fsmTickList)
            {
                fsmTick.Invoke();
            }
        }

        //登録関数
        public void RegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm)
        {
            if (fsmTickList.Contains(fsm.Tick)) return;
            fsmTickList.Add(fsm.Tick);
        } 

        
        //解除関数
        public void UnRegisterFSM<TOwner, TEvent>(FiniteStateMachine<TOwner, TEvent> fsm)
        {
            if (fsmTickList.Contains(fsm.Tick)) fsmTickList.Remove(fsm.Tick);
        }
    }
}
