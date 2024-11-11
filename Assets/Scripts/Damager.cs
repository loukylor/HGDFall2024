using HGDFall2024.LevelElements;
using UnityEngine;

namespace HGDFall2024.Projectiles
{
    [RequireComponent(typeof(Collider2D))]
    public class Damager : MonoBehaviour
    {
        public int damage = 1;

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("what");
            if (!collision.TryGetComponent(out IDamagable targetable))
            {
                return;
            }

            targetable.OnDamaged(damage);
        }
        
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("what");
            if (!collision.collider.TryGetComponent(out IDamagable targetable))
            {
                return;
            }

            targetable.OnDamaged(damage);
        }
    }
}
