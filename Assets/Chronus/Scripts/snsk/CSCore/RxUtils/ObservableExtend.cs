using System;

namespace RxUtils
{
    public static class ObservableExtend
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext)
        {
            return observable.Subscribe(new Observer<T>(onNext));
        }
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, Action onCompleted)
        {
            return observable.Subscribe(new Observer<T>(onNext, onCompleted));
        }
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, Action<Exception> onError)
        {
            return observable.Subscribe(new Observer<T>(onNext, onError));
        }
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, Action onCompleted, Action<Exception> onError)
        {
            return observable.Subscribe(new Observer<T>(onNext, onCompleted, onError));
        }
    }
}
