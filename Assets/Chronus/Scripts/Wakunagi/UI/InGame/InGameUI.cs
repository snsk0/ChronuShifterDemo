using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

namespace Chronus.UI.InGame.Interact {
    public class InGameUI : MonoBehaviour {

        /*
        //-----------------シングルトン用-----------------
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
        //-----------------シングルトン用-----------------
        */

        //アイテムを持つときに使う系
        [SerializeField] private InteractionDatabase interactDatabase;
        [SerializeField] private Image interactionUI_image;
        [SerializeField] private Text interactionUI_text;

        //アイテムを持っているときに使う系
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
            //入力情報
            inputScheme = playerInput.currentControlScheme;
            if (inputScheme == "GamePad") interactKey = "A";
            else if (inputScheme == "MouseKeyBoard") interactKey = "F";

            if(isHaving) havingItemMessage_text.text = interactKey + ":" + itemName_message;
        }

        //インタラクション用UIを表示
        public void DisplayInteractionUI(int itemType, int interactionType) {

            interactionUI_text.text = interactKey + ":" +
                interactDatabase.GetItemName(itemType) + interactDatabase.GetInteractionName(interactionType);
            interactionUI_image.gameObject.SetActive(true);
        }
        
        //インタラクション用UIを表示
        public void DisplayInteractionUI_Gimmick(int gimmickType, int interactionType) {

            interactionUI_text.text = interactKey + ":" +
                interactDatabase.GetGimmickName(gimmickType) + interactDatabase.GetInteractionName(interactionType);
            interactionUI_image.gameObject.SetActive(true);
        }


        //インタラクション用UIを非表示
        public void HiddenInteractionUI() {
            interactionUI_image.gameObject.SetActive(false);
            interactionUI_text.text = "";
        }

        //アイテムを持った時の処理
        public void DisplayItemUI(int itemType) {
            itemName_message = interactDatabase.GetItemName(itemType) + "を置く";
            havingItemMessage_text.text = interactKey + ":"+ itemName_message;

            havingItemMessage.gameObject.SetActive(true);
            itemDisplay.gameObject.SetActive(true);  
        }

        //アイテムを置いた時の処理
        public void HiddenItemUI() {
            havingItemMessage.gameObject.SetActive(false);
            itemDisplay.gameObject.SetActive(false);

            itemName_message = string.Empty;
            havingItemMessage_text.text = string.Empty;

        }
    }
}