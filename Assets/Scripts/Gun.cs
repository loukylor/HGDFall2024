using HGDFall2024.Audio;
using HGDFall2024.Projectiles;
using UnityEngine;

namespace HGDFall2024
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Gun : MonoBehaviour
    {
        public Vector2 origin;
        public float radius;
        public Vector2 direction;
        public float zPos = 0;

        private new SpriteRenderer renderer;
        private Damager projectile;
        private RandomAudioSource source;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            projectile = GetComponentInChildren<Damager>(true);
            source = GetComponent<RandomAudioSource>();
        }

        private void Update()
        {
            float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            Vector3 newPos = origin + (direction * radius);
            transform.SetPositionAndRotation(
                newPos + (Vector3.forward * zPos),
                Quaternion.Euler(0, 0, angle)
            );

            renderer.flipY = Mathf.Abs(angle) > 90;
        }

        public void Fire()
        {
            GameObject newProjectile = Instantiate(projectile.gameObject);
            newProjectile.transform.SetPositionAndRotation(
                transform.position + (transform.right * 0.1f),
                transform.rotation
            );
            newProjectile.SetActive(true);
            if (source != null)
            {
                source.Play();
            }
        }
    }
}
