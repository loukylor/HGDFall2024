using HGDFall2024.Managers;
using UnityEngine;

namespace HGDFall2024.Attachments
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BlowerAttachment : BaseAttachment
    {
        public override AttachmentType Attachment { get; } = AttachmentType.Blower;

        public float radius = 0.5f;
        public float blowerRotSpeed = 720;

        public float blowerDelay = 0.75f;
        public float rechargeDelay = 0.5f;
        public float blowerStrength = 1;
        public float maxStrength = 5;

        public Sprite blowerDefault;
        public Sprite blowerSqueeze;

        private float startBlowTime = 0;
        private float targetAngle = 90;
        private float currentAngle = 90;
        private new SpriteRenderer renderer;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            base.Update();

            Vector2 input = InputManager.Instance.Player.Movement.ReadValue<Vector2>();
            Vector2 direction = input.normalized;

            if (input.magnitude > 0.125)
            {
                targetAngle = -Vector2.SignedAngle(direction, Vector2.right);
            }

            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, blowerRotSpeed * Time.fixedDeltaTime);

            Vector2 actualDirection = Vector2.right.Rotate(currentAngle);
            transform.SetPositionAndRotation(
                new Vector3(
                    PlayerManager.Instance.Player.transform.position.x - (actualDirection.x * radius),
                    PlayerManager.Instance.Player.transform.position.y - (actualDirection.y * radius),
                    -2
                ),
                Quaternion.Euler(0, 0, currentAngle)
            );

            float timeSinceBlow = Time.time - startBlowTime;
            if (timeSinceBlow < blowerDelay)
            {
                return;
            }

            renderer.sprite = blowerDefault;
            
            if (input.magnitude >= 0.125 && timeSinceBlow >= blowerDelay + rechargeDelay)
            {
                startBlowTime = Time.time;
                renderer.sprite = blowerSqueeze;

                Vector2 push = Vector2.ClampMagnitude(blowerStrength * direction, maxStrength);
                PlayerManager.Instance.Player.Rb.velocity += push;
            }
        }
    }
}
