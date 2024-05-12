using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronuShift
{
    public abstract class AbstractChronusTargetFactory : MonoBehaviour
    {
        private ChronusTargetManager targetManager;

        protected abstract IChronusTarget CreateChronusTarget();
        public IChronusTarget Create()
        {
            IChronusTarget target = CreateChronusTarget();
            targetManager.AddChronusTarget(target);

            return target;
        }


        protected void SetTargetManager(ChronusTargetManager chronusTargetManager)
        {
            targetManager = chronusTargetManager;
        }
    }
}