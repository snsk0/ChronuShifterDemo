using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageGimmick {
    public class OnObjListScript : OnObjListSingleton<OnObjListScript> {
       [field:SerializeField] public List<GameObject> onObjList { private set; get; } = new List<GameObject>();
        public void SetOnObj(GameObject obj) { onObjList.Add(obj); }
    }
}