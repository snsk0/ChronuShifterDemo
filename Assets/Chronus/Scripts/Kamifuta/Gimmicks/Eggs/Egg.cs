using Chronus.ChronuShift;
using Chronus.Direction;
using Chronus.Tags;
using Chronus.UI.InGame.Interact;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Kamifuta.Utils;
using Chronus.Utils;

namespace Chronus.ChronusGimmick.Eggs
{
    public class Egg : MonoBehaviour, IChronusTarget
    {
        [SerializeField] private GameObject bornAnimal;

        [SerializeField] private TimelineAsset bornTimeline;
        [SerializeField] private TimelineAsset rewindTimeline;
        [SerializeField] private PlayableDirector playableDirector;

        [SerializeField] private AudioSource audioSource;

        private bool IsHatchable = false;
        private bool HadBorn = false;

        private void Start()
        {
            StartCoroutine(PlaySECoroutine());
            bornAnimal.SetActive(false);
        }

        public void OnShift(ChronusState state)
        {
            switch (state)
            {
                case ChronusState.Forward:
                    Hatch();
                    break;
                case ChronusState.Backward:
                    Rewind();
                    break;
                default:
                    break;
            }
        }

        private void Rewind()
        {
            if (!HadBorn)
                return;

            playableDirector.playableAsset = rewindTimeline;
            TimelinePlayer.SetTimeline(playableDirector);

            HadBorn = false;
        }

        public void Hatch()
        {
            Debug.Log(IsHatchable);
            if (!IsHatchable)
                return;

            playableDirector.playableAsset = bornTimeline;
            TimelinePlayer.SetTimeline(playableDirector);

            HadBorn = true;
        }

        private void FixedUpdate()
        {
            if (!IsHatchable)
                return;

            var hitObjects = Physics.SphereCastAll(transform.position, 2f, Vector3.up, 2f);
            if (hitObjects.Length == 0)
                return;

            var itemObjects = hitObjects.Where(x => x.collider.CompareTag(TagType.Item)).Select(x => x.collider.gameObject);
            var existFire = itemObjects.Any(x => x.GetComponent<InteractionItem>().itemType == InteractionDatabase.ItemType.Fire);

            IsHatchable = existFire;
        }

        private void OnTriggerEnter(Collider other)
        {
            //近くにギミック成立のアイテムがあるかを判定する。
            if (other.TryGetComponent<InteractionItem>(out var item))
            {
                IsHatchable = item.itemType == InteractionDatabase.ItemType.Fire;
            }
        }

        public IEnumerator PlaySECoroutine()
        {
            yield return new WaitForSeconds(0.16f);
            PlayIdleSE();

            var waiter = new WaitForSeconds(1.5f);

            while (true)
            {
                yield return waiter;
                PlayIdleSE();
            }
        }

        public void PlayIdleSE()
        {
            if (!audioSource.gameObject.activeSelf)
                return;

            audioSource.Play();
        }
    }
}

