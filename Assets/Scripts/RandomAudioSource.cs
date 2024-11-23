using System.Collections;
using UnityEngine;

namespace HGDFall2024
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        public AudioClip[] clips;

        public float randomDelayMin = 0;
        public float randomDelayMax = 0;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();

            if (randomDelayMax != randomDelayMin && audioSource.playOnAwake)
            {
                Play(Random.Range(randomDelayMin, randomDelayMax));
            }
        }

        public void Play()
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }

        public void Play(float delay)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayDelayed(delay);
        }
    }
}
