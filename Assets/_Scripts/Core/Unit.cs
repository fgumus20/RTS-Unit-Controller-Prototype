using UnityEngine;
using Scripts.Data;

namespace Scripts.Core

{
    public enum UnitState { Idle, Moving }

    public class Unit : CombatObject, ISelectable
    {
        [Header("Data")]
        public UnitData unitData;

        [Header("Visuals")]
        [SerializeField] private MeshRenderer unitRenderer;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material unselectedMaterial;

        private UnitState currentState = UnitState.Idle;
        private Vector3 moveDestination;

        private void Start()
        {
            health = unitData.maxHealth;
            Deselect();
        }

        private void Update()
        {
            if (IsDead) return;

            if (currentState == UnitState.Moving)
            {
                HandleMovement();
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
            moveDestination = new Vector3(destination.x, transform.position.y, destination.z);
            currentState = UnitState.Moving;
        }

        private void HandleMovement()
        {
           
            Vector3 direction = (moveDestination - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * unitData.rotationSpeed);
            }

            transform.position = Vector3.MoveTowards(transform.position, moveDestination, unitData.moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveDestination) < 0.1f)
            {
                currentState = UnitState.Idle;
            }
        }
    }
}