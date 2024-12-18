using UnityEngine;

namespace HGDFall2024.CameraUtils
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Camera))]
    public class FollowCamera : MonoBehaviour
    {
        public Transform followObject;
        public float targetSize;
        [Range(0f, 1f)]
        public float lerp = 0.5f;

        private Rigidbody2D rb;
        private Camera cam;

        private float lostTime;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            cam = GetComponent<Camera>();
            targetSize = cam.orthographicSize;
        }

        private void FixedUpdate()
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, lerp);

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
            if (hit.rigidbody != null && hit.rigidbody != rb)
            {
                if (lostTime != 0)
                {
                    if (Time.time - lostTime > 5)
                    {
                        rb.position = hit.point;
                        lostTime = 0;
                        return;
                    }
                }
                else
                {
                    lostTime = Time.time;
                }
            }

            // Lerp to object and let unity handle collisions
            Vector2 diff = followObject.position - transform.position;
            rb.velocity = diff * lerp / Time.fixedDeltaTime;

        }
    }
}
