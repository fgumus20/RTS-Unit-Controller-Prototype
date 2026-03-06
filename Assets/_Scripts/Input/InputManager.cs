using UnityEngine;
using Scripts.Core;

namespace Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        private Camera _cam;
        [SerializeField] private LayerMask _unitMask;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _targetMask;

        private Unit _selectedUnit;

        void Awake()
        {
            _cam = Camera.main;
        }

        void Update()
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0)) return;

            Ray ray = _cam.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (TrySelectUnit(ray)) return;
            if (TryAssignTarget(ray)) return;
            if (TryMoveToGround(ray)) return;
        }

        private bool TrySelectUnit(Ray ray)
        {
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f, _unitMask)) return false;

            var unit = hit.collider.GetComponentInParent<Unit>();
            if (unit == null) return false;

            SelectUnit(unit);
            return true;
        }

        private bool TryAssignTarget(Ray ray)
        {
            if (_selectedUnit == null) return false;
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f, _targetMask)) return false;

            var target = hit.collider.GetComponentInParent<CombatObject>();
            if (target == null || target == _selectedUnit || target.IsDead) return false;

            _selectedUnit.SetTarget(target);
            return true;
        }

        private bool TryMoveToGround(Ray ray)
        {
            if (_selectedUnit == null) return false;
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f, _groundMask)) return false;

            _selectedUnit.MoveTo(hit.point);
            return true;
        }

        private void SelectUnit(Unit unit)
        {
            if (_selectedUnit == unit) return;

            _selectedUnit?.Deselect();
            _selectedUnit = unit;
            _selectedUnit.Select();
        }
    }
}