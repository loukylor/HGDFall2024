using System.Collections;
using UnityEngine;

namespace HGDFall2024.Projectiles
{
    public class PistolProjectile : Damager
    {
        public float speed;
        public float lifetime = 3;

        private void Start()
        {
            StartCoroutine(LifetimeCoroutine());
        }

        private IEnumerator LifetimeCoroutine()
        {
            yield return new WaitForSeconds(lifetime);

            Destroy(gameObject);
        }

        private void Update()
        {
            transform.position += speed * Time.deltaTime * transform.right;
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
         
            Destroy(gameObject);
        }
    }
}
