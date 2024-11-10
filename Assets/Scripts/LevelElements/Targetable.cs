using HGDFall2024.Projectiles;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Targetable : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<ProjectileBase>() == null)
            {
                return;
            }

            OnHit(collision);
        }

        protected abstract void OnHit(Collider2D collider);
    }
}
