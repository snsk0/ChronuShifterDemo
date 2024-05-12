using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Chronus.UI.InGame.ToOut;
using Chronus.UI.InGame;
using System.Collections.Generic;

public class TitleSceneManager : MonoBehaviour
{
    bool checker;
    //[Header("�p�l��")]
    //[SerializeField] GameObject Option;
    //[Header("�^�C�g�����S")]
    [SerializeField] private RectTransform Title;
    [SerializeField] private RectTransform HowTo;
    [SerializeField] private RectTransform Credit;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;
    //[SerializeField] private Button _howToPlayButton;
    //[SerializeField] private Button _backByHowToPlayButton;
    [SerializeField] private Button _creditButton;
    [SerializeField] private Button _backByCreditButton;
    [SerializeField] private Button _optionButton;
    private SESingleton SE;
    private readonly List<Button> _tileButtonList = new List<Button>();

    [SerializeField] private InGameToOutGameUI _optionUI;

    private void Awake()
    {
        _playButton.onClick.AddListener(() => GoGame());
        _exitButton.onClick.AddListener(() => ExitGame());
        //_howToPlayButton.onClick.AddListener(() => GoHowTo());
        //_backByHowToPlayButton.onClick.AddListener(() => DownTitle());
        _optionButton.onClick.AddListener(() => ClickOption());
        _creditButton.onClick.AddListener(() => GoCredit().Forget());
        _backByCreditButton.onClick.AddListener(() => UpTitle().Forget());

        _tileButtonList.Add(_playButton);
        _tileButtonList.Add(_exitButton);
        _tileButtonList.Add(_optionButton);
        _tileButtonList.Add(_creditButton);
        _tileButtonList.Add(_backByCreditButton);

        SE = GameObject.Find("SEManager").GetComponent<SESingleton>();
    }

    void Start()
    {
        //Option.GetComponent<Canvas>().enabled = false;
        checker = true;
    }

    public void SetEnableAllButton(bool isEnable)
    {
        foreach(Button button in _tileButtonList)
        {
            button.interactable = isEnable;
        }
    }

    //public async UniTask GoHowTo()
    //{
    //    SetEnableAllButton(false);

    //    checker = false;
    //    SE.ClickSE();

    //    await UniTask.WhenAll(TitleDoAnchorPos(), HowToDoAnchorPos());

    //    SetEnableAllButton(true);

    //    async UniTask TitleDoAnchorPos()
    //    {
    //        await Title.DOAnchorPos(new Vector2(0, -1500f), 0.5f).SetEase(Ease.OutSine);
    //    }

    //    async UniTask HowToDoAnchorPos()
    //    {
    //        await HowTo.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutSine);
    //    }
    //}

    public async UniTask GoCredit()
    {
        SetEnableAllButton(false);

        checker = false;
        SE.ClickSE();

        await UniTask.WhenAll(TitleDoAnchorPos(), CreditDoAnchorPos());

        SetEnableAllButton(true);

        async UniTask TitleDoAnchorPos()
        {
            await Title.DOAnchorPos(new Vector2(0, 1500f), 0.5f).SetEase(Ease.OutSine);
        }

        async UniTask CreditDoAnchorPos()
        {
            await Credit.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.OutSine);
        }
    }

    //public void DownTitle()
    //{
    //    checker = true;
    //    SE.ClickSE();
    //    HowTo.DOAnchorPos(new Vector2(0, 1500f), 0.5f).SetEase(Ease.OutSine);
    //    Title.DOAnchorPos(new Vector2(0, 0f), 0.5f).SetEase(Ease.OutSine);
    //}

    public async UniTask UpTitle() 
    {
        SetEnableAllButton(false);

        checker = true;
        SE.ClickSE();

        await UniTask.WhenAll(TitleDoAnchorPos(), CreditDoAnchorPos());

        SetEnableAllButton(true);

        async UniTask TitleDoAnchorPos()
        {
            await Title.DOAnchorPos(new Vector2(0, 0f), 0.5f).SetEase(Ease.OutSine);
        }

        async UniTask CreditDoAnchorPos()
        {
            await Credit.DOAnchorPos(new Vector2(0, -1500f), 0.5f).SetEase(Ease.OutSine);
        }
    }
    public void ClickOption()
    {
        SE.ClickSE();
        _optionUI.SetInput(InputType.open);
        //Option.GetComponent<Canvas>().enabled = true;
    }

    //public void CloseOption()
    //{
    //    SE.ClickSE();
    //    //Option.GetComponent<Canvas>().enabled = false;
    //}

    //public void KeyChangeMode()
    //{
    //    SE.ClickSE();
    //    FadeManager.Instance.LoadScene("KeyOption", 0.5f);
    //}

    public void GoGame()
    {
        SetEnableAllButton(false);

        SE.ClickSE();
        FadeManager.Instance.LoadScene("StageSelect", 0.5f);
    }

    public void ExitGame()
    {
        //SE.ClickSE();
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
    #else
        Application.Quit();//ゲームプレイ終了
    #endif
    }
}
