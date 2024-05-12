using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.Respawn
{
    public class WayPoint : MonoBehaviour
    {
        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        }
        #endif
    }
}
