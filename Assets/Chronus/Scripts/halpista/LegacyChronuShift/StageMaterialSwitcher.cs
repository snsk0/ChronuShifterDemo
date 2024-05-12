using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using UniRx;

namespace Chronus.LegacyChronuShift
{
    public class StageMaterialSwitcher : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;
        GlobalKeyword mossLayerKeyword;

        void SwitchMaterial(bool isPast)
        {
            Shader.SetKeyword(mossLayerKeyword, !isPast);
        }

        void Start()
        {
            mossLayerKeyword = GlobalKeyword.Create("MOSSLAYER_ON");

            chronusManager.isPast.Subscribe(isPast => SwitchMaterial(isPast)).AddTo(this);
        }
    }
}