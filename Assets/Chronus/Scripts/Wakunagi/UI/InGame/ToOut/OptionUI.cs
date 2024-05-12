using UnityEngine;
using UnityEngine.UI;
using System;
using Chronus.Utils;
using static Chronus.UI.InGame.ToOut.OptionUI;

namespace Chronus.UI.InGame.ToOut {
    public class OptionUI : MonoBehaviour {
        /*
        //-----------------シングルトン用-----------------
        private static OptionUI instance;
        public static OptionUI Instance {
            get {
                if (instance == null) {
                    Type t = typeof(OptionUI);
                    instance = (OptionUI)FindObjectOfType(t);

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

        public enum OptionType {
            camera,
            sound,
            user_guide,
            end,
        }
        [SerializeField] InGameToOutGameUI ingameToOutgame;
 
        OptionType selectType = 0;
        bool isSelected = false;
        GameObject[] panels = new GameObject[(int)OptionType.end];
        RectTransform[] btns_pos = new RectTransform[(int)OptionType.end];
        [SerializeField] RectTransform selectArrow;
        [SerializeField] RectTransform selectArrow_inOptionPanel;
        UIStateRecorder recorder;

        //カメラ
        [SerializeField, HeaderAttribute("Camera")] private GameObject camera_panel;
        [SerializeField] private Button camera_selectBtn;
        [SerializeField] private Slider sensitivity_slider;
        [SerializeField] private Toggle reverse_x_tgl, reverse_y_tgl;
        [SerializeField] private RectTransform sensitivity_text, reverse_x_text, reverse_y_text;
        RectTransform[] camera_text_pos = new RectTransform[(int)CameraOption.end];
        public enum CameraOption {
            sensitivity,
            reverse_x,
            reverse_y,
            end,
        }
        CameraOption cameraOption = 0;

        //サウンド
        [SerializeField, HeaderAttribute("Sound")] private GameObject sound_panel;
        [SerializeField] private Button sound_selectBtn;
        [SerializeField] private Slider bgm_slider, se_slider;
        [SerializeField] private RectTransform bgm_text, se_text;
        RectTransform[] sound_text_pos = new RectTransform[(int)SoundOption.end];
        public enum SoundOption {
            bgm_slider,
            se_slider,
            end,
        }
        SoundOption soundOption = 0;


        //操作説明
        [SerializeField, HeaderAttribute("UserGuide")] private GameObject userGude_panel;
        [SerializeField] private Button userGuide_selectBtn;
        [SerializeField] private GameObject disp_keyboard, disp_gamepad;
        [SerializeField] private RectTransform keyboard_text, gamepad_text;
        RectTransform[] userGuide_text_pos = new RectTransform[(int)UserGuideOption.end];
        public enum UserGuideOption {
            keyboard, gamepad,
            end,
        }
        UserGuideOption userGuideOption = 0;

        private void Start() {

            //各パネル
            panels[(int)OptionType.camera] = camera_panel;
            panels[(int)OptionType.sound] = sound_panel;
            panels[(int)OptionType.user_guide] = userGude_panel;
            foreach (GameObject obj in panels) obj.SetActive(false);

            //各ボタンの位置
            btns_pos[(int)OptionType.camera] = camera_selectBtn.GetComponent<RectTransform>();
            btns_pos[(int)OptionType.sound] = sound_selectBtn.GetComponent<RectTransform>();
            btns_pos[(int)OptionType.user_guide] = userGuide_selectBtn.GetComponent<RectTransform>();

            //各文字の位置

            //カメラ
            camera_text_pos[(int)CameraOption.sensitivity] = sensitivity_text;
            camera_text_pos[(int)CameraOption.reverse_x] = reverse_x_text;
            camera_text_pos[(int)CameraOption.reverse_y] = reverse_y_text;

            //サウンド
            sound_text_pos[(int)SoundOption.bgm_slider] = bgm_text;
            sound_text_pos[(int)SoundOption.se_slider] = se_text;

            //ガイド
            userGuide_text_pos[(int)UserGuideOption.keyboard] = keyboard_text;
            userGuide_text_pos[(int)UserGuideOption.gamepad] = gamepad_text;

            LoadOptionStatus();
        }

        void LoadOptionStatus() {
            recorder = UIStateRecorder.Instance;
            sensitivity_slider.value = recorder.sensitivity;
            reverse_x_tgl.interactable = recorder.isRevers_x;
            reverse_y_tgl.interactable = recorder.isRevers_y;

            bgm_slider.value = recorder.bgm_volume;
            se_slider.value = recorder.se_volume;
        }

        public void OpenOption() {
            Vector3 buttonPos = btns_pos[(int)selectType].position;
            selectArrow.position = buttonPos;
        }

        //入力を受け取る
        public void SetInput(InputType type) {

            selectArrow_inOptionPanel.gameObject.SetActive(true);

            if (GetInputType.IsInputBack(type) || GetInputType.IsInputOpen(type)) { OnClick_Back(); return; }

            selectArrow_inOptionPanel.gameObject.SetActive(true);
            selectArrow.gameObject.SetActive(true);

            //各オプションを実行中
            if (isSelected) {
                switch (selectType) {
                    case OptionType.camera: CameraSettingChanger(type); break;
                    case OptionType.sound: SoundSettingChanger(type); break;
                    case OptionType.user_guide: UserGuideSettingChanger(type); break;
                    default: break;
                }
            }

            //オプション自体を選び中
            else {
                if (GetInputType.IsInputArrow(type)) { SelectChange(type); return; }
                if (GetInputType.IsInputDecision(type)) { Decision(); return; }
            }

            Vector3 buttonPos = btns_pos[(int)selectType].position;
            selectArrow.position = buttonPos;

        }

        //選択
        void SelectChange(InputType type) {
            int change = GetInputType.IsInputUpDown(type);

            if (change == 0) return;
            if (change < 0 && selectType == 0) return;
            if (change > 0 && selectType == OptionType.end - 1) return;

            selectType += change;
            Vector3 buttonPos = btns_pos[(int)selectType].position;
            selectArrow.position = buttonPos;

        }

        //決定
        void Decision() {
            foreach (GameObject obj in panels) obj.SetActive(false);

            selectArrow_inOptionPanel.gameObject.transform.SetParent(panels[(int)selectType].transform);
            RectTransform rect = new RectTransform();
            switch (selectType) {
                case OptionType.camera: rect = camera_text_pos[(int)cameraOption]; break;
                case OptionType.sound: rect = sound_text_pos[(int)soundOption]; break;
                case OptionType.user_guide: rect = userGuide_text_pos[(int)userGuideOption]; break;
                default: break;
            }
            if (rect != null) {
                Vector3 buttonPos = rect.position;
                selectArrow_inOptionPanel.position = buttonPos;
                panels[(int)selectType].SetActive(true);
            }

            isSelected = true;
            return;
        }

        //各ボタンの決定
        public void OnClick_Camera() {
            OnClick(OptionType.camera);
        }
        public void OnClick_Sound() {
            OnClick(OptionType.sound);
        }
        public void OnClick_UserGuide() {
            OnClick(OptionType.user_guide);
        }
        void OnClick(OptionType type) {
            selectType = type;
            selectArrow_inOptionPanel.gameObject.SetActive(false);
            selectArrow.gameObject.SetActive(false);
            Decision();
        }

        //終了処理
        void OnClick_Back() {

            //各オプションを実行中
            if (isSelected) {
                foreach (GameObject obj in panels) obj.SetActive(false);
                Vector3 buttonPos = btns_pos[(int)selectType].position;
                selectArrow.position = buttonPos;
                isSelected = false;
                return;
            }

            //オプション自体を選び中
            else {
                selectType = OptionType.camera;
                //InGameToOutGameUI.Instance.OnClick_OpenOption();
                ingameToOutgame.OnClick_OpenOption();
                return;
            }
        }

        void CameraSettingChanger(InputType type) {
            /*
            
            <UIの配置>

            カメラ感度
            [----[]----]

            反転
            X軸 [ ]
            Y軸 [ ]

            */

