using HGDFall2024.LevelElements;
using UnityEngine;

namespace HGDFall2024.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        public int damage = 1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<IDamagable>(out var targetable))
            {
                return;
            }

            targetable.OnDamaged(damage);
        }
    }
}
