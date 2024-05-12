using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class delegateDebug : MonoBehaviour
{
    void Start()
    {
        //メソッドをアクション型に代入して果たしてアドレス一致するのか
        TickTest tickTest = new TickTest();     //1つのTickTest
        Action action1 = tickTest.Tick;
        Action action2 = tickTest.Tick;


        //equalが呼ばれる
        if(action1 == action2)
        {
            Debug.Log("equal");
        }
        else
        {
            Debug.Log("Not");
        }




        //リストはどうなるか検証
        //しっかりcontain呼ばれる
        List<Action> list = new List<Action>();
        list.Add(tickTest.Tick);
        if (list.Contains(tickTest.Tick))
        {
            Debug.Log("Contain");
        }
    }



    //FSMのTickに見立てる
    private class TickTest
    {
        public void Tick()
        {
            Debug.Log("Tick");
        }
    }
}
