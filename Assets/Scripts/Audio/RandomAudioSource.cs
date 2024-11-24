using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        public AudioClip[] clips;

        public float randomDelayMin = 0;
        public float randomDelayMax = 0;

        public AudioSource Source => source;

        private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();

            if (source.playOnAwake)
            {
                Play(Random.Range(randomDelayMin, randomDelayMax));
            }
        }

        public void Play(float delay = 0)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.PlayDelayed(delay);
        }

        public void Play(AudioClip clip, float delay = 0)
        {
            source.clip = clip;
            source.PlayDelayed(delay);
        }
    }
}
