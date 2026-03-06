using UnityEngine;

namespace Scripts.Core
{
    public class EnemyBuilding : CombatObject
    {
        [SerializeField] private int _maxHealth = 500;

        protected override int GetMaxHealth() => _maxHealth;
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material _deadMaterial;

        protected override void Die()
        {
            base.Die();
            _renderer.material = _deadMaterial;
        }
    }
}