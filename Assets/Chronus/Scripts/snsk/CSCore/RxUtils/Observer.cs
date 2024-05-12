using System;

namespace RxUtils
{
    internal class Observer<T> : IObserver<T>
    {
        //フィールド
        private readonly Action<T> onNext;
        private readonly Action onCompleted;
        private readonly Action<Exception> onError;



        //コンストラクタ
        public Observer(Action<T> onNext)
        {
            this.onNext = onNext;
        }
        public Observer(Action<T> onNext, Action onCompleted)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
        }
        public Observer(Action<T> onNext, Action<Exception> onError)
        {
            this.onNext = onNext;
            this.onError = onError;
        }
        public Observer(Action<T> onNext, Action onCompleted, Action<Exception> onError)
        {
            this.onNext = onNext;
            this.onCompleted = onCompleted;
            this.onError = onError;
        }



        //各関数の呼び出し
        public void OnNext(T value)
        {
            onNext.Invoke(value);
        }
        public void OnCompleted()
        {
            if(onCompleted != null) onCompleted.Invoke();
        }
        public void OnError(Exception error)
        {
            if(onError != null) onError.Invoke(error);
        }
    }
}
