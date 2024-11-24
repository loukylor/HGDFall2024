using HGDFall2024.Attachments;
using HGDFall2024.Managers;
using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(RandomAudioSource))]
    public class WooshSource : MonoBehaviour
    {
        public new Rigidbody2D rigidbody;

        private RandomAudioSource source;

        private bool hasPlayed;

        private void Start()
        {
            source = GetComponent<RandomAudioSource>();
        }

        private void FixedUpdate()
        {
            if (source == null)
            {
                return;
            }

            NoneAttachment grabber = PlayerManager.Instance.Attachments[AttachmentType.None] as NoneAttachment;
            if (grabber.HeldBody != rigidbody)
            {
                hasPlayed = false;
                return;
            }

            Vector2 dir = rigidbody.position - grabber.JointPos;
            if (Vector2.Angle(dir, Vector2.right) < 10)
            {
                if (!hasPlayed)
                {
                    float scale = rigidbody.velocity.magnitude * 0.1f - 1;
                    source.Source.pitch = Mathf.Clamp(scale - 0.5f, 0.7f, 2);
                    source.Source.volume = scale;
                    source.Play();
                    hasPlayed = true;
                }
            }
            else
            {
                hasPlayed = false;
            }
        }
    }
}
