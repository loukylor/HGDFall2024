using HGDFall2024.Managers;
using UnityEngine;

namespace HGDFall2024.Attachments
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BlowerAttachment : BaseAttachment
    {
        public override AttachmentType Attachment { get; } = AttachmentType.Blower;

        public float blowerDelay = 0.75f;
        public float blowerStrength = 1;
        public float maxStrength = 5;

        public Sprite blowerDefault;
        public Sprite blowerSqueeze;

        private float startBlowTime = 0;
        private new SpriteRenderer renderer;

        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        protected override void Update()
        {
            base.Update();

            transform.position = new Vector3(
                MousePosition.x,
                MousePosition.y,
                -2
            );

            // rotate blower to face player
            Vector2 diff = PlayerManager.Instance.Player.transform.position - transform.position;
            Vector2 direction = diff.normalized;
            transform.eulerAngles = new Vector3(0, 0,
                Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x)
            );

            if (Time.time - startBlowTime < blowerDelay)
            {
                return;
            }

            if (!InputManager.Instance.Player.Click.IsPressed())
            {
                renderer.sprite = blowerDefault;
            }
            else
            {
                startBlowTime = Time.time;
                renderer.sprite = blowerSqueeze;

                // Make blow force inverse propertional to distance squared
                float distance = diff.magnitude;
                float falloff = Mathf.Clamp((-0.25f * distance) + 1, 0, 100);

                Vector2 push = blowerStrength * falloff * direction;
                PlayerManager.Instance.Player.Rb.velocity += Vector2.ClampMagnitude(push, maxStrength);
            }
        }
    }
}
