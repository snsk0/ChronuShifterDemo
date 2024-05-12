using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Chronus.Gimmick
{
    public class TreeLeavesTimeControl : MonoBehaviour, ITimeControl
    {
        [SerializeField] private List<Renderer> renderers;

        private Material material;
        // private string propertyKeyword = "_Dither";
        private string propertyName = "_DitherPower";

        [SerializeField] private float num = 0;

        private void Awake()
        {
            if(material == null)
            {
                material = new Material(renderers[0].sharedMaterial);
                foreach (Renderer r in renderers)
                {
                    r.material = material;
                }
            }
        }

        public void OnControlTimeStart()
        {
        }

        public void OnControlTimeStop()
        {
        }

        public void SetTime(double time)
        {
            if(num < 0)
            {
                material.SetFloat(propertyName, 1 - (float)time);
            }
            else if(num > 0)
            {
                material.SetFloat(propertyName, (float)time);
            }
        }
    }
}