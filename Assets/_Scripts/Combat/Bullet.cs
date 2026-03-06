using UnityEngine;
using Scripts.Core;

namespace Scripts.Combat
{
    public class Bullet : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private CombatObject _targetObject;
        private int _damage;
        private float _speed;
        private bool _isActive;

        public void Initialize(Vector3 startPos, CombatObject target, int damage, float speed)
        {
            transform.position = startPos;
            _targetObject = target;
            _targetPosition = target.transform.position;
            _damage = damage;
            _speed = speed;
            _isActive = true;
        }

        private void Update()
        {
            if (!_isActive) return;

            if (_targetObject == null || _targetObject.IsDead)
            {
                ReturnToPool();
                return;
            }

            _targetPosition = _targetObject.transform.position;

            float step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                HitTarget();
            }
        }

        private void HitTarget()
        {
            if (_targetObject != null && !_targetObject.IsDead)
            {
                _targetObject.TakeDamage(_damage);
            }

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _isActive = false;
            _targetObject = null;
            BulletPool.Instance.Return(this);
        }
    }
}