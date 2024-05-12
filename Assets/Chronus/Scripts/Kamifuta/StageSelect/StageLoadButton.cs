using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Chronus.StageSelect
{
    public class StageLoadButton : Button
    {
        public Action onPointerEnterCallback { get; set; }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnterCallback?.Invoke();
        }
    }
}

