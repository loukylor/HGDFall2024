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

        private Rigidbody2D rb;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();            
        }

        protected virtual void OnDestroy()
        {
            if (ApplicationManager.Instance.HasQuit || disableAnim)
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

            // Only get crushed between static or dynamic rb's or colliders with
            // no rbs
            if (first.rigidbody == second.rigidbody
                || (first.rigidbody != null 
                && first.rigidbody.bodyType == RigidbodyType2D.Dynamic
                && first.relativeVelocity.magnitude == 0
                && second.rigidbody != null 
                && second.rigidbody.bodyType == RigidbodyType2D.Dynamic
                && second.relativeVelocity.magnitude == 0))
            {
                return;
            }

            if (Vector2.Distance(first.point, second.point) >= 0.5f)
            {
                return;
            }

            Destroy(gameObject);
        }
    }
}
