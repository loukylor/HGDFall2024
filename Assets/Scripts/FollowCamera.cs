using UnityEngine;

namespace HGDFall2024
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FollowCamera : MonoBehaviour
    {
        public Transform followObject;
        [Range(0f, 1f)]
        public float lerp = 0.5f;

        private Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (followObject == null)
            {
                return;
            }

            RaycastHit2D hit = Physics2D.Linecast(
                followObject.position,
                transform.position,
                Physics2D.GetLayerCollisionMask(transform.gameObject.layer)
            );

            // Let's not lose line of sight of the player
            if (hit.collider != null)
            {
                rb.position = hit.point;
            }
            else
            {
                // Lerp to object and let unity handle collisions
                Vector2 diff = followObject.position - transform.position;
                rb.velocity = diff * lerp / Time.fixedDeltaTime;
            }

        }
    }
}
