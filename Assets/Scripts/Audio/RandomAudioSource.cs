using UnityEngine;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        public AudioClip[] clips;

        public float randomDelayMin = 0;
        public float randomDelayMax = 0;

        public AudioSource AudioSource => audioSource;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            if (randomDelayMax != randomDelayMin && audioSource.playOnAwake)
            {
                Play(Random.Range(randomDelayMin, randomDelayMax));
            }
        }

        public void Play(float delay = 0)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayDelayed(delay);
        }

        public void Play(AudioClip clip, float delay = 0)
        {
            audioSource.clip = clip;
            audioSource.PlayDelayed(delay);
        }
    }
}
