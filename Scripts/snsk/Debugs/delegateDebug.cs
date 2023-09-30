using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class delegateDebug : MonoBehaviour
{
    void Start()
    {
        //���\�b�h���A�N�V�����^�ɑ�����ĉʂ����ăA�h���X��v����̂�
        TickTest tickTest = new TickTest();     //1��TickTest
        Action action1 = tickTest.Tick;
        Action action2 = tickTest.Tick;


        //equal���Ă΂��
        if(action1 == action2)
        {
            Debug.Log("equal");
        }
        else
        {
            Debug.Log("Not");
        }




        //���X�g�͂ǂ��Ȃ邩����
        //��������contain�Ă΂��
        List<Action> list = new List<Action>();
        list.Add(tickTest.Tick);
        if (list.Contains(tickTest.Tick))
        {
            Debug.Log("Contain");
        }
    }



    //FSM��Tick�Ɍ����Ă�
    private class TickTest
    {
        public void Tick()
        {
            Debug.Log("Tick");
        }
    }
}
