using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Chronus.LegacyChronuShift
{
    public class LegacyGrowingTree : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;

        [SerializeField] LegacyWaterFeature waterFeature;

        [SerializeField] GameObject pastSeedlings;
        [SerializeField] GameObject currentTree;
        [SerializeField] GameObject currentDeadTree;

        void ChangeTree(bool isPast)
        {
            if (isPast)
            {
                pastSeedlings.SetActive(true);
                currentTree.SetActive(false);
                currentDeadTree.SetActive(false);
            }
            else
            {
                pastSeedlings.SetActive(false);

                if (waterFeature.isWatered)
                {
                    currentTree.SetActive(true);
                }
                else
                {
                    currentDeadTree.SetActive(true);
                }
            }
        }

        void Start()
        {
            chronusManager.isPast.Subscribe(isPast => ChangeTree(isPast)).AddTo(this);
        }
    }
}