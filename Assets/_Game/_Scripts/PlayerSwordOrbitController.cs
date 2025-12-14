using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LoopGames.Combat
{
    public class PlayerSwordOrbitController : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private float _radius;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _smoothTime;
        [SerializeField] private float _spawnGrowDuration;

        [Header("Despawn Settings")]
        [SerializeField] private float _despawnDuration = 0.35f;
        [SerializeField] private float _despawnSpinSpeed = 1080f;
        [SerializeField] private float _despawnOffscreenMargin = 1.5f;
        [SerializeField] private float _removeInterval = 5f;

        [Header("References")]
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _swordPrefab;
        [SerializeField] private float _spawnInterval = 5f;

        private readonly List<SwordData> m_Swords = new ();
        
        private float m_SpawnTimer;
        private float m_RemoveTimer;
        

        private void Update()
        {
            HandleTestRemoval();
            UpdateSwordLocalPositions();
            RotateController();
        }
        
        private void HandleTestRemoval()
        {
            if (m_Swords.Count == 0)
                return;

            m_RemoveTimer += Time.deltaTime;
            if (m_RemoveTimer >= _removeInterval)
            {
                m_RemoveTimer = 0f;

                // Remove the last sword for test purposes
                Transform swordToRemove = m_Swords[m_Swords.Count - 1].transform;
                if (swordToRemove != null)
                    RemoveSwordAnimated(swordToRemove);
            }
        }

        private void HandleTestSpawning()
        {
            if (_swordPrefab == null || _center == null)
                return;

            m_SpawnTimer += Time.deltaTime;
            if (m_SpawnTimer >= _spawnInterval)
            {
                m_SpawnTimer = 0f;
                SpawnSword();
            }
        }

        public void SpawnSword()
        {
            Transform swordInstance = Instantiate(_swordPrefab, transform);
            swordInstance.localPosition = Vector3.zero;

            swordInstance.localScale = Vector3.zero;

            AddSword(swordInstance);
        }


        private void AddSword(Transform sword)
        {
            SwordData data = new SwordData
            {
                transform = sword,
                currentAngle = 0f,
                targetAngle = 0f,
                angularVelocity = 0f,
                targetLocalScale = Vector3.one,
                spawnElapsed = 0f,
                spawnDuration = Mathf.Max(0.01f, _spawnGrowDuration),
                isSpawning = true
            };

            m_Swords.Add(data);
            RecalculateTargetAngles();
        }

        public void RemoveSword(Transform sword)
        {
            RemoveSwordAnimated(sword);
        }

        public void RemoveSwordAnimated(Transform sword)
        {
            int index = FindSwordIndex(sword);
            if (index < 0)
                return;

            // Remove from orbit list first so spacing updates immediately.
            m_Swords.RemoveAt(index);
            RecalculateTargetAngles();

            // Detach so controller rotation/orbit updates do not affect the despawn motion.
            sword.SetParent(null, true);

            StartCoroutine(DespawnRoutine(sword));
        }


        private void UpdateSwordLocalPositions()
        {
            for (int i = 0; i < m_Swords.Count; i++)
            {
                SwordData s = m_Swords[i];

                if (s.transform == null)
                    continue;

                s.currentAngle = Mathf.SmoothDampAngle(
                    s.currentAngle,
                    s.targetAngle,
                    ref s.angularVelocity,
                    _smoothTime
                );

                float radiusForThisSword = _radius;
                if (s.isSpawning)
                {
                    s.spawnElapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(s.spawnElapsed / s.spawnDuration);
                    float eased = t * t * (3f - 2f * t); // smoothstep

                    radiusForThisSword = Mathf.Lerp(0f, _radius, eased);
                    s.transform.localScale = Vector3.Lerp(Vector3.zero, s.targetLocalScale, eased);

                    if (t >= 1f)
                    {
                        s.isSpawning = false;
                        s.transform.localScale = s.targetLocalScale;
                    }
                }

                Vector2 localOffset = AngleToVector(s.currentAngle) * radiusForThisSword;
                s.transform.localPosition = localOffset;
                s.transform.localRotation = Quaternion.Euler(0f, 0f, s.currentAngle);
            }
        }

        private void RotateController()
        {
            transform.Rotate(0f, 0f, -_rotationSpeed * Time.deltaTime);
        }

        private void RecalculateTargetAngles()
        {
            int count = m_Swords.Count;
            if (count == 0)
                return;

            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                m_Swords[i].targetAngle = i * angleStep;
            }
        }


        private int FindSwordIndex(Transform sword)
        {
            for (int i = 0; i < m_Swords.Count; i++)
            {
                if (m_Swords[i].transform == sword)
                    return i;
            }

            return -1;
        }

        private IEnumerator DespawnRoutine(Transform sword)
        {
            if (sword == null)
                yield break;

            Camera cam = Camera.main;
            Vector3 startPos = sword.position;
            Vector3 targetPos = GetOffscreenTargetPosition(cam, startPos);

            float duration = Mathf.Max(0.01f, _despawnDuration);
            float elapsed = 0f;

            // Keep the current z rotation as a base, then add extra spinning.
            float baseZ = sword.eulerAngles.z;

            while (elapsed < duration)
            {
                if (sword == null)
                    yield break;

                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                float eased = t * t * (3f - 2f * t); // smoothstep

                sword.position = Vector3.Lerp(startPos, targetPos, eased);

                float z = baseZ + (_despawnSpinSpeed * elapsed);
                sword.rotation = Quaternion.Euler(0f, 0f, z);

                yield return null;
            }

            if (sword != null)
                Destroy(sword.gameObject);
        }

        private Vector3 GetOffscreenTargetPosition(Camera cam, Vector3 fromWorldPos)
        {
            // Fallback if there's no camera.
            if (cam == null)
            {
                Vector3 dirFallback = (fromWorldPos - transform.position);
                if (dirFallback.sqrMagnitude < 0.0001f)
                    dirFallback = Vector3.right;
                dirFallback.Normalize();

                return fromWorldPos + dirFallback * (_radius + _despawnOffscreenMargin + 5f);
            }

            // Compute a direction away from the controller center.
            Vector3 centerWorld = transform.position;
            Vector3 dir = (fromWorldPos - centerWorld);
            if (dir.sqrMagnitude < 0.0001f)
                dir = Vector3.right;
            dir.Normalize();

            // Compute camera world bounds at the sword's depth.
            float zDistance = Mathf.Abs(fromWorldPos.z - cam.transform.position.z);
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;

            Vector3 camCenter = cam.transform.position;
            camCenter.z = fromWorldPos.z;

            // Move far enough so the point is outside bounds along the chosen direction.
            float needX = halfWidth + _despawnOffscreenMargin;
            float needY = halfHeight + _despawnOffscreenMargin;

            float tx = (Mathf.Abs(dir.x) < 0.0001f) ? float.PositiveInfinity : (needX / Mathf.Abs(dir.x));
            float ty = (Mathf.Abs(dir.y) < 0.0001f) ? float.PositiveInfinity : (needY / Mathf.Abs(dir.y));
            float t = Mathf.Min(tx, ty);

            // If direction is diagonal, Min() ensures we cross at least one boundary.
            Vector3 edgeOffset = dir * t;
            Vector3 edgePos = camCenter + edgeOffset;

            // Push a bit further to guarantee off-screen.
            return edgePos + dir * _despawnOffscreenMargin;
        }


        private Vector2 AngleToVector(float angle)
        {
            float rad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }


        private class SwordData
        {
            public Transform transform;
            public float currentAngle;
            public float targetAngle;
            public float angularVelocity;
            public Vector3 targetLocalScale;
            public float spawnElapsed;
            public float spawnDuration;
            public bool isSpawning;
        }
    }
}