            bool isDirection = GetInputType.IsInputDecision(type);
            int change_x = GetInputType.IsInputRightLeft(type);
            int change_y = GetInputType.IsInputUpDown(type);

            if (!isDirection && change_x == 0 && change_y == 0) return;

            if (isDirection) {

                //x軸反転処理
                if (cameraOption == CameraOption.reverse_x) {
                    reverse_x_tgl.interactable = !reverse_x_tgl.interactable;
                    SetReverse_x();
                }

                //y軸反転処理
                if (cameraOption == CameraOption.reverse_y) {
                    reverse_y_tgl.interactable = !reverse_y_tgl.interactable;
                    SetReverse_y();
                }

                return;

            }
            if (change_x != 0) {

                //感度処理
                if (cameraOption == CameraOption.sensitivity) {
                    sensitivity_slider.value += change_x;
                    SetSensitivitySlider();
                }
                return;
            }

            if (change_y != 0) {
                if (change_y < 0 && cameraOption == 0) return;
                if (change_y > 0 && cameraOption == CameraOption.end - 1) return;
                cameraOption += change_y;

                selectArrow_inOptionPanel.gameObject.transform.SetParent(camera_panel.transform);
                Vector3 buttonPos = camera_text_pos[(int)cameraOption].position;
                selectArrow_inOptionPanel.position = buttonPos;

                return;

            }

        }

