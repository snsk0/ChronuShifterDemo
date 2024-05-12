using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.StageSelect
{
    public static class UnlockedStageManager
    {
        private static readonly HashSet<StageType> unlockedStages = new HashSet<StageType>() { StageType.Stage1 };
        public static IEnumerable<StageType> UnlockedStages => unlockedStages;

        public static void Unlock(StageType stageType)
        {
            unlockedStages.Add(stageType);
        }
    }
}

