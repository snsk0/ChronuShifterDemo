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




    //generic�e�X�g�N���X
    //�X�e�[�g�x�[�X
    private class Generic<T,S>
    {
        public T param;
    }
    //���ۂ̃X�e�[�g,S����v�����Ȃ���΂����Ȃ��̂��������炢,�ł��p���Ȃ炱���Ȃ�͓̂�����O(generic���g��Ȃ���΂Ȃ�Ȃ�����
    private class GenericEx<S> : Generic<GenericOw<S>, S>
    {

    }
    //�R���e�L�X�g
    private class GenericOw<S>
    {
        public Generic<GenericOw<S>, S> generic;
    }


    private interface IBase { }
    private class Base { }
    private class Extend : Base, IBase { }
}
