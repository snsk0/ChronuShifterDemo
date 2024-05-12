using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeShift : MonoBehaviour
{
    [SerializeField]int count;
    [SerializeField] Image imagefill;
    [SerializeField] Text CurrentText;//現代表示のテキスト
    [SerializeField] Text PastText;//過去表示のテキスト
    //    [SerializeField] Image LongN;
    //    [SerializeField] Image ShortN;
    // Start is called before the first frame update
    private SESingleton SE;
    void Start()
    {
        count = 0;
        SE = GameObject.Find("SEManager").GetComponent<SESingleton>();
        PastText.rectTransform.anchoredPosition = new Vector2(100, 0);
        CurrentText.rectTransform.anchoredPosition = new Vector2(-20, 0);
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
        sequence.Append(imagefill.DOFillAmount(0, 1f)).SetEase(Ease.OutSine);
        sequence.Join(CurrentText.rectTransform.DOLocalMoveX(100,1f).SetEase(Ease.OutSine));
        sequence.Join(PastText.rectTransform.DOLocalMoveX(-20,1f).SetEase(Ease.OutSine));
        //       LongN.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.Fast);
        //       ShortN.transform.DORotate(new Vector3(0,0,360),2f, RotateMode.Fast);
    }
    void play2()
    {
        SE.TimeShiftSE();
        var sequence = DOTween.Sequence();
        sequence.Append(imagefill.DOFillAmount(1, 1f)).SetEase(Ease.OutSine);
        sequence.Join(CurrentText.rectTransform.DOLocalMoveX(-20, 1f).SetEase(Ease.OutSine));
        sequence.Join(PastText.rectTransform.DOLocalMoveX(100, 1f).SetEase(Ease.OutSine));
        //        LongN.transform.DORotate(new Vector3(0, 0, 0), 1f, RotateMode.Fast);
        //        ShortN.transform.DORotate(new Vector3(0, 0, 0), 2f, RotateMode.Fast);
    }
}
