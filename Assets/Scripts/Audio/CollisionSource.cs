using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(RandomAudioSource))]
    [RequireComponent(typeof(Collider2D))]
    public class CollisionSource : MonoBehaviour
    {
        public bool scaleSpeed = true;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            float scale = collision.relativeVelocity.magnitude * 0.05f;

            RandomAudioSource source = GetComponent<RandomAudioSource>();
            if (scaleSpeed)
            {
                source.Source.volume = scale;
            }
            source.Play();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            RandomAudioSource source = GetComponent<RandomAudioSource>();
            source.Play();
        }
    }
}
