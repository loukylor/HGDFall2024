using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public class LevelEndTrigger : MonoBehaviour
    {
        public static event Action OnLevelEnd;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            OnLevelEnd?.Invoke();
        }
    }
}
