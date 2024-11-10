using UnityEngine;

namespace HGDFall2024.LevelElements
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Fan : MonoBehaviour
    {
        public float length = 10;
        public float force = 1;
        public LayerMask layers = 0;

        public void Start()
        {
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = new Vector2(length, collider.size.y);
            collider.offset = new Vector2(length / 2, collider.offset.y);

            ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule main = particle.main;
            main.startSpeed = force * 10;
            main.startLifetime = length / (7 * force);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if ((layers.value & (1 << collision.gameObject.layer)) == 0)
            {
                return;
            }

            if (collision.attachedRigidbody == null)
            {
                return;
            }

            RaycastHit2D hit = Physics2D.Linecast(
                transform.position, 
                collision.transform.position, 
                LayerMask.GetMask("Default", "Pickupable")
            );
            if (hit.collider != null)
            {
                return;
            }

            collision.attachedRigidbody.velocity += 
                force * Time.fixedDeltaTime * 10 * (Vector2)transform.right;
        }
    }
}
