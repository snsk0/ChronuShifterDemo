using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chronus.KinematicPhysics
{
    [DefaultExecutionOrder(-1000)]
    internal static class PhysicsSystem
    {
        private class PhysicsSystemEntry : MonoBehaviour
        {
            private float _lastFixedTime = 0f;

            private void Update()
            {
                foreach (CharacterBody characterBody in _characterBodies)
                {
                    characterBody.InternalInterpolateUpdate((Time.time - _lastFixedTime) / Time.fixedDeltaTime);
                }
            }

            private void FixedUpdate()
            {
                foreach (CharacterBody characterBody in _characterBodies)
                {
                    characterBody.InternalPhysicsStaticUpdate(Time.fixedDeltaTime);
                }

                //TODO _dynamicCollidersÇÃFixedÇåƒÇ‘ ëŒè€ÇÃDefaultExecutionOrderÇ≈àÍéûëŒâû
                Physics.SyncTransforms();

                foreach (CharacterBody characterBody in _characterBodies)
                {
                    characterBody.InternalPhysicsDynamicUpdate(_dynamicColliders);
                }

                _lastFixedTime = Time.time;
            }
        }

        private static PhysicsSystemEntry _instance;

        private static readonly List<CharacterBody> _characterBodies = new List<CharacterBody>();
        private static readonly List<Collider> _dynamicColliders = new List<Collider>();

        private static bool _isInitialized = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SceneManager.sceneUnloaded += SceneUnloaded;
        }

        private static void SceneUnloaded(Scene thisScene)
        {
            _characterBodies.Clear();
            _dynamicColliders.Clear();
        }

        internal static void Register(CharacterBody body)
        {
            if(!_isInitialized)
            {
                CreateInstance();
            }

            _characterBodies.Add(body);
        }

        internal static void Unregister(CharacterBody body)
        {
            if(!_isInitialized)
            {
                CreateInstance();
            }

            _characterBodies.Remove(body);
        }

        internal static bool IsDynamic(this Collider collider)
        {
            return _dynamicColliders.Contains(collider);
        }

        private static void CreateInstance()
        {
            GameObject gameObject = new GameObject(nameof(PhysicsSystem));
            Object.DontDestroyOnLoad(gameObject);

            _instance = gameObject.AddComponent<PhysicsSystemEntry>();

            _isInitialized = true;
        }

        public static void RegisterDynamicCollider(Collider collider)
        {
            if (!_dynamicColliders.Contains(collider))
            {
                _dynamicColliders.Add(collider);
            }
        }
    }
}
