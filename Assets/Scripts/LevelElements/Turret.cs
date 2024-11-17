using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public class Turret : MonoBehaviour, IDamagable
    {
        public bool isDestroyed = false;
        public float viewRadius = 10;
        public float viewAngle = 70;
        public float barrelSpeed = 20;
        public float barrelRadius = 0.1f;

        private float targetAngle;
        private Transform barrel;

        public event Action OnDeath;

        private void Start()
        {
            barrel = transform.Find("Barrel");
        }

        public void OnDamaged(int damage, Collision2D collision)
        {
            isDestroyed = true;
            OnDeath?.Invoke();
        }

        private void Update()
        {
            float currentAngle = Vector2.SignedAngle(transform.right, barrel.right);
            float angle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, barrelSpeed * Time.deltaTime);

            Vector2 posDelta = ((Vector2)transform.right).Rotate(angle);
            barrel.SetPositionAndRotation(
                transform.position + (barrelRadius * (Vector3)posDelta), 
                Quaternion.Euler(0, 0, angle)
            );
        }

        private void FixedUpdate()
        {
            if (isDestroyed)
            {

            }

            (GameObject target, float angle) = Helper.FindPlayer(
                transform.position,
                viewRadius,
                transform.right,
                viewAngle: viewAngle
            );

            if (target != null)
            {
                targetAngle = angle;
            }
            else
            {
                targetAngle = 0;
            }
        }
    }
}
