using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeShift : MonoBehaviour
{
    [SerializeField]int count;
    [SerializeField] Image imagefill;
    //    [SerializeField] Image LongN;
    //    [SerializeField] Image ShortN;
    // Start is called before the first frame update
    private SESingleton SE;
    void Start()
    {
        count = 0;
        SE = GameObject.Find("SEManager").GetComponent<SESingleton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && count == 0)
        {
            count++;
            play1();
        }else if(Input.GetKeyDown(KeyCode.Q)&& count == 1)
        {
            count--;
            play2();
        }
    }
    void play1()
    {
        SE.TimeShiftSE();
        var sequence = DOTween.Sequence();
        sequence.Append(imagefill.DOFillAmount(1, 2f));
 //       LongN.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.Fast);
 //       ShortN.transform.DORotate(new Vector3(0,0,360),2f, RotateMode.Fast);
    }
    void play2()
    {
        SE.TimeShiftSE();
        var sequence = DOTween.Sequence();
        sequence.Append(imagefill.DOFillAmount(0, 2f));
//        LongN.transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.Fast);
//        ShortN.transform.DORotate(new Vector3(0, 0, 0), 2f, RotateMode.Fast);
    }
}
