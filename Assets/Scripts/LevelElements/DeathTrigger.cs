using HGDFall2024.Projectiles;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    public class DeathTrigger : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<Player>() == null)
            {
                return;
            }

            Debug.Log("death");
        }
    }
}
