using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider2D))]
    public class CollisionSource : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            float scale = collision.relativeVelocity.magnitude * 0.05f;

            RandomAudioSource source = GetComponent<RandomAudioSource>();
            source.Source.volume = scale;
            source.Play();
        }
    }
}
