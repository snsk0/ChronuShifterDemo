using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyChangeText : MonoBehaviour
{
    [SerializeField] Text Forward;
    [SerializeField] Text Backward;
    [SerializeField] Text Left;
    [SerializeField] Text Right;
    [SerializeField] Text Dash;
    [SerializeField] Text Jump;
    [SerializeField] Text Interact;
    [SerializeField] Text Timeshift;
    [SerializeField] Text Pause;
    char Command;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    enum Key//�L�[�{�[�h��
    {
        up, down, left, right,//�㉺���E
        a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,//Alphabet
        one,two,three,four,five,six,seven,seige,eight,nine,zero,//����
        Lshift,Rshift,Return,Tab,Esc//����L�[
    }
    enum Controller
    {
        Lstick,Rstick,
        a,b,x,y,
        Up,Down,Left,Right,
        L,R,LB,RB,
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForwardClick()
    {
        Forward.text = "Press Any key";
    }
}
