using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

namespace Chronus.UI.InGame.Interact {
    public class InGameUI : MonoBehaviour {

        /*
        //-----------------�V���O���g���p-----------------
        private static InGameUI instance;
        public static InGameUI Instance {
            get {
                if (instance == null) {
                    Type t = typeof(InGameUI);
                    instance = (InGameUI)FindObjectOfType(t);

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

        //�A�C�e�������Ƃ��Ɏg���n
        [SerializeField] private InteractionDatabase interactDatabase;
        [SerializeField] private Image interactionUI_image;
        [SerializeField] private Text interactionUI_text;

        //�A�C�e���������Ă���Ƃ��Ɏg���n
        [SerializeField] private GameObject itemDisplay;
        [SerializeField] private GameObject havingItemMessage;
        [SerializeField] private Text havingItemMessage_text;
        bool isHaving = false;
        string itemName_message = string.Empty;

        //InputSystem
        [field: SerializeField] public PlayerInput playerInput { private set; get; }
        public string inputScheme { private set; get; } = "";
        string interactKey = "";

        void Update() {
            //���͏��
            inputScheme = playerInput.currentControlScheme;
            if (inputScheme == "GamePad") interactKey = "A";
            else if (inputScheme == "MouseKeyBoard") interactKey = "F";

            if(isHaving) havingItemMessage_text.text = interactKey + ":" + itemName_message;
        }

        //�C���^���N�V�����pUI��\��
        public void DisplayInteractionUI(int itemType, int interactionType) {

            interactionUI_text.text = interactKey + ":" +
                interactDatabase.GetItemName(itemType) + interactDatabase.GetInteractionName(interactionType);
            interactionUI_image.gameObject.SetActive(true);
        }
        
        //�C���^���N�V�����pUI��\��
        public void DisplayInteractionUI_Gimmick(int gimmickType, int interactionType) {

            interactionUI_text.text = interactKey + ":" +
                interactDatabase.GetGimmickName(gimmickType) + interactDatabase.GetInteractionName(interactionType);
            interactionUI_image.gameObject.SetActive(true);
        }


        //�C���^���N�V�����pUI���\��
        public void HiddenInteractionUI() {
            interactionUI_image.gameObject.SetActive(false);
            interactionUI_text.text = "";
        }

        //�A�C�e�������������̏���
        public void DisplayItemUI(int itemType) {
            itemName_message = interactDatabase.GetItemName(itemType) + "��u��";
            havingItemMessage_text.text = interactKey + ":"+ itemName_message;

            havingItemMessage.gameObject.SetActive(true);
            itemDisplay.gameObject.SetActive(true);  
        }

        //�A�C�e����u�������̏���
        public void HiddenItemUI() {
            havingItemMessage.gameObject.SetActive(false);
            itemDisplay.gameObject.SetActive(false);

            itemName_message = string.Empty;
            havingItemMessage_text.text = string.Empty;

        }
    }
}