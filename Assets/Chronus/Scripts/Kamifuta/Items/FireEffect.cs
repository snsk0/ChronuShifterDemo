using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chronus.ChronusItem
{
    public class FireEffect : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        [SerializeField] private Collider parentCollider;
        [SerializeField] private AudioSource audioSource;

        private float defaultAudioVolume;

        private void Start()
        {
            defaultAudioVolume = audioSource.volume;
        }

        private void Update()
        {
            if (parent != null)
            {
                transform.localScale = parent.localScale;
            }

            if (parentCollider != null)
            {
                audioSource.volume = parentCollider.enabled ? defaultAudioVolume : 0f;
            }
        }
    }
}

