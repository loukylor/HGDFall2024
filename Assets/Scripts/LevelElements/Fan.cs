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

            AudioSource source = GetComponent<AudioSource>();
            source.PlayDelayed(Random.Range(0, 1));
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!layers.IsSelected(collision.gameObject.layer))
            {
                return;
            }

            if (collision.attachedRigidbody == null)
            {
                return;
            }

            RaycastHit2D hit = Physics2D.Linecast(
                transform.position + (transform.right * 0.3f), 
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
