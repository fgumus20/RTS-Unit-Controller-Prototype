using UnityEngine;
using Scripts.Data;
using Scripts.Combat;
using Scripts.Util;

namespace Scripts.Core

{
    public enum UnitState { Idle, Moving, Chasing, Attacking}

    public class Unit : CombatObject, ISelectable
    {
        [Header("Data")]
        [SerializeField] private UnitData _unitData;

        [Header("Visuals")]
        [SerializeField] private MeshRenderer _unitRenderer;
        [SerializeField] private Material _selectedMaterial;
        [SerializeField] private Material _unselectedMaterial;

        [Header("Combat")]
        [SerializeField] private Transform _projectileSpawnPoint;

        private UnitState _currentState = UnitState.Idle;
        private Vector3 _moveDestination;
        private CombatObject _currentTarget;
        private float _nextFireTime;


        private void Start()
        {
            health = _unitData.maxHealth;
            Deselect();
        }

        private void Update()
        {
            if (IsDead) return;

            switch (_currentState)
            {
                case UnitState.Moving:
                    HandleMovement();
                    break;
                case UnitState.Chasing:
                    HandleChasing();
                    break;
                case UnitState.Attacking:
                    HandleAttacking();
                    break;
            }
        }

        public void Select()
        {
            if (_unitRenderer != null && _selectedMaterial != null) _unitRenderer.material = _selectedMaterial;
        }

        public void Deselect()
        {
            if (_unitRenderer != null && _unselectedMaterial != null) _unitRenderer.material = _unselectedMaterial;
        }

        public void MoveTo(Vector3 destination)
        {
            _currentTarget = null;
            _moveDestination = new Vector3(destination.x, transform.position.y, destination.z);
            _currentState = UnitState.Moving;
        }

        public void SetTarget(CombatObject target)
        {
            _currentTarget = target;
            _currentState = UnitState.Chasing;
        }

        private void HandleMovement()
        {

            MoveTowardsPosition(_moveDestination);

            if (Vector3.Distance(transform.position, _moveDestination) < 0.1f)
            {
                _currentState = UnitState.Idle;
            }
        }

        private void HandleChasing()
        {
            if (_currentTarget == null || _currentTarget.IsDead)
            {
                _currentState = UnitState.Idle;
                return;
            }

            Vector3 targetPos = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
            float distance = Vector3.Distance(transform.position, targetPos);

            if (distance <= _unitData.attackRange)
            {
                _currentState = UnitState.Attacking;
            }
            else
            {
                MoveTowardsPosition(targetPos);
            }
        }


        private void HandleAttacking()
        {
            if (_currentTarget == null || _currentTarget.IsDead)
            {
                _currentState = UnitState.Idle;
                _currentTarget = null;
                return;
            }

            Vector3 targetPos = new Vector3(_currentTarget.transform.position.x, transform.position.y, _currentTarget.transform.position.z);
            float distance = Vector3.Distance(transform.position, targetPos);

            if (distance > _unitData.attackRange)
            {
                _currentState = UnitState.Chasing;
                return;
            }

            RotateTowardsPosition(targetPos);

            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + _unitData.fireRate;
            }
        }

        private void Fire()
        {
            if (_currentTarget == null) return;

            Vector3 spawnPos = _projectileSpawnPoint.position;

            Bullet bullet = BulletPool.Instance.GetBullet();
            bullet.Initialize(spawnPos, _currentTarget, _unitData.attackDamage, _unitData.bulletSpeed);
        }

        private void MoveTowardsPosition(Vector3 targetPosition)
        {
            RotateTowardsPosition(targetPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _unitData.moveSpeed * Time.deltaTime);
        }

        private void RotateTowardsPosition(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _unitData.rotationSpeed);
            }
        }
    }
}