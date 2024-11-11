using HGDFall2024.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HGDFall2024.Attachments
{
    [RequireComponent(typeof(Gun))]
    public class PistolAttachment : BaseAttachment
    {
        public override AttachmentType Attachment { get; } = AttachmentType.Pistol;

        public float fireDelay = 0.25f;
        public float recoil = 3;

        private float lastFireTime = 0;
        private Gun gun;

        private void Start()
        {
            gun = GetComponent<Gun>();
        }

        private void OnEnable()
        {
            InputManager.Instance.Player.Click.started += OnClick;
        }

        private void OnDisable()
        {
            if (ApplicationManager.Instance.HasQuit)
            {
                return;
            }

            InputManager.Instance.Player.Click.started -= OnClick;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (Time.time - lastFireTime < fireDelay)
            {
                return;
            }
            lastFireTime = Time.time;

            gun.Fire();

            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            Vector2 direction = (playerPos - MousePosition).normalized;
            PlayerManager.Instance.Player.Rb.velocity += direction * recoil;
        }

        protected override void Update()
        {
            base.Update();

            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            Vector2 direction = (MousePosition - playerPos).normalized;

            gun.direction = direction;
            gun.origin = playerPos;
        }
    }
}
