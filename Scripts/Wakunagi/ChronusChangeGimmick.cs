using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

using Chronus;


public class ChronusChangeGimmick : MonoBehaviour
{
    [SerializeField] ChronusManager chronusManager;

    void Start() {
        chronusManager.isPast.Subscribe(isPast => ChangeGimmick(isPast)).AddTo(this);
    }

    protected virtual void VirtualStart() { }

    protected virtual void ChangeGimmick(bool isPast) { }

}