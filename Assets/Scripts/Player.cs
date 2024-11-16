using HGDFall2024.LevelElements;
using System;
using UnityEngine;

namespace HGDFall2024
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Balloon, IDamagable
    {
        public Rigidbody2D Rb { get; private set; }

        public event Action OnDeath;

        protected override void Start()
        {
            base.Start();

            Rb = GetComponent<Rigidbody2D>();
        }

        //private void OnEnable()
        //{
        //    InputManager.Instance.Player.CycleAttachmentBack.started += OnCycleAttachmentBack;
        //    InputManager.Instance.Player.CycleAttachmentNext.started += OnCycleAttachmentNext;
        //}

        //private void OnDisable()
        //{
        //    if (ApplicationManager.Instance.HasQuit)
        //    {
        //        return;
        //    }

        //    InputManager.Instance.Player.CycleAttachmentBack.started -= OnCycleAttachmentBack;
        //    InputManager.Instance.Player.CycleAttachmentNext.started -= OnCycleAttachmentNext;
        //}

        //private void OnCycleAttachmentBack(InputAction.CallbackContext context)
        //{
        //    PlayerManager.Instance.PreviousAttachment();
        //}

        //private void OnCycleAttachmentNext(InputAction.CallbackContext context)
        //{
        //    PlayerManager.Instance.NextAttachment();
        //}

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!gameObject.scene.isLoaded)
            {
                return;
            }
         
            OnDeath?.Invoke();
        }

        public void OnDamaged(int damage, Collision2D collision)
        {
            //Destroy(gameObject);
        }
    }
}
