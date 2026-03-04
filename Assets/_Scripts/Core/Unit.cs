using UnityEngine;
using Scripts.Data;

namespace Scripts.Core

{
    public enum UnitState { Idle, Moving, Chasing }

    public class Unit : CombatObject, ISelectable
    {
        [Header("Data")]
        public UnitData unitData;

        [Header("Visuals")]
        [SerializeField] private MeshRenderer unitRenderer;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material unselectedMaterial;

        private UnitState _currentState = UnitState.Idle;
        private Vector3 _moveDestination;
        private CombatObject _currentTarget;


        private void Start()
        {
            health = unitData.maxHealth;
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
            }
        }

        public void Select()
        {
            if (unitRenderer != null && selectedMaterial != null) unitRenderer.material = selectedMaterial;
        }

        public void Deselect()
        {
            if (unitRenderer != null && unselectedMaterial != null) unitRenderer.material = unselectedMaterial;
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

            if (distance <= unitData.attackRange)
            {
                // Attack state
                _currentState = UnitState.Idle;
            }
            else
            {
                MoveTowardsPosition(targetPos);
            }
        }

        private void MoveTowardsPosition(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * unitData.rotationSpeed);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, unitData.moveSpeed * Time.deltaTime);
        }
    }
}