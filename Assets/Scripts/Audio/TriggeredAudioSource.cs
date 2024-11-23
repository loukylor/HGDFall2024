using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggeredAudioSource : MonoBehaviour
    {
        public AudioSource source;

        public AudioClip[] regularClips;
        public AudioClip[] interruptClips;
        public bool playOnce = true;
        public float delay = 0;

        public bool HasPlayed { get; private set; } = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (HasPlayed)
            {
                return;
            }
            HasPlayed = true;
            Play();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (HasPlayed)
            {
                return;
            }
            HasPlayed = true;
            Play();
        }

        private void Play()
        {
            if (interruptClips != null && interruptClips.Length > 0 && source.isPlaying)
            {
                source.clip = interruptClips[Random.Range(0, interruptClips.Length)];
            }
            else
            {
                source.clip = regularClips[Random.Range(0, regularClips.Length)];
            }
            source.PlayDelayed(delay);
        }
    }
}
