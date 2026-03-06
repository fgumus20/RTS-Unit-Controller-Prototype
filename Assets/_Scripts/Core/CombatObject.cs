using UnityEngine;
using System;

namespace Scripts.Core
{

    public abstract class CombatObject : MonoBehaviour
    {
        public event Action OnDeath;
        public event Action<int, int> OnHealthChanged;

        protected int currentHealth;
        protected int maxHealth;
        public bool IsDead => currentHealth <= 0;

        protected virtual void Start()
        {
            maxHealth = GetMaxHealth();
            currentHealth = maxHealth;
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        protected abstract int GetMaxHealth();
        public virtual void TakeDamage(int amount)
        {
            if (IsDead) return;

            currentHealth -= amount;

            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        protected virtual void Die()
        {

            OnDeath?.Invoke();
        }
    }
}