        public void SetSensitivitySlider() {
            cameraOption = CameraOption.sensitivity;
            sensitivity_slider.value = (int)sensitivity_slider.value;
            recorder.SetOptionStatus_Sensitivity((int)sensitivity_slider.value);
        }
        public void SetReverse_x() {
            cameraOption = CameraOption.reverse_x;
            recorder.SetOptionStatus_Revers_x(reverse_x_tgl.interactable);
        }
        public void SetReverse_y() {
            cameraOption = CameraOption.reverse_y;
            recorder.SetOptionStatus_Revers_y(reverse_y_tgl.interactable);
        }


        void SoundSettingChanger(InputType type) {
            /*
            
            <UIの配置>

            BGM
            [----[]----]
            
            SE
            [----[]----]

            */

            int change_x = GetInputType.IsInputRightLeft(type);
            int change_y = GetInputType.IsInputUpDown(type);

            if (change_x == 0 && change_y == 0) return;

            if (change_x != 0) {

                //BGM処理
                if (soundOption == SoundOption.bgm_slider) {
                    bgm_slider.value += change_x;
                    SetBGMSlider();
                }

                //SE処理
                if (soundOption == SoundOption.se_slider) {
                    se_slider.value += change_x;
                    SetSESlider();
                }

                return;
            }

            if (change_y != 0) {
                if (change_y < 0 && soundOption == 0) return;
                if (change_y > 0 && soundOption == SoundOption.end - 1) return;
                soundOption += change_y;

                selectArrow_inOptionPanel.gameObject.transform.SetParent(sound_panel.transform);
                Vector3 buttonPos = sound_text_pos[(int)soundOption].position;
                selectArrow_inOptionPanel.position = buttonPos;

                return;

            }


        }
        public void SetBGMSlider() {
            soundOption = SoundOption.bgm_slider;
            bgm_slider.value = (int)bgm_slider.value;
            AudioVolumeManager.SetVolume(bgm_slider.value / bgm_slider.maxValue, Utils.AudioType.BackGoundMusic);
            recorder.SetOptionStatus_BGm((int)bgm_slider.value);
        }
        public void SetSESlider() {
            soundOption = SoundOption.se_slider;
            se_slider.value = (int)se_slider.value;
            AudioVolumeManager.SetVolume(se_slider.value / se_slider.maxValue, Utils.AudioType.SoundEffect);
            recorder.SetOptionStatus_SE((int)se_slider.value);
        }

        void UserGuideSettingChanger(InputType type) {
            /*
            
            <UIの配置>

            [KeyBord] [GamePad]
            +------------------+
            |                  |
            |                  |
            |                  |
            +------------------+

            */

            int change_x = GetInputType.IsInputRightLeft(type);

            if (change_x == 0) return;

            if (change_x < 0 && userGuideOption == 0) return;
            if (change_x > 0 && userGuideOption == UserGuideOption.end - 1) return;
            userGuideOption += change_x;

            selectArrow_inOptionPanel.gameObject.transform.SetParent(userGude_panel.transform);
            Vector3 buttonPos = userGuide_text_pos[(int)userGuideOption].position;
            selectArrow_inOptionPanel.position = buttonPos;

            disp_keyboard.gameObject.SetActive(false);
            disp_gamepad.gameObject.SetActive(false);
            if (userGuideOption == UserGuideOption.keyboard) disp_keyboard.gameObject.SetActive(true);
            if (userGuideOption == UserGuideOption.gamepad) disp_gamepad.gameObject.SetActive(true);

            return;
        }

        public void OnClick_Display_KeyboardGuide() {
            disp_keyboard.gameObject.SetActive(true);
            disp_gamepad.gameObject.SetActive(false);

            userGuideOption = UserGuideOption.keyboard;

            selectArrow_inOptionPanel.gameObject.SetActive(false);
            selectArrow.gameObject.SetActive(false);
        }
        public void OnClick_Display_GamepadGuide() {
            disp_keyboard.gameObject.SetActive(false);
            disp_gamepad.gameObject.SetActive(true);

            userGuideOption = UserGuideOption.gamepad;

            selectArrow_inOptionPanel.gameObject.SetActive(false);
            selectArrow.gameObject.SetActive(false);
        }

    }

}