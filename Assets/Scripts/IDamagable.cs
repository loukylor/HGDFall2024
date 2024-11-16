using System;
using UnityEngine;

namespace HGDFall2024.LevelElements
{
    public interface IDamagable
    {
        public event Action OnDeath;

        public void OnDamaged(int damage, Collision2D collision);
    }
}
