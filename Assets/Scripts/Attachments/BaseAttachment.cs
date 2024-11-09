using System;
using UnityEngine;

namespace HGDFall2024.Attachments
{
    public class BaseAttachment : MonoBehaviour
    {
        public virtual AttachmentType Attachment { get => throw new NotImplementedException(); }
    }
}
