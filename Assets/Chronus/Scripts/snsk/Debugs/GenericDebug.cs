using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Generic<Base, int> generic = new Generic<Base, int>();
        generic.param = new Extend();
    }

    // Update is called once per frame
    void Update()
    {
        
    }




    //genericテストクラス
    //ステートベース
    private class Generic<T,S>
    {
        public T param;
    }
    //実際のステート,Sを一致させなければいけないのが書きずらい,でも継承ならこうなるのは当たり前(genericを使わなければならないため
    private class GenericEx<S> : Generic<GenericOw<S>, S>
    {

    }
    //コンテキスト
    private class GenericOw<S>
    {
        public Generic<GenericOw<S>, S> generic;
    }


    private interface IBase { }
    private class Base { }
    private class Extend : Base, IBase { }
}
