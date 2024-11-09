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
            // Lerp to object and let unity handle collisions
            Vector2 diff = followObject.position - transform.position;
            rb.velocity = diff * lerp / Time.fixedDeltaTime;
        }
    }
}
