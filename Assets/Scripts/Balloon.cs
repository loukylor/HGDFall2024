using HGDFall2024.Managers;
using UnityEngine;

namespace HGDFall2024
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Balloon : MonoBehaviour
    {
        private static readonly ContactPoint2D[] contacts = new ContactPoint2D[2];
        
        public GameObject popAnim;
        public bool disableAnim = false;

        protected Rigidbody2D rb;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();            
        }

        protected virtual void OnDestroy()
        {
            if (!gameObject.scene.isLoaded || ApplicationManager.Instance.HasQuit || disableAnim)
            {
                return;
            }

            GameObject anim = Instantiate(popAnim);
            anim.transform.position = transform.position;
        }

        protected virtual void FixedUpdate()
        {
            // There really shoudln't be a lot of cases where we're colliding
            // with 3 things
            int contactCount = rb.GetContacts(contacts);

            if (contactCount != 2) 
            {
                return;
            }

            ContactPoint2D first = contacts[0];
            ContactPoint2D second = contacts[1];

            // Only get crushed between colliders with no rbs
            if (first.rigidbody != null || second.rigidbody != null)
            {
                return;
            }

            float distance = Vector2.Distance(first.point, second.point);
            if (distance < 0.2f || distance >= 0.5f)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}
