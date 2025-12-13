using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace LoopGames.Combat
{
    public class SwordOrbitController : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private float _radius;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _smoothTime;
        [SerializeField] private float _spawnGrowDuration;

        [Header("References")]
        [SerializeField] private Transform _center;
        [SerializeField] private Transform _swordPrefab;
        [SerializeField] private float _spawnInterval = 5f;

        private readonly List<SwordData> m_Swords = new ();
        
        private float m_SpawnTimer;

        
        private void Update()
        {
            HandleTestSpawning();
            UpdateSwordLocalPositions();
            RotateController();
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

        private void SpawnSword()
        {
            Transform swordInstance = Instantiate(_swordPrefab, transform);
            swordInstance.localPosition = Vector3.zero;

            Vector3 targetScale = swordInstance.localScale;
            swordInstance.localScale = Vector3.zero;

            AddSword(swordInstance, targetScale);
        }


        public void AddSword(Transform sword, Vector3 targetScale)
        {
            SwordData data = new SwordData
            {
                transform = sword,
                currentAngle = 0f,
                targetAngle = 0f,
                angularVelocity = 0f,
                targetLocalScale = targetScale,
                spawnElapsed = 0f,
                spawnDuration = Mathf.Max(0.01f, _spawnGrowDuration),
                isSpawning = true
            };

            m_Swords.Add(data);
            RecalculateTargetAngles();
        }

        public void RemoveSword(Transform sword)
        {
            m_Swords.RemoveAll(s => s.transform == sword);
            RecalculateTargetAngles();
        }


        private void UpdateSwordLocalPositions()
        {
            for (int i = 0; i < m_Swords.Count; i++)
            {
                SwordData s = m_Swords[i];

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