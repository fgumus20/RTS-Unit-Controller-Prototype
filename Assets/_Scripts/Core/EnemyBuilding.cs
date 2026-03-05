using UnityEngine;

namespace Scripts.Core
{
    public class EnemyBuilding : CombatObject
    {
        [SerializeField] private int _maxHealth = 500;

        protected override int GetMaxHealth() => _maxHealth;

        protected override void Die()
        {
            base.Die();

            // effect will added
        }
    }
}