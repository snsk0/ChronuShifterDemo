using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace Chronus.ChronusItem
{
    public class DissolveTimeControl : MonoBehaviour, ITimeControl
    {
        [SerializeField] private List<Renderer> renderers;

        private List<Material> materials = new List<Material>();
        private string propertyName = "_DisolveClipping";

        [SerializeField] private float startValue = 1;
        [SerializeField] private float endValue = 0;

        [SerializeField] bool useNum;
        [SerializeField] private float num = 0;

        private void Awake()
        {
            materials.Clear();

            foreach(Renderer r in renderers)
            {
                materials.Add(r.material);
            }
        }

        private void OnDestroy()
        {
            foreach(Material material in materials)
            {
                Destroy(material);
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
            if (!useNum)
            {
                SetDissolveValue((startValue - endValue) * (float)time + endValue);
            }
            else if(num > 0)
            {
                SetDissolveValue(1 - (float)time);
            }
            else if(num < 0)
            {
                SetDissolveValue((float)time);
            }
        }

        private void SetDissolveValue(float value)
        {
            foreach(Material material in materials)
            {
                material.SetFloat(propertyName, value);
            }
        }
    }
}