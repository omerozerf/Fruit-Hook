using System;
using System.Collections.Generic;
using UnityEngine;

namespace LoopGames
{
    /// <summary>
    /// General purpose object pool (Queue-based).
    /// - Uses SetActive for lifecycle
    /// - If no inactive instance is available, instantiates a new one
    /// - No "auto expand" toggle: it always creates when needed
    /// </summary>
    public sealed class ObjectPool<T> where T : Component
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly bool _keepWorldPositionWhenParenting;

        private readonly Queue<T> _inactiveQueue = new Queue<T>(64);
        private readonly HashSet<int> _inactiveInstanceIds = new HashSet<int>();
        private readonly HashSet<int> _activeInstanceIds = new HashSet<int>();

        public int TotalCreated { get; private set; }
        public int ActiveCount => _activeInstanceIds.Count;
        public int InactiveCount => _inactiveQueue.Count;

        public ObjectPool(
            T prefab,
            Transform parent = null,
            int prewarmCount = 0,
            bool keepWorldPositionWhenParenting = false)
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab));

            _prefab = prefab;
            _parent = parent;
            _keepWorldPositionWhenParenting = keepWorldPositionWhenParenting;

            if (prewarmCount > 0)
                Prewarm(prewarmCount);
        }

        public void Prewarm(int count)
        {
            if (count <= 0) return;

            for (int i = 0; i < count; i++)
            {
                var instance = CreateNewInstance();
                DeactivateAndEnqueue(instance);
            }
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            var instance = TryDequeueInactive();
            if (instance == null)
            {
                instance = CreateNewInstance();
            }

            ActivateInstance(instance, position, rotation);
            return instance;
        }

        public T Get(Transform followParent, bool worldPositionStays = true)
        {
            var instance = TryDequeueInactive();
            if (instance == null)
            {
                instance = CreateNewInstance();
            }

            instance.transform.SetParent(followParent, worldPositionStays);
            instance.gameObject.SetActive(true);

            MarkActive(instance);
            CallPoolableSpawned(instance);
            return instance;
        }

        public void Release(T instance)
        {
            if (instance == null) return;

            int id = instance.gameObject.GetInstanceID();

            // If it wasn't active (or already released), ignore to prevent double enqueue bugs.
            if (!_activeInstanceIds.Remove(id))
                return;

            CallPoolableDespawned(instance);

            // Optional: reset parent back to pool parent.
            if (_parent != null)
                instance.transform.SetParent(_parent, _keepWorldPositionWhenParenting);

            DeactivateAndEnqueue(instance);
        }

        public void ReleaseAllActive(List<T> activeListSnapshot)
        {
            if (activeListSnapshot == null) return;
            for (int i = 0; i < activeListSnapshot.Count; i++)
                Release(activeListSnapshot[i]);
        }

        private T TryDequeueInactive()
        {
            while (_inactiveQueue.Count > 0)
            {
                var instance = _inactiveQueue.Dequeue();
                if (instance == null) continue;

                int id = instance.gameObject.GetInstanceID();
                _inactiveInstanceIds.Remove(id);

                // If someone activated it externally, skip it.
                if (instance.gameObject.activeSelf)
                {
                    _activeInstanceIds.Add(id);
                    continue;
                }

                return instance;
            }

            return null;
        }

        private T CreateNewInstance()
        {
            var instance = UnityEngine.Object.Instantiate(_prefab, _parent);
            TotalCreated++;

            instance.gameObject.SetActive(false);
            return instance;
        }

        private void ActivateInstance(T instance, Vector3 position, Quaternion rotation)
        {
            var tr = instance.transform;

            tr.SetPositionAndRotation(position, rotation);

            // Parent it to pool parent (optional) without breaking world transform if requested.
            if (_parent != null)
                tr.SetParent(_parent, _keepWorldPositionWhenParenting);

            instance.gameObject.SetActive(true);

            MarkActive(instance);
            CallPoolableSpawned(instance);
        }

        private void DeactivateAndEnqueue(T instance)
        {
            instance.gameObject.SetActive(false);

            int id = instance.gameObject.GetInstanceID();
            if (_inactiveInstanceIds.Add(id))
                _inactiveQueue.Enqueue(instance);
        }

        private void MarkActive(T instance)
        {
            int id = instance.gameObject.GetInstanceID();
            _activeInstanceIds.Add(id);
        }

        private static void CallPoolableSpawned(T instance)
        {
            if (instance.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnSpawnedFromPool();
        }

        private static void CallPoolableDespawned(T instance)
        {
            if (instance.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnDespawnedToPool();
        }
    }
}