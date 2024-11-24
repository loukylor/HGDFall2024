using HGDFall2024.Attachments;
using HGDFall2024.Audio;
using HGDFall2024.Managers;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Pickupable : MonoBehaviour
    {
        private Rigidbody2D rb;
        private RandomAudioSource source;

        private bool hasPlayed;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            source = GetComponent<RandomAudioSource>();    
        }

        private void FixedUpdate()
        {
            if (source == null)
            {
                return;
            }

            NoneAttachment grabber = PlayerManager.Instance.Attachments[AttachmentType.None] as NoneAttachment;
            if (grabber.HeldBody != rb)
            {
                hasPlayed = false;
                return;
            }

            Vector2 dir = rb.position - grabber.JointPos;
            if (Vector2.Angle(dir, Vector2.right) < 10)
            {
                if (!hasPlayed)
                {
                    float scale = rb.velocity.magnitude * 0.1f - 1;
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
