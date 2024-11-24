using HGDFall2024.Attachments;
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
        public AudioListener Listener { get; private set; }
        //public BaseAttachment CurrentAttachment { get; private set; }

        public IReadOnlyDictionary<AttachmentType, BaseAttachment> Attachments => attachments;
        private Dictionary<AttachmentType, BaseAttachment> attachments;

        protected override void Awake()
        {
            base.Awake();

            attachments = transform.GetComponentsInChildren<BaseAttachment>(true)
                .ToDictionary(attachment => attachment.Attachment);
            //SetAttachment(AttachmentType.None);

            SceneManager.activeSceneChanged += OnSceneChange;
            //ProgressManager.Instance.OnAvailableAttachmentsChanged += OnAvailableAttachmentsChanged;
        }

#if UNITY_EDITOR
        private void Start()
        {
            GameObject balloon = GameObject.Find("Balloon");
            Player = balloon.GetComponentInChildren<Player>(true);
            Listener = balloon.GetComponentInChildren<AudioListener>(true);
            Player.OnDeath += OnPlayerDeath;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
#endif

        protected override void OnDestroy()
        {
            if (ApplicationManager.Instance.HasQuit)
            {
                return;
            }

            base.OnDestroy();
            SceneManager.activeSceneChanged -= OnSceneChange;
            //ProgressManager.Instance.OnAvailableAttachmentsChanged -= OnAvailableAttachmentsChanged;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex < 3)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
                return;
            }

            foreach (GameObject go in newScene.GetRootGameObjects())
            {
                if (go.CompareTag("Player"))
                {
                    Player = go.GetComponentInChildren<Player>();
                    Listener = go.GetComponentInChildren<AudioListener>(true);
                    Player.OnDeath += OnPlayerDeath;

                    foreach (Transform child in transform)
                    {
                        child.gameObject.SetActive(true);
                    }
                    break;
                }
            }
        }

        private void OnPlayerDeath()
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            ApplicationManager.Instance.ShowLevelMenu(ApplicationManager.LevelMenuState.Died);
        }

        //private void OnAvailableAttachmentsChanged(AttachmentType[] oldAttachments, AttachmentType[] newAttachments)
        //{
        //    if (CurrentAttachment == null && newAttachments.Contains(CurrentAttachment.Attachment)) 
        //    {
        //        return;
        //    }

        //    Debug.LogWarning("Available attachments was changed to a value that doesn't include current attachment");
        //    SetAttachment(AttachmentType.None);
        //}

        //public void SetAttachment(AttachmentType attachment)
        //{
        //    if (!Enum.IsDefined(typeof(AttachmentType), attachment))
        //    {
        //        throw new ArgumentException("Passed in invalid attachment: " + attachment);
        //    }

        //    // Check if it is available
        //    if (!ProgressManager.Instance.AvailableAttachments.Contains(attachment))
        //    {
        //        throw new ArgumentException("Attempted to set an attachment that is unavailable: " + attachment);
        //    }

        //    if (CurrentAttachment != null)
        //    {
        //        CurrentAttachment.gameObject.SetActive(false);
        //    }
        //    CurrentAttachment = attachments[attachment];
        //    CurrentAttachment.gameObject.SetActive(true);
        //}

        //public void NextAttachment()
        //{
        //    if (ProgressManager.Instance.AvailableAttachments.Length == 0)
        //    {
        //        return;
        //    }
        //    else if (CurrentAttachment == null)
        //    {
        //        SetAttachment(ProgressManager.Instance.AvailableAttachments[0]);
        //        return;
        //    }

        //    AttachmentType[] available = ProgressManager.Instance.AvailableAttachments;
        //    int index = 1 + Array.IndexOf(available, CurrentAttachment.Attachment);
        //    index %= available.Length;
        //    SetAttachment(available[index]);
        //}

        //public void PreviousAttachment()
        //{
        //    if (ProgressManager.Instance.AvailableAttachments.Length == 0)
        //    {
        //        return;
        //    }
        //    else if (CurrentAttachment == null)
        //    {
        //        SetAttachment(ProgressManager.Instance.AvailableAttachments[0]);
        //        return;
        //    }

        //    AttachmentType[] available = ProgressManager.Instance.AvailableAttachments;
        //    int index = -1 + Array.IndexOf(available, CurrentAttachment.Attachment);
        //    SetAttachment(index >= 0 ? available[index] : available[^1]);
        //}
    }
}
