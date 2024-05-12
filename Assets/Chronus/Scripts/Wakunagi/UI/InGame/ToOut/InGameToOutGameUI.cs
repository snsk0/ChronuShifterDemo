using Chronus.UI.InGame.Interact;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.Mathematics;
using Chronus.Utils;
using UnityView.InputProviders;
using UnityEngine.InputSystem.UI;

namespace Chronus.UI.InGame.ToOut {
    public class InGameToOutGameUI : MonoBehaviour {
        /*
        //-----------------シングルトン用-----------------
        private static InGameToOutGameUI instance;
        public static InGameToOutGameUI Instance {
            get {
                if (instance == null) {
                    Type t = typeof(InGameToOutGameUI);
                    instance = (InGameToOutGameUI)FindObjectOfType(t);

                    if (instance == null) {
                        Debug.LogError(t + " is not found.");
                    }
                }
                return instance;
            }
        }

        void Awake() {
            if (instance == null) { instance = this; }
            else if (Instance == this) { }
            else Destroy(this);
        }
        //-----------------シングルトン用-----------------
        */

        //メニュー内のボタンの種類
        public enum SelectButton {
            exit,
            retry,
            retry_middle,
            select_stage,
            option,
            gameend,
            end,
        }


        float t = 0;

        [SerializeField, HeaderAttribute("OutGameUI用か?")] private bool isOutGame = false;

        SelectButton selecting = SelectButton.retry;
        bool isOpenNow = false;
        [SerializeField] private OptionUI optionUI;
        [SerializeField] private GameObject menu_Panel;
        [SerializeField] private string selectScene;
        [SerializeField] private GameObject option_Panel;

        [SerializeField] private RectTransform exit_Button, retry_Button, retry_middle_Button, select_Button, option_Button, gameend_Button;
        [field: SerializeField] public RectTransform selectArrow { private set; get; }
        [SerializeField] private float selectArrow_timeSpeed = 10;
        Image selectArrow_Image = null;
        List<RectTransform> selectButtonList = new List<RectTransform>();

        [SerializeField] UnityInputSystemPlayerInputProvider uispip;

        bool isOption = false;

        private void Start() {

            for (int i = 0; i < (int)SelectButton.end; i++) {
                selectButtonList.Add(null);
            }
            selectButtonList[(int)SelectButton.exit] = exit_Button;
            selectButtonList[(int)SelectButton.retry] = retry_Button;
            selectButtonList[(int)SelectButton.retry_middle] = retry_middle_Button;
            selectButtonList[(int)SelectButton.select_stage] = select_Button;
            selectButtonList[(int)SelectButton.option] = option_Button;
            selectButtonList[(int)SelectButton.gameend] = gameend_Button;
            selectArrow_Image = selectArrow.GetComponent<Image>();
            if (selectArrow_Image == null) Debug.LogError("SelectArrow does not have Image.");

            if (isOutGame) {
                selectButtonList[(int)SelectButton.retry].gameObject.SetActive(false);
                selectButtonList[(int)SelectButton.retry_middle].gameObject.SetActive(false);
                selectButtonList[(int)SelectButton.select_stage].gameObject.SetActive(false);
                selecting = SelectButton.option;
            }

            option_Panel.gameObject.SetActive(false);
        }

        private void Update() {
            t += Time.unscaledDeltaTime * selectArrow_timeSpeed;
            if (t > 2 * math.PI) t = 0;
            selectArrow_Image.color = new Color(1, 1, 1, (math.cos(t) + 1) / 2);
        }

        //入力判定
        public void SetInput(InputType type) {
            selectArrow.gameObject.SetActive(true);

            if (isOption) {
                // OptionUI.Instance.SetInput(type);
                optionUI.SetInput(type);
                return;
            }

            if (!isOpenNow) {
                if (GetInputType.IsInputOpen(type)) { OpenMenu(); return; }
            }

            else {
                if (GetInputType.IsInputBack(type) || GetInputType.IsInputOpen(type)) { CloseMenu(); return; }
                if (GetInputType.IsInputDecision(type)) { SelectDecision(); return; }

                int change = GetInputType.IsInputUpDown(type);
                if (change != 0) { SelectChange(change); return; }
            }

        }

