using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HGDFall2024.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        public WeightedClip[] clips;

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
            int total = clips.Sum(clip => clip.weight);
            int randVal = Random.Range(0, total);
            int val = 0;
            for (int i = 0; i < clips.Length; i++)
            {
                val += clips[i].weight;
                if (val > randVal)
                {
                    source.clip = clips[i].clip;
                    if (source.clip != null)
                    {
                        StartCoroutine(PlayDelayed(delay));
                    }
                    return;
                }
            }

        }

        public void Play(AudioClip clip, float delay = 0)
        {
            source.clip = clip;
            StartCoroutine(PlayDelayed(delay));
        }

        [Serializable]
        public class WeightedClip
        {
            public AudioClip clip;
            public int weight = 1;
        }

        private IEnumerator PlayDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);

            source.Play();
        }
    }
}
