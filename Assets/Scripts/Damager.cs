using HGDFall2024.LevelElements;
using UnityEngine;

namespace HGDFall2024.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        public int damage = 1;

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.TryGetComponent(out IDamagable targetable))
            {
                return;
            }

            targetable.OnDamaged(damage, collision);
        }
    }
}
