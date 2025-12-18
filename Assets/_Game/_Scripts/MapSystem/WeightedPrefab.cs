using System;
using UnityEngine;

namespace _Game._Scripts.MapSystem
{
    [Serializable]
    public struct WeightedPrefab
    {
        [Min(0)] public int _weight;
        public Transform _prefab;
    }
}