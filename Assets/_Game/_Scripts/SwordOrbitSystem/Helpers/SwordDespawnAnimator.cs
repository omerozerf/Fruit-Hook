using System;
using System.Collections;
using _Game._Scripts.SwordSystem;
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


        public void StartDespawn(Sword sword, Vector3 centerWorld, float orbitRadius)
        {
            if (!sword)
                return;

            // Prefer running the coroutine on the sword itself so the animation lifecycle is owned by the sword.
            // If the sword is pooled/disabled/destroyed, the coroutine will naturally stop.
            MonoBehaviour runner = null;

            if (sword && sword.isActiveAndEnabled)
                runner = sword;
            else if (m_Host && m_Host.isActiveAndEnabled)
                // Fallback for cases where sword doesn't have a valid MonoBehaviour to host the coroutine.
                runner = m_Host;

            if (!runner)
                return;

            runner.StartCoroutine(DespawnRoutine(sword.transform, centerWorld, orbitRadius));
        }
    }
}