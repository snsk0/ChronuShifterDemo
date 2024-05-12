using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.LegacyChronuShift
{
    interface IChronusObject
    {
        public ObjectChronusType chronusType{ get; set;}

        public void ToggleCarriedState();
        public void ToggleCarriedState(bool carried);
    }
}