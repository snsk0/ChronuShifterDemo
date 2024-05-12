using UnityEngine;

namespace Chronus.Stage
{
    public class GroundData : MonoBehaviour
    {
        [SerializeField] private Renderer groundRenderer;
        private Material material;

        private void Awake()
        {
            material = groundRenderer.sharedMaterial;
        }

        public Renderer GetGroundRenderer()
        {
            return groundRenderer;
        }

        public Material GetGroundMaterial()
        {
            return material;
        }
    }
}