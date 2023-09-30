using System;

using UnityEngine;
using UniRx;

using InputProviders.LifeCycle;

namespace UnityView.InputProviders
{
    public class UnityUpdateInputProvider : MonoBehaviour, IInputUpdateProvider
    {
        private Subject<RxUtils.Unit> updateSubject = new Subject<RxUtils.Unit>();
        public IObservable<RxUtils.Unit> onUpdate => updateSubject;



        private void Update()
        {
            updateSubject.OnNext(new RxUtils.Unit());
        }


        private void OnDestroy()
        {
            updateSubject.Dispose();
        }
    }
}
