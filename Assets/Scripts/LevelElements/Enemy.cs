using HGDFall2024.Attachments;
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

        private float lastHitTime = 0;
        private new SpriteRenderer renderer;
        private float lastSpotted = 0;
        private float lastFired = 0;
        private Gun gun;
        private EnemyState state = EnemyState.Waiting;

        public event Action OnDeath;

        protected override void Start()
        {
            base.Start();
            
            gun = GetComponentInChildren<Gun>(true);
            renderer = GetComponent<SpriteRenderer>();

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
                lastFired = Time.time;
            }

            if (gun.isActiveAndEnabled)
            {
                float currentAngle = Vector2.SignedAngle(transform.right, gun.direction);
                float nextAngle = Mathf.MoveTowardsAngle(
                    currentAngle,
                    player == null ? 0 : angle,
                    gunMoveSpeed * Time.fixedDeltaTime
                );
                gun.direction = ((Vector2)transform.right).Rotate(nextAngle);
            }

            switch (state)
            {
                case EnemyState.Waiting:
                    gun.gameObject.SetActive(false);
                    break;
                case EnemyState.Searching:
                    gun.gameObject.SetActive(false);

                    if (Time.time - lastSpotted < searchTime)
                    {
                        break;
                    }

                    state = EnemyState.Waiting;
                    break;
                case EnemyState.Attacking:
                    gun.gameObject.SetActive(true);
                    
                    if (player == null)
                    {
                        state = EnemyState.Searching;
                        lastSpotted = Time.time;
                        break;
                    }

                    if (Time.time - lastFired < fireInterval)
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
        }

        private void SetHitState()
        {
            if (hits == 1)
            {
                renderer.sprite = defaultSprite;
                rb.mass = defaultMass;
                rb.drag = defaultDrag;

                NoneAttachment grabber = PlayerManager.Instance.Attachments[AttachmentType.None] as NoneAttachment;
                if (grabber.HeldBody == rb)
                {
                    grabber.Drop();
                }
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
    }
}
