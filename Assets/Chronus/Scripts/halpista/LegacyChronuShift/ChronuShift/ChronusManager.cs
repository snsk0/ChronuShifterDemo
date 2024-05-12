using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Chronus.LegacyChronuShift
{
    public class ChronusManager : MonoBehaviour, IChronusChanger{
        public ReactiveProperty<bool> isPast = new ReactiveProperty<bool>(false);

        public bool ChangeTime(){
            isPast.Value = !isPast.Value;
            return isPast.Value;
        }

        public bool ChangeTime(ObjectChronusType type)
        {
            if(type == ObjectChronusType.past)
            {
                isPast.Value = true;
                return isPast.Value;
            }
            else
            {
                isPast.Value = false;
                return isPast.Value;
            }
        }
    }
}