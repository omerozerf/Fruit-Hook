using UnityEngine;

namespace LoopGames.Combat
{
    internal struct SwordOrbitEntry
    {
        public Transform Transform;

        public float CurrentAngle;
        public float TargetAngle;
        public float AngularVelocity;

        public Vector3 TargetLocalScale;

        public float SpawnElapsed;
        public float SpawnDuration;
        public bool IsSpawning;

        public static SwordOrbitEntry CreateForSpawn(Transform transform, float spawnGrowDuration)
        {
            return new SwordOrbitEntry
            {
                Transform = transform,
                CurrentAngle = 0f,
                TargetAngle = 0f,
                AngularVelocity = 0f,
                TargetLocalScale = Vector3.one,
                SpawnElapsed = 0f,
                SpawnDuration = Mathf.Max(0.01f, spawnGrowDuration),
                IsSpawning = true
            };
        }

        public void Tick(float deltaTime, float radius, float smoothTime)
        {
            CurrentAngle = Mathf.SmoothDampAngle(
                CurrentAngle,
                TargetAngle,
                ref AngularVelocity,
                smoothTime
            );

            float radiusForThisSword = radius;

            if (IsSpawning)
            {
                SpawnElapsed += deltaTime;
                float t = Mathf.Clamp01(SpawnElapsed / SpawnDuration);
                float eased = OrbitMath.Smoothstep01(t);

                radiusForThisSword = Mathf.Lerp(0f, radius, eased);
                Transform.localScale = Vector3.Lerp(Vector3.zero, TargetLocalScale, eased);

                if (t >= 1f)
                {
                    IsSpawning = false;
                    Transform.localScale = TargetLocalScale;
                }
            }

            Vector2 localOffset = OrbitMath.AngleToVector(CurrentAngle) * radiusForThisSword;
            Transform.localPosition = localOffset;

            // Sword is always perpendicular to the orbit direction.
            Transform.localRotation = Quaternion.Euler(0f, 0f, CurrentAngle);
        }
    }
}