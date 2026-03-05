using UnityEngine;

namespace Scripts.Core
{
    public class EnemyBuilding : CombatObject
    {
        [SerializeField] private int maxHealth = 500;

        private void Start()
        {
            health = maxHealth;
        }

        protected override void Die()
        {
            base.Die();

            // effect will added
        }
    }
}