using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.Respawn
{
    public class WayPointDataMock : MonoBehaviour
    {
        [SerializeField] public Transform[] wayPointData;
        [SerializeField] public int wayPointNumber = 0;
    }
}