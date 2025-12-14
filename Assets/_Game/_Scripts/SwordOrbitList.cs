using System.Collections.Generic;
using UnityEngine;

namespace LoopGames.Combat
{
    internal sealed class SwordOrbitList
    {
        private readonly List<SwordOrbitEntry> _swords = new();

        public int Count => _swords.Count;

        public void Add(Transform sword, float spawnGrowDuration)
        {
            if (sword == null)
                return;

            _swords.Add(SwordOrbitEntry.CreateForSpawn(sword, spawnGrowDuration));
        }

        public bool TryRemove(Transform sword, out SwordOrbitEntry removed)
        {
            for (int i = 0; i < _swords.Count; i++)
            {
                if (_swords[i].Transform != sword)
                    continue;

                removed = _swords[i];
                _swords.RemoveAt(i);
                return true;
            }

            removed = default;
            return false;
        }

        public void RecalculateTargetAngles()
        {
            int count = _swords.Count;
            if (count == 0)
                return;

            float angleStep = 360f / count;
            for (int i = 0; i < count; i++)
            {
                SwordOrbitEntry entry = _swords[i];
                entry.TargetAngle = i * angleStep;
                _swords[i] = entry;
            }
        }

        public void TickOrbit(float deltaTime, float radius, float smoothTime)
        {
            for (int i = 0; i < _swords.Count; i++)
            {
                SwordOrbitEntry entry = _swords[i];
                if (entry.Transform == null)
                    continue;

                entry.Tick(deltaTime, radius, smoothTime);
                _swords[i] = entry;
            }
        }

        public Transform GetLastTransform()
        {
            if (_swords.Count == 0)
                return null;

            return _swords[_swords.Count - 1].Transform;
        }
    }
}