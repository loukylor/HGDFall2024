using HGDFall2024.Managers;
using HGDFall2024.Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HGDFall2024.Attachments
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PistolAttachment : BaseAttachment
    {
        public override AttachmentType Attachment { get; } = AttachmentType.Pistol;

        public float radius = 0.5f;
        public float fireDelay = 0.25f;
        public float recoil = 3;

        private float lastFireTime = 0;

        private new SpriteRenderer renderer;
        private ProjectileBase projectile;

        private void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
            projectile = GetComponentInChildren<ProjectileBase>(true);
        }

        private void OnEnable()
        {
            InputManager.Instance.Player.Click.started += OnClick;
        }

        private void OnDisable()
        {
            InputManager.Instance.Player.Click.started -= OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (Time.time - lastFireTime < fireDelay)
            {
                return;
            }
            lastFireTime = Time.time;

            GameObject newProjectile = Instantiate(projectile.gameObject);
            newProjectile.transform.SetPositionAndRotation(
                transform.position + (transform.right * 0.1f), 
                transform.rotation
            );
            newProjectile.SetActive(true);

            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            Vector2 direction = (playerPos - MousePosition).normalized;
            PlayerManager.Instance.Player.Rb.velocity += direction * recoil;
        }

        protected override void Update()
        {
            base.Update();

            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            Vector2 direction = (MousePosition - playerPos).normalized;

            transform.position = playerPos + (direction * radius);
            float angle = Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x);
            transform.eulerAngles = new Vector3(0, 0, angle);

            renderer.flipY = Mathf.Abs(angle) > 90;
        }
    }
}
