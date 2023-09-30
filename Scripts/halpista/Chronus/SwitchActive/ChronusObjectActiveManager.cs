using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;

namespace Chronus
{
    public class ChronusObjectActiveManager : MonoBehaviour
    {
        [SerializeField] ChronusManager chronusManager;

        AlwaysObjectDisplayManager[] alwaysObjects;
        TemporaryObjectDisplayManager[] temporaryObjects;
        ChronusObjectDeteriorator[] deterioratableObjects;

        void SwitchObjectsActive(bool isPast)
        {
            foreach(AlwaysObjectDisplayManager objects in alwaysObjects)
            {
                objects.SwitchDisplay(isPast);
            }

            foreach(TemporaryObjectDisplayManager objects in temporaryObjects)
            {
                objects.SwitchDisplay(isPast);
            }

            foreach(ChronusObjectDeteriorator objects in deterioratableObjects)
            {
                objects.SwitchDisplay(isPast);
            }
        }

        void Start()
        {
            alwaysObjects = FindObjectsByType(typeof(AlwaysObjectDisplayManager), FindObjectsSortMode.None) as AlwaysObjectDisplayManager[];
            temporaryObjects = FindObjectsByType(typeof(TemporaryObjectDisplayManager), FindObjectsSortMode.None) as TemporaryObjectDisplayManager[];

            deterioratableObjects = FindObjectsByType(typeof(ChronusObjectDeteriorator), FindObjectsSortMode.None) as ChronusObjectDeteriorator[];

            chronusManager.isPast.Subscribe(isPast => SwitchObjectsActive(isPast)).AddTo(this);
        }
    }
}