using _Game._Scripts.ScriptableObjects;
using UnityEngine;

namespace _Game._Scripts.MapSystem.Helpers
{
    internal sealed class WeightedGroundSelector
    {
        private readonly GridMapBuilderSettingsSO m_Settings;

        public WeightedGroundSelector(GridMapBuilderSettingsSO settings)
        {
            m_Settings = settings;
        }

        public Transform Pick()
        {
            var variants = m_Settings.GroundVariants;
            if (variants == null || variants.Length == 0)
            {
                Debug.LogError("No ground variants assigned in GridMapBuilder.");
                return null;
            }

            var totalWeight = 0;
            for (var i = 0; i < variants.Length; i++)
            {
                var w = variants[i]._weight;
                if (w > 0 && variants[i]._prefab)
                    totalWeight += w;
            }

            if (totalWeight <= 0)
            {
                Debug.LogError("All ground variant weights are 0 or prefabs are missing.");
                return null;
            }

            var roll = Random.Range(0, totalWeight);
            for (var i = 0; i < variants.Length; i++)
            {
                var entry = variants[i];
                if (entry._weight <= 0 || !entry._prefab)
                    continue;

                roll -= entry._weight;
                if (roll < 0)
                    return entry._prefab;
            }

            // Fallback (should never happen)
            return variants[0]._prefab;
        }
    }
}