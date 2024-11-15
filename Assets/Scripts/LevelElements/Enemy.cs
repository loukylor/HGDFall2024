﻿using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public class Enemy : Balloon, IDamagable
    {
        public float viewRadius = 10;
        public float fireInterval = 1;
        public float searchTime = 5;
        public float gunMoveSpeed = 60;

        private float lastSpotted = 0;
        private float lastFired = 0;
        private Gun gun;
        private EnemyState state = EnemyState.Waiting;

        public event Action OnDeath;

        protected override void Start()
        {
            base.Start();
            
            gun = GetComponentInChildren<Gun>(true);
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

        public void OnDamaged(int damage)
        {
            Destroy(gameObject);
        }

        private enum EnemyState
        {
            Waiting,
            Searching,
            Attacking
        }
    }
}
