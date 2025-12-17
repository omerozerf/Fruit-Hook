using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Game._Scripts.ObjectPoolSystem
{
    public sealed class ObjectPool<T> where T : Component
    {
        private readonly HashSet<int> m_ActiveInstanceIds = new();
        private readonly HashSet<int> m_InactiveInstanceIds = new();
        private readonly Queue<T> m_InactiveQueue = new(64);
        private readonly bool m_KeepWorldPositionWhenParenting;
        private readonly Transform m_Parent;
        private readonly T m_Prefab;

        
        public ObjectPool(
            T prefab,
            Transform parent = null,
            int prewarmCount = 0,
            bool keepWorldPositionWhenParenting = false)
        {
            if (!prefab) throw new ArgumentNullException(nameof(prefab));

            m_Prefab = prefab;
            m_Parent = parent;
            m_KeepWorldPositionWhenParenting = keepWorldPositionWhenParenting;

            if (prewarmCount > 0)
                Prewarm(prewarmCount);
        }

        
        public int TotalCreated { get; private set; }
        public int ActiveCount => m_ActiveInstanceIds.Count;
        public int InactiveCount => m_InactiveQueue.Count;

        
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
        

        private T TryDequeueInactive()
        {
            while (m_InactiveQueue.Count > 0)
            {
                var instance = m_InactiveQueue.Dequeue();
                if (!instance) continue;

                var id = instance.gameObject.GetInstanceID();
                m_InactiveInstanceIds.Remove(id);

                // If someone activated it externally, skip it.
                if (instance.gameObject.activeSelf)
                {
                    m_ActiveInstanceIds.Add(id);
                    continue;
                }

                return instance;
            }

            return null;
        }

        private T CreateNewInstance()
        {
            var instance = Object.Instantiate(m_Prefab, m_Parent);
            TotalCreated++;

            instance.gameObject.SetActive(false);
            return instance;
        }

        private void ActivateInstance(T instance, Vector3 position, Quaternion rotation)
        {
            var tr = instance.transform;

            tr.SetPositionAndRotation(position, rotation);

            // Parent it to pool parent (optional) without breaking world transform if requested.
            if (m_Parent)
                tr.SetParent(m_Parent, m_KeepWorldPositionWhenParenting);

            instance.gameObject.SetActive(true);

            MarkActive(instance);
            CallPoolableSpawned(instance);
        }

        private void DeactivateAndEnqueue(T instance)
        {
            instance.gameObject.SetActive(false);

            var id = instance.gameObject.GetInstanceID();
            if (m_InactiveInstanceIds.Add(id))
                m_InactiveQueue.Enqueue(instance);
        }

        private void MarkActive(T instance)
        {
            var id = instance.gameObject.GetInstanceID();
            m_ActiveInstanceIds.Add(id);
        }

        
        public void Prewarm(int count)
        {
            if (count <= 0) return;

            for (var i = 0; i < count; i++)
            {
                var instance = CreateNewInstance();
                DeactivateAndEnqueue(instance);
            }
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            var instance = TryDequeueInactive();
            if (!instance) instance = CreateNewInstance();

            ActivateInstance(instance, position, rotation);
            return instance;
        }

        public T Get(Transform followParent, bool worldPositionStays = true)
        {
            var instance = TryDequeueInactive();
            if (!instance) instance = CreateNewInstance();

            instance.transform.SetParent(followParent, worldPositionStays);
            instance.gameObject.SetActive(true);

            MarkActive(instance);
            CallPoolableSpawned(instance);
            return instance;
        }

        public void Release(T instance)
        {
            if (!instance) return;

            var id = instance.gameObject.GetInstanceID();

            // If it wasn't active (or already released), ignore to prevent double enqueue bugs.
            if (!m_ActiveInstanceIds.Remove(id))
                return;

            CallPoolableDespawned(instance);

            // Optional: reset parent back to pool parent.
            if (m_Parent)
                instance.transform.SetParent(m_Parent, m_KeepWorldPositionWhenParenting);

            DeactivateAndEnqueue(instance);
        }

        public void ReleaseAllActive(List<T> activeListSnapshot)
        {
            if (activeListSnapshot == null) return;
            for (var i = 0; i < activeListSnapshot.Count; i++)
                Release(activeListSnapshot[i]);
        }
    }
}