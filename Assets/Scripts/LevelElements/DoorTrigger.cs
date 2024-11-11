using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public class DoorTrigger : MonoBehaviour
    {
        public LayerMask mask;
        public string targetTag;

        public event Action OnEnterTrigger;
        public event Action OnExitTrigger;

        private void OnTriggerEnter2D(Collider2D collision) => Enter(collision.gameObject);

        private void OnTriggerExit2D(Collider2D collision) => Exit(collision.gameObject);

        private void OnCollisionEnter2D(Collision2D collision) => Enter(collision.gameObject);

        private void OnCollisionExit2D(Collision2D collision) => Exit(collision.gameObject);

        private void Enter(GameObject go)
        {
            if (!mask.IsSelected(go.layer) || (targetTag != "" && !go.CompareTag(targetTag)))
            {
                return;
            }

            OnEnterTrigger?.Invoke();
        }

        private void Exit(GameObject go)
        {
            if (!mask.IsSelected(go.layer) || (targetTag != "" && !go.CompareTag(targetTag)))
            {
                return;
            }

            OnExitTrigger?.Invoke();
        }
    }
}
