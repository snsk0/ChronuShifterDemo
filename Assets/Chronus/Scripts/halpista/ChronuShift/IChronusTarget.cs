using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronuShift
{
    public interface IChronusTarget
    {
        public void OnShift(ChronusState state);
    }
}