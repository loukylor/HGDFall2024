using HGDFall2024.Attachments;
using HGDFall2024.Audio;
using HGDFall2024.Managers;
using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : Balloon, IDamagable
    {
        [Header("Gun")]
        public float viewRadius = 10;
        public float fireInterval = 1;
        public float searchTime = 5;
        public float lostTime = 1;
        public float gunMoveSpeed = 60;

        [Header("Hits")]
        public int hits = 1;

        public Sprite armoredSprite;
        public float armoredMass = 10;
        public float armoredDrag = 3;
        public float invincibilityTime = 0.2f;
        public Sprite defaultSprite;
        public float defaultMass = 3;
        public float defaultDrag = 0.5f;
        public float popForce = 10;

        [Header("Voicelines")]
        public RandomAudioSource waitingSource;
        public RandomAudioSource spottedSource;
        public RandomAudioSource searchingSource;
        public RandomAudioSource toSearchingSource;
        public RandomAudioSource lostSource;
        public RandomAudioSource attackingSource;
        public float voiceLineDelayMin;
        public float voiceLineDelayMax;

        private float lastHitTime = 0;
        private new SpriteRenderer renderer;
        private float lastSpotted = 0;
        private float lastFired;
        private Gun gun;
        private EnemyState state = EnemyState.Waiting;
        private float lastVoiceLine;
        private float voiceLineWait;

        public event Action OnDeath;

        protected override void Start()
        {
            base.Start();
            
            gun = GetComponentInChildren<Gun>(true);
            renderer = GetComponent<SpriteRenderer>();
            lastFired = Time.time;

            SetHitState();
        }

        private void Update()
        {
            gun.origin = transform.position;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            (GameObject player, float angle) = Helper.FindPlayer(
                transform.position, 
                viewRadius, 
                transform.right,
                gameObject
            );

            if (player != null && state != EnemyState.Attacking)
            {
                gun.direction = ((Vector2)transform.right).Rotate(angle);
                state = EnemyState.Attacking;
                TryVoiceLine(spottedSource, true);
            }

            float currentAngle = Vector2.SignedAngle(transform.right, gun.direction);
            if (gun.isActiveAndEnabled && player != null)
            {
                float nextAngle = Mathf.MoveTowardsAngle(
                    currentAngle,
                    angle,
                    gunMoveSpeed * Time.fixedDeltaTime
                );
                gun.direction = ((Vector2)transform.right).Rotate(nextAngle);
            }

            switch (state)
            {
                case EnemyState.Waiting:
                    lastFired = Time.time;
                    gun.gameObject.SetActive(false);
                    TryVoiceLine(waitingSource);
                    break;
                case EnemyState.Searching:
                    gun.gameObject.SetActive(false);

                    if (Time.time - lastSpotted < searchTime)
                    {
                        TryVoiceLine(searchingSource);
                        break;
                    }

                    state = EnemyState.Waiting;
                    TryVoiceLine(lostSource, true);
                    break;
                case EnemyState.Attacking:
                    gun.gameObject.SetActive(true);
                    
                    if (player == null)
                    {
                        if (Time.time - lastSpotted > lostTime)
                        {
                            TryVoiceLine(toSearchingSource, true);
                            state = EnemyState.Searching;
                        }
                        break;
                    }
                    lastSpotted = Time.time;
                    TryVoiceLine(attackingSource);

                    if (Time.time - lastFired < fireInterval && MathF.Abs(currentAngle - angle) < 5)
                    {
                        break;
                    }

                    lastFired = Time.time;
                    gun.Fire();
                    break;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            OnDeath?.Invoke();
        }

        public void OnDamaged(int damage, Collision2D collision)
        {
            if (Time.time - lastHitTime < invincibilityTime)
            {
                return;
            }

            hits--;
            lastHitTime = Time.time;
            if (hits == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                SetHitState();
            }

            Vector2 normal = collision.GetContact(0).normal;
            if (collision.otherRigidbody != null 
                && collision.otherRigidbody.bodyType == RigidbodyType2D.Dynamic)
            {
                collision.otherRigidbody.AddForce(normal * popForce);
            }
            rb.AddForce(normal * -popForce);


            NoneAttachment grabber = PlayerManager.Instance.Attachments[AttachmentType.None] as NoneAttachment;
            if (grabber.HeldBody == rb || grabber.HeldBody == collision.otherRigidbody)
            {
                grabber.Drop();
            }
        }

        private void SetHitState()
        {
            if (hits == 1)
            {
                renderer.sprite = defaultSprite;
                rb.mass = defaultMass;
                rb.drag = defaultDrag;
            }
            else if (hits == 2) 
            {
                renderer.sprite = armoredSprite;
                rb.mass = armoredMass;
                rb.drag = armoredDrag;
            }
        }

        private enum EnemyState
        {
            Waiting,
            Searching,
            Attacking
        }

        private bool TryVoiceLine(RandomAudioSource source, bool bypassWait = false)
        {
            if (Time.time - lastVoiceLine > voiceLineWait || bypassWait)
            {
                voiceLineWait = UnityEngine.Random.Range(voiceLineDelayMin, voiceLineDelayMax);
                lastVoiceLine = Time.time;
                if (source != null && (!source.Source.isPlaying || bypassWait))
                {
                    source.Play();
                }
                return true;
            }
            return false;
        }
    }
}
