using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;
using DG.Tweening;

using Chronus;

public class UIClockSwitcherMock : MonoBehaviour
{
    [SerializeField] ChronusManager chronusManager;

    [SerializeField] Image imagefill;

    GameObject seManager;
    private SESingleton SE = null;

    void SwitchUI(bool isPast)
    {
        if(isPast)
            play2();
        else
            play1();
    }

    void Start()
    {
        seManager = GameObject.Find("SEManager");
        if(seManager != null) SE = seManager.GetComponent<SESingleton>();
        
        chronusManager.isPast.Subscribe(isPast => SwitchUI(isPast)).AddTo(this);
    }

    void play1()
    {
        if(SE != null)
        {
            SE.TimeShiftSE();
        }
        var sequence = DOTween.Sequence();
        sequence.Append(imagefill.DOFillAmount(1, 1f)).SetEase(Ease.OutSine);

    }
    
    void play2()
    {
        if(SE != null)
        {
            SE.TimeShiftSE();
        }
        var sequence = DOTween.Sequence();
        sequence.Append(imagefill.DOFillAmount(0, 1f)).SetEase(Ease.OutSine);

    }
}
