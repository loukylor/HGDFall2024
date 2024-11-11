using UnityEngine;

namespace HGDFall2024
{
    public static class Extensions
    {
        public static Vector2 Rotate(this Vector2 val, float theta)
            => new(
                val.x * Mathf.Cos(theta * Mathf.Deg2Rad) - val.y * Mathf.Sin(theta * Mathf.Deg2Rad),
                val.x * Mathf.Sin(theta * Mathf.Deg2Rad) + val.y * Mathf.Cos(theta * Mathf.Deg2Rad)
            );

        public static bool IsSelected(this LayerMask mask, int layer)
            => (mask.value & (1 << layer)) != 0;
    }
}
