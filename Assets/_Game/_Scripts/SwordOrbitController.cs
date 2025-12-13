using System.Collections.Generic;
using UnityEngine;

namespace LoopGames.Combat
{
    public class SwordOrbitController : MonoBehaviour
    {
        [Header("Orbit Settings")]
        [SerializeField] private float radius = 1.5f;
        [SerializeField] private float rotationSpeed = 90f; // derece / saniye
        [SerializeField] private float smoothTime = 0.2f;

        [Header("References")]
        [SerializeField] private Transform center;
        [SerializeField] private Transform swordPrefab;
        [SerializeField] private float spawnInterval = 5f;

        private readonly List<SwordData> swords = new List<SwordData>();
        private float spawnTimer;

        private void Update()
        {
            HandleTestSpawning();
            UpdateSwordLocalPositions();
            RotateController();
        }

        private void HandleTestSpawning()
        {
            if (swordPrefab == null || center == null)
                return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;
                SpawnSword();
            }
        }

        private void SpawnSword()
        {
            Transform swordInstance = Instantiate(swordPrefab, transform);
            swordInstance.localPosition = Vector3.zero;
            AddSword(swordInstance);
        }


        public void AddSword(Transform sword)
        {
            SwordData data = new SwordData
            {
                transform = sword,
                currentAngle = 0f,
                targetAngle = 0f,
                angularVelocity = 0f
            };

            swords.Add(data);
            RecalculateTargetAngles();
        }

        public void RemoveSword(Transform sword)
        {
            swords.RemoveAll(s => s.transform == sword);
            RecalculateTargetAngles();
        }



        private void UpdateSwordLocalPositions()
        {
            for (int i = 0; i < swords.Count; i++)
            {
                SwordData s = swords[i];

                s.currentAngle = Mathf.SmoothDampAngle(
                    s.currentAngle,
                    s.targetAngle,
                    ref s.angularVelocity,
                    smoothTime
                );

                Vector2 localOffset = AngleToVector(s.currentAngle) * radius;
                s.transform.localPosition = localOffset;
                s.transform.localRotation = Quaternion.Euler(0f, 0f, s.currentAngle);
            }
        }

        private void RotateController()
        {
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }

        private void RecalculateTargetAngles()
        {
            int count = swords.Count;
            if (count == 0)
                return;

            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                swords[i].targetAngle = i * angleStep;
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
        }

    }
}