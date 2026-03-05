using UnityEngine;

namespace Scripts.Data
{
    [CreateAssetMenu(fileName = "NewUnitData", menuName = "RTS/Unit Data")]
    public class UnitData : ScriptableObject
    {
        public int maxHealth;
        public float moveSpeed;
        public float rotationSpeed;
        public int attackDamage;
        public float attackRange;
        public float fireRate;
        public float bulletSpeed;
    }
}