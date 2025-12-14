using UnityEngine;

namespace _Game._Scripts.SwordOrbitSystem.Helpers
{
    internal struct SwordOrbitEntry
    {
        public Transform transform;

        public float currentAngle;
        public float targetAngle;
        public float angularVelocity;

        public Vector3 targetLocalScale;

        public float spawnElapsed;
        public float spawnDuration;
        public bool isSpawning;


        public static SwordOrbitEntry CreateForSpawn(Transform transform, float spawnGrowDuration)
        {
            return new SwordOrbitEntry
            {
                transform = transform,
                currentAngle = 0f,
                targetAngle = 0f,
                angularVelocity = 0f,
                targetLocalScale = Vector3.one,
                spawnElapsed = 0f,
                spawnDuration = Mathf.Max(0.01f, spawnGrowDuration),
                isSpawning = true
            };
        }

        public void Tick(float deltaTime, float radius, float smoothTime)
        {
            currentAngle = Mathf.SmoothDampAngle(
                currentAngle,
                targetAngle,
                ref angularVelocity,
                smoothTime
            );

            float radiusForThisSword = radius;

            if (isSpawning)
            {
                spawnElapsed += deltaTime;
                float t = Mathf.Clamp01(spawnElapsed / spawnDuration);
                float eased = OrbitMath.Smoothstep01(t);

                radiusForThisSword = Mathf.Lerp(0f, radius, eased);
                transform.localScale = Vector3.Lerp(Vector3.zero, targetLocalScale, eased);

                if (t >= 1f)
                {
                    isSpawning = false;
                    transform.localScale = targetLocalScale;
                }
            }

            Vector2 localOffset = OrbitMath.AngleToVector(currentAngle) * radiusForThisSword;
            transform.localPosition = localOffset;

            // Sword is always perpendicular to the orbit direction.
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }
}