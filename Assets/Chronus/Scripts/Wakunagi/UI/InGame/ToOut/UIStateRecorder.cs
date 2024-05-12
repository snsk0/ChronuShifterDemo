using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Chronus.UI.InGame.ToOut {
    public class UIStateRecorder : MonoBehaviour {

        //-----------------シングルトン用-----------------
        private static UIStateRecorder instance;
        public static UIStateRecorder Instance {
            get {
                if (instance == null) {
                    Type t = typeof(UIStateRecorder);
                    instance = (UIStateRecorder)FindObjectOfType(t);

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

        [field: SerializeField] public int sensitivity { private set; get; }
        [field: SerializeField] public bool isRevers_x { private set; get; }
        [field: SerializeField] public bool isRevers_y { private set; get; }

        [field: SerializeField] public int bgm_volume { private set; get; }
        [field: SerializeField] public int se_volume { private set; get; }



        void Start() {
            DontDestroyOnLoad(gameObject);
        }

        public void SetOptionStatus_Sensitivity(int _sensitivity) { sensitivity = _sensitivity; }
        public void SetOptionStatus_Revers_x(bool _revers_x) { isRevers_x = _revers_x; }
        public void SetOptionStatus_Revers_y(bool _revers_y) { isRevers_y = _revers_y; }

        public void SetOptionStatus_BGm(int _bgm) { bgm_volume = _bgm; }
        public void SetOptionStatus_SE(int _se) { se_volume = _se; }

    }
}