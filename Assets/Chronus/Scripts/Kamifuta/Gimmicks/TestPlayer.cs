using Chronus.ChronusGimmick;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamifuta.Testers
{
    public class TestPlayer : MonoBehaviour
    {
        private IInteractableGimmick gimmick;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                gimmick?.Interact(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.TryGetComponent(out gimmick);
            Debug.Log(gimmick);
        }
    }
}

