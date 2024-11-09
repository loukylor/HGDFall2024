using UnityEngine;

namespace HGDFall2024.Projectiles
{
    public class PistolProjectile : ProjectileBase
    {
        public float speed;

        private void Update()
        {
            transform.position += speed * Time.deltaTime * transform.right;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(gameObject);
        }
    }
}
