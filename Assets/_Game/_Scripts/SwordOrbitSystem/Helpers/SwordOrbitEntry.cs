using UnityEngine;

namespace _Game._Scripts.SwordOrbitSystem.Helpers
{
    internal struct SwordOrbitEntry
    {
        public Transform transform;
        public float currentAngle;
        public float targetAngle;
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
                targetLocalScale = Vector3.one,
                spawnElapsed = 0f,
                spawnDuration = Mathf.Max(0.01f, spawnGrowDuration),
                isSpawning = true
            };
        }

        public void Tick(float deltaTime, float radius, float smoothSpeed)
        {
            var radiusForThisSword = radius;

            if (isSpawning)
            {
                spawnElapsed += deltaTime;
                var t = Mathf.Clamp01(spawnElapsed / spawnDuration);
                var eased = OrbitMath.Smoothstep01(t);

                radiusForThisSword = Mathf.Lerp(0f, radius, eased);
                transform.localScale = Vector3.Lerp(Vector3.zero, targetLocalScale, eased);

                if (t >= 1f)
                {
                    isSpawning = false;
                    transform.localScale = targetLocalScale;
                }
            }

            var targetOffset = OrbitMath.AngleToVector(targetAngle) * radiusForThisSword;

            var lerpT = deltaTime * smoothSpeed;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetOffset, lerpT);

            var currentPos = (Vector2)transform.localPosition;
            var currentAngleDeg = Mathf.Atan2(currentPos.y, currentPos.x) * Mathf.Rad2Deg;
            currentAngle = currentAngleDeg;
            transform.localRotation = Quaternion.Euler(0f, 0f, currentAngleDeg);
        }
    }
}