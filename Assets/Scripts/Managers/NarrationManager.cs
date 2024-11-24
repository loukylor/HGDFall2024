using HGDFall2024.Audio;
using System.Collections;
using UnityEngine;

namespace HGDFall2024.Managers
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class NarrationManager : BaseManager
    {
        public static NarrationManager Instance { get; private set; }

        private readonly int startStateHash = Animator.StringToHash("Start");
        private readonly int hideStateHash = Animator.StringToHash("Hide");
        private Animator animator;
        private Rigidbody2D rb;

        private Coroutine coroutine;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponentInChildren<Animator>(true);
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (PlayerManager.Instance.Player == null)
            {
                return;
            }

            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            rb.position = playerPos;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent(out TriggeredAudioSource source))
            {
                return;
            }

            if (source.HasPlayed)
            {
                return;
            }

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(Show(source));
        }

        private IEnumerator Show(TriggeredAudioSource source)
        {
            yield return new WaitForSeconds(source.delay);

            animator.gameObject.SetActive(true);
            animator.Play(startStateHash);

            yield return new WaitForSeconds(source.source.clip.length);

            animator.Play(hideStateHash);
            yield return new WaitForSeconds(0.7f);
            animator.gameObject.SetActive(false);

            coroutine = null;
        }
    }
}
