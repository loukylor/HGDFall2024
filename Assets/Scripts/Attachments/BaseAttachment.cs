using HGDFall2024.Managers;
using System;
using UnityEngine;

namespace HGDFall2024.Attachments
{
    public class BaseAttachment : MonoBehaviour
    {
        public virtual AttachmentType Attachment { get => throw new NotImplementedException(); }

        protected Vector2 MousePosition { get; private set; }

        protected virtual void Update()
        {
            MousePosition = Camera.main.ScreenToWorldPoint(
                InputManager.Instance.Player.PointerPosition.ReadValue<Vector2>()
            );
        }
    }
}
