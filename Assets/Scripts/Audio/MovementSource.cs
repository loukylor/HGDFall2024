using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(RandomAudioSource))]
    public class MovementSource : MonoBehaviour
    {
        public new Rigidbody2D rigidbody;
        public float scale = 1;

        private RandomAudioSource source;

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

            if (rigidbody.velocity.magnitude < 0.5f)
            {
                source.Source.Stop();
            }
            else if (!source.Source.isPlaying)
            {
                source.Play();
            }

            source.Source.volume = rigidbody.velocity.magnitude * 0.1f * scale;
        }
    }
}
