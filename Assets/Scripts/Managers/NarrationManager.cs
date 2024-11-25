using HGDFall2024.Audio;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private TextMeshProUGUI subtitle;

        private Coroutine coroutine;

        protected override void Awake()
        {
            base.Awake();

            animator = GetComponentInChildren<Animator>(true);
            rb = GetComponent<Rigidbody2D>();
            subtitle = GetComponentInChildren<TextMeshProUGUI>(true);

            SceneManager.activeSceneChanged += OnSceneChange;
        }

        private void OnSceneChange(Scene _, Scene __)
        {
            animator.gameObject.SetActive(false);
            StopAllCoroutines();
            coroutine = null;
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
            float beforeWait = Mathf.Max(0, source.delay - 0.6f);
            yield return new WaitForSeconds(beforeWait);

            animator.gameObject.SetActive(true);
            animator.Play(startStateHash);
            if (source.subtitle != "")
            {
                subtitle.text = source.subtitle;
                subtitle.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(source.source.clip.length + (source.delay - beforeWait));

            animator.Play(hideStateHash);
            yield return new WaitForSeconds(0.7f);
            animator.gameObject.SetActive(false);
            subtitle.gameObject.SetActive(false);

            coroutine = null;
        }
    }
}
