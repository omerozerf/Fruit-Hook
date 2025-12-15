using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts.SwordOrbitSystem.Helpers
{
    internal sealed class SwordOrbitList
    {
        private readonly List<SwordOrbitEntry> m_Swords = new();

        public int Count => m_Swords.Count;

        public void Add(Transform sword, float spawnGrowDuration)
        {
            if (sword == null)
                return;

            m_Swords.Add(SwordOrbitEntry.CreateForSpawn(sword, spawnGrowDuration));
        }

        public bool TryRemove(Transform sword, out SwordOrbitEntry removed)
        {
            for (var i = 0; i < m_Swords.Count; i++)
            {
                if (m_Swords[i].transform != sword)
                    continue;

                removed = m_Swords[i];
                m_Swords.RemoveAt(i);
                return true;
            }

            removed = default;
            return false;
        }

        public void RecalculateTargetAngles()
        {
            var count = m_Swords.Count;
            if (count == 0)
                return;

            var angleStep = 360f / count;
            for (var i = 0; i < count; i++)
            {
                var entry = m_Swords[i];
                entry.targetAngle = i * angleStep;
                m_Swords[i] = entry;
            }
        }

        public void TickOrbit(float deltaTime, float radius, float smoothSpeed)
        {
            for (var i = 0; i < m_Swords.Count; i++)
            {
                var entry = m_Swords[i];
                if (entry.transform == null)
                    continue;

                entry.Tick(deltaTime, radius, smoothSpeed);
                m_Swords[i] = entry;
            }
        }

        public Transform GetLastTransform()
        {
            if (m_Swords.Count == 0)
                return null;

            return m_Swords[^1].transform;
        }
    }
}