        //メニューを開く
        public void OpenMenu() {
            Time.timeScale = 0;
            if (!isOutGame) uispip.SetInputActive(false);
            if (!isOutGame) MouseCusorController.SetCursolActive(true);
            selecting = SelectButton.retry;
            if(isOutGame) selecting = SelectButton.option;

            //タイトル用追加コード
            if (isOutGame)
            {
                var manager = FindObjectOfType<TitleSceneManager>();
                if(manager != null)
                {
                    manager.SetEnableAllButton(false);
                }
            }

            option_Panel.gameObject.SetActive(false);
            SetPosSelectArrow();

            isOpenNow = true;
            menu_Panel.SetActive(true);

        }

        //メニューを閉じる
        public void CloseMenu() {
            isOpenNow = false;
            isOption = false;
            if (!isOutGame) uispip.SetInputActive(true);
            if (!isOutGame) MouseCusorController.SetCursolActive(false);
            Time.timeScale = 1.0f;
            menu_Panel.SetActive(false);

            //タイトル用追加コード
            if (isOutGame)
            {
                var manager = FindObjectOfType<TitleSceneManager>();
                if (manager != null)
                {
                    manager.SetEnableAllButton(true);
                }
            }
        }

        //選択の変更
        public void SelectChange(int delta) {
            Debug.Log("Selecting ");

            if (!((int)selecting == 0 && delta < 0)
             && !(selecting == SelectButton.end - 1 && delta > 0)) {

                if (isOutGame) {
                    Debug.Log("Selecting 1 : " + selecting);
                    if (selecting == SelectButton.exit && delta > 0) selecting = SelectButton.option;
                    else
                    if (selecting == SelectButton.option && delta < 0) selecting = SelectButton.exit;
                    else selecting += delta;
                    Debug.Log("Selecting 2 : " + selecting);
                }
                else selecting += delta;
                SetPosSelectArrow();
            }
        }

        //selectArrowの位置の変更
        void SetPosSelectArrow() {
            Vector3 buttonPos = selectButtonList[(int)selecting].position;
            selectArrow.position = buttonPos;
        }

        //選択の決定
        public void SelectDecision() {
            switch (selecting) {
                case SelectButton.exit: CloseMenu(); break;
                case SelectButton.retry: OnClick_Retry(); break;
                case SelectButton.retry_middle: OnClick_Retry_Middle(); break;
                case SelectButton.select_stage: OnClick_GoStageSelect(); break;
                case SelectButton.option: OnClick_OpenOption(); break;
                case SelectButton.gameend: OnClick_GameEnd(); break;
                default: Debug.LogError("Menu is Out."); break;
            }
        }

        //リトライ
        public void OnClick_Retry() {
            SetPosSelectArrow();
            selecting = SelectButton.retry;

            CloseMenu();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //途中から
        public void OnClick_Retry_Middle() {
            SetPosSelectArrow();
            selecting = SelectButton.retry_middle;

            CloseMenu();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //ステージセレクト
        public void OnClick_GoStageSelect() {
            SetPosSelectArrow();
            selecting = SelectButton.select_stage;

            CloseMenu();

            SceneManager.LoadScene(selectScene);
        }

        //オプション
        public void OnClick_OpenOption() {
            SetPosSelectArrow();
            selecting = SelectButton.option;
            isOption = !isOption;
            if (isOption) {
                // OptionUI.Instance.OpenOption();
                optionUI.OpenOption();
            }
            else {
                Vector3 buttonPos = selectButtonList[(int)selecting].position;
                selectArrow.position = buttonPos;
            }
            option_Panel.SetActive(isOption);
        }

        //終了
        public void OnClick_GameEnd() {
            SetPosSelectArrow();
            selecting = SelectButton.gameend;

            CloseMenu();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}