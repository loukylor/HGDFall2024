using System.Collections;
using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(Collider2D))]
    public class TriggeredAudioSource : MonoBehaviour
    {
        public AudioSource source;
        public string targetTag = "Narration";

        public AudioClip[] regularClips;
        public AudioClip[] interruptClips;
        public bool playOnce = true;
        public float delay = 0;

        public string subtitle;

        public bool HasPlayed { get; private set; } = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (HasPlayed || !collision.gameObject.CompareTag(targetTag))
            {
                return;
            }
            StartCoroutine(SetHasPlayed());
            Play();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (HasPlayed || !collision.gameObject.CompareTag(targetTag))
            {
                return;
            }
            StartCoroutine(SetHasPlayed());
            Play();
        }

        private void Play()
        {
            if (interruptClips != null && interruptClips.Length > 0 && source.isPlaying)
            {
                source.Stop();
                source.clip = interruptClips[Random.Range(0, interruptClips.Length)];
            }
            else
            {
                source.clip = regularClips[Random.Range(0, regularClips.Length)];
            }
            StartCoroutine(PlayDelayed(delay));
        }

        private IEnumerator SetHasPlayed()
        {
            yield return null;
            HasPlayed = true;
        }

        private IEnumerator PlayDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);

            source.Play();
        }
    }
}
