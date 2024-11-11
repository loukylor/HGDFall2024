using HGDFall2024.LevelElements;
using HGDFall2024.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HGDFall2024
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Balloon, IDamagable
    {
        public Rigidbody2D Rb { get; private set; }

        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            InputManager.Instance.Player.CycleAttachmentBack.started += OnCycleAttachmentBack;
            InputManager.Instance.Player.CycleAttachmentNext.started += OnCycleAttachmentNext;
        }

        private void OnDisable()
        {
            if (ApplicationManager.Instance.HasQuit)
            {
                return;
            }

            InputManager.Instance.Player.CycleAttachmentBack.started -= OnCycleAttachmentBack;
            InputManager.Instance.Player.CycleAttachmentNext.started -= OnCycleAttachmentNext;
        }

        private void OnCycleAttachmentBack(InputAction.CallbackContext context)
        {
            PlayerManager.Instance.PreviousAttachment();
        }

        private void OnCycleAttachmentNext(InputAction.CallbackContext context)
        {
            PlayerManager.Instance.NextAttachment();
        }

        public void OnDamaged(int damage)
        {
            Destroy(gameObject);
        }
    }
}
