using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public class Door : MonoBehaviour
    {
        public GameObject[] deathWatchers;
        public DoorTrigger[] triggers;
        public GameObject[] disableOnTrigger;

        public Vector2 destPos;
        public float openSpeed = 6;

        private Vector2 originalPos;
        private Vector2 targetPos;

#pragma warning disable IDE0052 // Remove unread private members
        private int Count
#pragma warning restore IDE0052 // Remove unread private members
        { 
            get => _count; 
            set
            {
                _count = value;
                if (_count <= 0)
                {
                    Open();
                }
                else
                {
                    Close();
                }
            } 
        }
        private int _count = 0;

        private void Start()
        {
            Count = deathWatchers.Length + triggers.Length;

            foreach (DoorTrigger trigger in triggers)
            {
                trigger.OnEnterTrigger += () => Count -= 1;
                trigger.OnExitTrigger += () => Count += 1;
            }

            foreach (GameObject go in deathWatchers)
            {
                if (!go.TryGetComponent(out IDamagable damagable))
                {
                    Count--;
                    continue;
                }

                damagable.OnDeath += () => Count -= 1;
            }

            originalPos = transform.localPosition;
            targetPos = transform.localPosition;
        }

        private void Update()
        {
            transform.localPosition = Vector2.MoveTowards(
                transform.localPosition, 
                targetPos, 
                openSpeed * Time.deltaTime
            );
        }

        public void Open()
        {
            targetPos = originalPos + destPos;
            foreach (GameObject disable in disableOnTrigger)
            {
                disable.SetActive(false);
            }
        }

        public void Close()
        {
            targetPos = originalPos;
            foreach (GameObject disable in disableOnTrigger)
            {
                disable.SetActive(true);
            }
        }
    }
}
