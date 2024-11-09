using HGDFall2024.Attachments;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HGDFall2024.Managers
{
    public class PlayerManager : BaseManager
    {
        public static PlayerManager Instance { get; private set; }

        public Player Player { get; private set; }
        public BaseAttachment CurrentAttachment { get; private set; }

        private Dictionary<AttachmentType, BaseAttachment> attachments;

        protected override void Awake()
        {
            base.Awake();

            attachments = transform.GetComponentsInChildren<BaseAttachment>(true)
                .ToDictionary(attachment => attachment.Attachment);
            SetAttachment(AttachmentType.None);

            SceneManager.activeSceneChanged += OnSceneChange;
            ProgressManager.Instance.OnAvailableAttachmentsChanged += OnAvailableAttachmentsChanged;
        }

#if UNITY_EDITOR
        private void Start()
        {
            OnSceneChange(SceneManager.GetSceneByName("Game"), SceneManager.GetSceneByName("Game"));
        }
#endif

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SceneManager.activeSceneChanged -= OnSceneChange;
            if (ProgressManager.Instance != null)
            {
                ProgressManager.Instance.OnAvailableAttachmentsChanged -= OnAvailableAttachmentsChanged;
            }
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            foreach (GameObject go in newScene.GetRootGameObjects())
            {
                if (go.CompareTag("Player"))
                {
                    Player = go.GetComponent<Player>();
                    break;
                }
            }
        }

        private void OnAvailableAttachmentsChanged(AttachmentType[] oldAttachments, AttachmentType[] newAttachments)
        {
            if (CurrentAttachment == null && newAttachments.Contains(CurrentAttachment.Attachment)) 
            {
                return;
            }

            Debug.LogWarning("Available attachments was changed to a value that doesn't include current attachment");
            SetAttachment(AttachmentType.None);
        }

        public void SetAttachment(AttachmentType attachment)
        {
            if (!Enum.IsDefined(typeof(AttachmentType), attachment))
            {
                throw new ArgumentException("Passed in invalid attachment: " + attachment);
            }

            // Check if it is available
            if (!ProgressManager.Instance.AvailableAttachments.Contains(attachment))
            {
                throw new ArgumentException("Attempted to set an attachment that is unavailable: " + attachment);
            }

            if (CurrentAttachment != null)
            {
                CurrentAttachment.gameObject.SetActive(false);
            }
            CurrentAttachment = attachments[attachment];
            CurrentAttachment.gameObject.SetActive(true);
        }

        public void NextAttachment()
        {
            AttachmentType[] available = ProgressManager.Instance.AvailableAttachments;
            int index = 1 + Array.IndexOf(available, CurrentAttachment.Attachment);
            index %= available.Length;
            SetAttachment(available[index]);
        }

        public void PreviousAttachment()
        {
            AttachmentType[] available = ProgressManager.Instance.AvailableAttachments;
            int index = -1 + Array.IndexOf(available, CurrentAttachment.Attachment);
            SetAttachment(index >= 0 ? available[index] : available[^1]);
        }
    }
}
