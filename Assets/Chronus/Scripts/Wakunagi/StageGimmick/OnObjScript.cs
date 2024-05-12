using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    public class OnObjScript : MonoBehaviour {
        void Start() {
            OnObjListScript.Instance.SetOnObj(this.gameObject);
            Destroy(this);
        }
    }
}