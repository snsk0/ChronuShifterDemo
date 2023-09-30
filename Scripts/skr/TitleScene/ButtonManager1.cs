using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonManager1 : MonoBehaviour
{
    bool checker;
    [Header("�p�l��")]
    [SerializeField] GameObject Option;
    [Header("�^�C�g�����S")]
    [SerializeField] RectTransform Title;
    [SerializeField] RectTransform HowTo;
    [SerializeField] RectTransform Credit;
    private SESingleton SE;
    // Start is called before the first frame update
    void Start()
    {
        SE = GameObject.Find("SEManager").GetComponent<SESingleton>();
        Option.GetComponent<Canvas>().enabled = false;
        checker = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoHowTo()
    {
        checker = false;
        SE.ClickSE();
        Title.DOAnchorPos(new Vector2(0, -1500f), 0.5f).SetEase(Ease.OutSine);
        HowTo.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutSine);
    }

    public void GoCredit()
    {
        checker = false;
        SE.ClickSE();
        Title.DOAnchorPos(new Vector2(0, 1500f), 0.5f).SetEase(Ease.OutSine);
        Credit.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutSine);
    }

    public void DownTitle()
    {
        checker = true;
        SE.ClickSE();
        HowTo.DOAnchorPos(new Vector2(0, 1500f), 0.5f).SetEase(Ease.OutSine);
        Title.DOAnchorPos(new Vector2(0, 0f), 0.5f).SetEase(Ease.OutSine);
    }

    public void UpTitle() 
    {
        checker = true;
        SE.ClickSE();
        Credit.DOAnchorPos(new Vector2(0, -1500f), 0.5f).SetEase(Ease.OutSine);
        Title.DOAnchorPos(new Vector2(0, 0f), 0.5f).SetEase(Ease.OutSine);
    }
    public void ClickOption()
    {
        SE.ClickSE();
        Option.GetComponent<Canvas>().enabled = true;
    }

    public void CloseOption()
    {
        SE.ClickSE();
        Option.GetComponent<Canvas>().enabled = false;
    }

    public void KeyChangeMode()
    {
        SE.ClickSE();
        FadeManager.Instance.LoadScene("KeyOption", 0.5f);
    }

    public void GoGame()
    {
        SE.ClickSE();
        FadeManager.Instance.LoadScene("StageSelect", 0.5f);
    }

    public void ExitGame()
    {
        SE.ClickSE();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
    #else
        Application.Quit();//ゲームプレイ終了
    #endif
    }
}
