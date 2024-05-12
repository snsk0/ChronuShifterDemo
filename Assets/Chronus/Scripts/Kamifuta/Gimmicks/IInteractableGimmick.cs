using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronusGimmick
{
    public interface IInteractableGimmick
    {
        public bool IsInteractable { get; }
        public bool IsInteracting { get; }

        public void Interact<T>(T interactor) where T : MonoBehaviour;
    }
}

