using Chronus.UI.InGame.ToOut;
using UnityEngine;
using System;

namespace Chronus.UI.InGame {
    public class UIInputManager : MonoBehaviour {

        /*
        //-----------------�V���O���g���p-----------------
        private static UIInputManager instance;
        public static UIInputManager Instance {
            get {
                if (instance == null) {
                    Type t = typeof(UIInputManager);
                    instance = (UIInputManager)FindObjectOfType(t);

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
        //-----------------�V���O���g���p-----------------
        */

        [SerializeField]InGameToOutGameUI inToOutGameUI;

        //InputSystem�n
        UIController ui_inputSystem;
        Vector2 input_navigater, oldInput_navigater;


        void Start() {
            //inToOutGameUI = InGameToOutGameUI.Instance;

            ui_inputSystem = new UIController();
            ui_inputSystem.Enable();

        }

        void Update() {

            GetInput();
        }


        void GetInput() {

            //�㉺�̓���
            input_navigater = ui_inputSystem.UI.Navigate.ReadValue<Vector2>();
            if (input_navigater != oldInput_navigater) {
                if (input_navigater.x >= 1) inToOutGameUI.SetInput(InputType.right);
                if (input_navigater.x <= -1) inToOutGameUI.SetInput(InputType.left);
                if (input_navigater.y >= 1) inToOutGameUI.SetInput(InputType.up);
                if (input_navigater.y <= -1) inToOutGameUI.SetInput(InputType.down);
            }

            //���j���[���J�����茈�肵����
            if (ui_inputSystem.UI.Escape.triggered) inToOutGameUI.SetInput(InputType.open);
            if (ui_inputSystem.UI.Decision.triggered) inToOutGameUI.SetInput(InputType.decision);
            if (ui_inputSystem.UI.Back.triggered) inToOutGameUI.SetInput(InputType.back);

            oldInput_navigater = input_navigater;
        }

    }
}