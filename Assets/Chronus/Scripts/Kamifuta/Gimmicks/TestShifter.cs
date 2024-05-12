using Chronus.ChronuShift;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kamifuta.Testers
{
    public class TestShifter : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChronusStateManager.Instance.ChronuShift();
            }
        }
    }
}

