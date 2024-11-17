using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Target : MonoBehaviour, IDamagable
    {
        public event Action OnDeath;

        public Sprite deadSprite;

        public void OnDamaged(int damage, Collision2D collision)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            if (renderer.sprite == deadSprite)
            {
                return;
            }
            renderer.sprite = deadSprite;
            OnDeath?.Invoke();
        }
    }
}
