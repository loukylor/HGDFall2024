using System;

namespace HGDFall2024.LevelElements
{
    public interface IDamagable
    {
        public event Action OnDeath;

        public void OnDamaged(int damage);
    }
}
