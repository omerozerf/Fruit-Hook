using System.Collections;
using UnityEngine;

namespace LoopGames.Combat
{
    internal sealed class SwordDespawnAnimator
    {
        private readonly MonoBehaviour _host;
        private readonly PlayerSwordOrbitController.DespawnSettings _settings;

        public SwordDespawnAnimator(MonoBehaviour host, PlayerSwordOrbitController.DespawnSettings settings)
        {
            _host = host;
            _settings = settings;
        }

        public void StartDespawn(Transform sword, Vector3 centerWorld, float orbitRadius)
        {
            if (sword == null || _host == null)
                return;

            _host.StartCoroutine(DespawnRoutine(sword, centerWorld, orbitRadius));
        }

        private IEnumerator DespawnRoutine(Transform sword, Vector3 centerWorld, float orbitRadius)
        {
            Camera cam = Camera.main;

            Vector3 startPos = sword.position;
            Vector3 targetPos = OrbitMath.GetOffscreenTargetPosition(
                cam,
                fromWorldPos: startPos,
                centerWorld: centerWorld,
                fallbackRadius: orbitRadius,
                offscreenMargin: _settings._offscreenMargin
            );

            float duration = Mathf.Max(0.01f, _settings._duration);
            float elapsed = 0f;
            float baseZ = sword.eulerAngles.z;

            while (elapsed < duration)
            {
                if (sword == null)
                    yield break;

                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = OrbitMath.Smoothstep01(t);

                sword.position = Vector3.Lerp(startPos, targetPos, eased);

                float z = baseZ + (_settings._spinSpeed * elapsed);
                sword.rotation = Quaternion.Euler(0f, 0f, z);

                yield return null;
            }

            if (sword != null)
                UnityEngine.Object.Destroy(sword.gameObject);
        }
    }
}