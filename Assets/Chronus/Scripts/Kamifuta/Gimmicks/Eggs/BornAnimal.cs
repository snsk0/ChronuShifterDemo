using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chronus.KinematicPhysics;

namespace Chronus.ChronusGimmick.Eggs
{
    public class BornAnimal : MonoBehaviour, IInteractableGimmick
    {
        private GimmickEventPlayer gimmickEventPlayer;

        [SerializeField] private Transform startTransform;
        [SerializeField] private Transform endTransform;

        private Vector3 startPoint;
        private Vector3 endPoint;

        public bool IsInteractable { get; } = true;
        public bool IsInteracting { get; private set; } = false;

        private void Awake()
        {
            startPoint = startTransform.position;
            endPoint = endTransform.position;
        }

        private void Start()
        {
            gimmickEventPlayer = FindObjectOfType<GimmickEventPlayer>();

            startTransform.position = transform.position;
            startTransform.rotation = transform.rotation;
        }

        private void Update()
        {
            Debug.Log(startPoint);
        }

        public void Move(Transform playerTransform)
        {
            if (transform.position == startPoint)
            {
                gimmickEventPlayer.FadeInAsync(callback: ()=>Move(endPoint, endTransform, playerTransform)).Forget();
            }
            else if (transform.position == endPoint)
            {
                gimmickEventPlayer.FadeInAsync(callback: () => Move(startPoint, startTransform, playerTransform)).Forget();
            }
        }

        private void Move(Vector3 goalPosition, Transform goalTransform, Transform playerTransform)
        {
            transform.position = goalPosition;
            transform.rotation = goalTransform.rotation;

            var playerPosition = transform.position + goalTransform.right;
            playerPosition.y = playerTransform.position.y;
            //playerTransform.position = playerPosition;
            playerTransform.GetComponent<CharacterBody>().MovePosition(playerPosition);

            IsInteracting = false;
        }

        public void Interact<T>(T interactor) where T : MonoBehaviour
        {
            IsInteracting = true;
            var playerTransform = interactor.transform;
            Move(playerTransform);
        }
    }
}

