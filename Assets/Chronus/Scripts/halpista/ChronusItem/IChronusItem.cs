using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronusItem
{
    public interface IChronusItem
    {
        public bool ToggleCarriedState();
        public void ToggleCarriedState(bool carried);

        public void SetPermanence(bool permanence);
    }
}