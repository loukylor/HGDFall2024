using HGDFall2024.Attachments;
using System;
using System.Linq;
using UnityEngine;

namespace HGDFall2024.Managers
{
    public class ProgressManager : BaseManager
    {
        public static ProgressManager Instance { get; private set; }

        public event Action<uint, uint> OnAvailableLevelsChanged;
        private uint _availableLevels = 0;
        public uint AvailableLevels 
        { 
            get => _availableLevels; 
            set
            {
                OnAvailableLevelsChanged?.Invoke(_availableLevels, value);
                _availableLevels = value;
                PlayerPrefs.SetInt(nameof(AvailableLevels), (int)_availableLevels);
            }
        }

        public event Action<AttachmentType[], AttachmentType[]> OnAvailableAttachmentsChanged;
        private AttachmentType[] _availableAttachments = new AttachmentType[3] { AttachmentType.None, AttachmentType.Blower, AttachmentType.Pistol };
        public AttachmentType[] AvailableAttachments 
        { 
            get => _availableAttachments;
            set 
            {
                // Has to be a valid flag
                if (value.Any(attachment => !Enum.IsDefined(typeof(AttachmentType), attachment)))
                {
                    Debug.LogWarning($"Attempted to set an invalid attachment value ({string.Join(", ", value)}):\n{new System.Diagnostics.StackTrace()}");
                    return;
                }

                // Make sure switch order is consistent
                Array.Sort(value);
                OnAvailableAttachmentsChanged?.Invoke(_availableAttachments, value);
                _availableAttachments = value;
                PlayerPrefs.SetString(nameof(AvailableAttachments), string.Join(',', _availableAttachments));
            } 
        }

        protected override void Awake()
        {
            base.Awake();

            _availableLevels = (uint)PlayerPrefs.GetInt(nameof(AvailableLevels), (int)_availableLevels);
            _availableAttachments = PlayerPrefs.GetString(nameof(AvailableAttachments), string.Join(',', _availableAttachments))
                .Split(',')
                .Select(element => Enum.Parse<AttachmentType>(element))
                .ToArray();
        }
    }
}
