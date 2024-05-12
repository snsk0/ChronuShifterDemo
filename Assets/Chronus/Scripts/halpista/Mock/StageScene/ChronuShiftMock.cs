using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Chronus.LegacyChronuShift
{
    public class ChronuShiftMock : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;

        void Start()
        {
            chronusManager.ChangeTime(ObjectChronusType.current);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                bool chronus = chronusManager.ChangeTime();
            }
        }

        void OnTimeShift(InputValue value)
        {
            if (value.isPressed)
                chronusManager.ChangeTime();
        }
    }
}