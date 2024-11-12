using UnityEngine;

namespace HGDFall2024
{
    public static class Helper
    {
        private static readonly Collider2D[] overlaps = new Collider2D[10];

        public static (GameObject, float) FindPlayer(
            Vector2 origin, 
            float radius, 
            Vector2 right, 
            float viewAngle = 360
        ) {
            int hitCount = Physics2D.OverlapCircleNonAlloc(
                 origin,
                 radius,
                 overlaps,
                 LayerMask.GetMask("Player")
            );

            for (int i = 0; i < hitCount; i++)
            {
                if (!overlaps[i].gameObject.CompareTag("Player"))
                {
                    continue;
                }
                
                Vector2 dir = ((Vector2)overlaps[i].transform.position - origin).normalized;
                float angle = Vector2.SignedAngle(right, dir);
                if (Mathf.Abs(angle) > viewAngle / 2)
                {
                    continue;
                }

                RaycastHit2D los = Physics2D.Linecast(
                    origin,
                    overlaps[i].transform.position,
                    LayerMask.GetMask("Default", "Pickupable")
                );
                if (los.collider != null)
                {
                    continue;
                }

                return (overlaps[i].gameObject, angle);
            }

            return (null, 0);
        }
    }
}
