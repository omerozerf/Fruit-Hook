using UnityEngine;

namespace _Game._Scripts.SwordOrbitSystem.Helpers
{
    internal static class OrbitMath
    {
        public static float Smoothstep01(float t)
        {
            t = Mathf.Clamp01(t);
            return t * t * (3f - 2f * t);
        }

        public static Vector2 AngleToVector(float angle)
        {
            var rad = angle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        public static Vector3 GetOffscreenTargetPosition(
            Camera cam,
            Vector3 fromWorldPos,
            Vector3 centerWorld,
            float fallbackRadius,
            float offscreenMargin
        )
        {
            if (!cam)
            {
                var dirFallback = fromWorldPos - centerWorld;
                if (dirFallback.sqrMagnitude < 0.0001f)
                    dirFallback = Vector3.right;

                dirFallback.Normalize();
                return fromWorldPos + dirFallback * (fallbackRadius + offscreenMargin + 5f);
            }

            var dir = fromWorldPos - centerWorld;
            if (dir.sqrMagnitude < 0.0001f)
                dir = Vector3.right;

            dir.Normalize();

            var halfHeight = cam.orthographicSize;
            var halfWidth = halfHeight * cam.aspect;

            var camCenter = cam.transform.position;
            camCenter.z = fromWorldPos.z;

            var needX = halfWidth + offscreenMargin;
            var needY = halfHeight + offscreenMargin;

            var tx = Mathf.Abs(dir.x) < 0.0001f ? float.PositiveInfinity : needX / Mathf.Abs(dir.x);
            var ty = Mathf.Abs(dir.y) < 0.0001f ? float.PositiveInfinity : needY / Mathf.Abs(dir.y);
            var t = Mathf.Min(tx, ty);

            var edgePos = camCenter + dir * t;
            return edgePos + dir * offscreenMargin;
        }
    }
}