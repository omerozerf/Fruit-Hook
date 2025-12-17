using System;
using System.Collections;
using UnityEngine;

namespace _Game._Scripts.SwordOrbitSystem.Helpers
{
    internal sealed class SwordDespawnAnimator
    {
        private readonly MonoBehaviour m_Host;
        private readonly Action<Transform> m_OnDespawnCompleted;
        private readonly SwordOrbitController.DespawnSettings m_Settings;
        private Camera m_Cam;

        public SwordDespawnAnimator(
            MonoBehaviour host,
            SwordOrbitController.DespawnSettings settings,
            Action<Transform> onDespawnCompleted)
        {
            m_Host = host;
            m_Settings = settings;
            m_OnDespawnCompleted = onDespawnCompleted;
        }

        private void Awake()
        {
            m_Cam = Camera.main;
        }


        public void StartDespawn(Transform sword, Vector3 centerWorld, float orbitRadius)
        {
            if (!sword || !m_Host)
                return;

            m_Host.StartCoroutine(DespawnRoutine(sword, centerWorld, orbitRadius));
        }

        private IEnumerator DespawnRoutine(Transform sword, Vector3 centerWorld, float orbitRadius)
        {
            var startPos = sword.position;
            var targetPos = OrbitMath.GetOffscreenTargetPosition(
                m_Cam,
                startPos,
                centerWorld,
                orbitRadius,
                m_Settings._offscreenMargin
            );

            var duration = Mathf.Max(0.01f, m_Settings._duration);
            var elapsed = 0f;
            var baseZ = sword.eulerAngles.z;

            while (elapsed < duration)
            {
                if (!sword)
                    yield break;

                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                var eased = OrbitMath.Smoothstep01(t);

                sword.position = Vector3.Lerp(startPos, targetPos, eased);

                var z = baseZ + m_Settings._spinSpeed * elapsed;
                sword.rotation = Quaternion.Euler(0f, 0f, z);

                yield return null;
            }

            if (sword)
                m_OnDespawnCompleted?.Invoke(sword);
        }
    }
}