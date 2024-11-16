using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public class Door : MonoBehaviour
    {
        private static readonly ContactPoint2D[] contacts = new ContactPoint2D[3];

        public bool IsOpen { get; private set; }

        public GameObject[] deathWatchers;
        public DoorTrigger[] triggers;
        public GameObject[] disableOnTrigger;

        public Vector2 destPos;
        public float openSpeed = 6;

        private new Collider2D collider;

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
            collider = GetComponent<Collider2D>();

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

            if (collider != null 
                && Vector2.Distance(transform.localPosition, targetPos) > 0.03)
            {
                int contactCount = collider.GetContacts(contacts);
                for (int i = 0; i < contactCount; i++)
                {
                    if (contacts[i].collider.GetComponent<Pickupable>() == null)
                    {
                        continue;
                    }

                    if (IsOpen)
                    {
                        Close();
                    }
                    else
                    {
                        Open();
                    }
                    return;
                }
            }
        }

        public void Open()
        {
            targetPos = originalPos + destPos;
            foreach (GameObject disable in disableOnTrigger)
            {
                disable.SetActive(false);
            }
            IsOpen = true;
        }

        public void Close()
        {
            targetPos = originalPos;
            foreach (GameObject disable in disableOnTrigger)
            {
                disable.SetActive(true);
            }
            IsOpen = false;
        }
    }
}
