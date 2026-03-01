using UnityEngine;
using System;

namespace Scripts.Core
{

    public abstract class CombatObject : MonoBehaviour
    {
        public event Action OnDeath;

        protected int health;

        public bool IsDead => health <= 0;


        public virtual void TakeDamage(int amount)
        {
            if (IsDead) return;

            health -= amount;

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        protected virtual void Die()
        {

            OnDeath?.Invoke();
        }
    }
}