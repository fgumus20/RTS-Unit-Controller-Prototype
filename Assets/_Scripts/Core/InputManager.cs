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

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, _unitMask))
            {
                var unit = hit.collider.GetComponentInParent<Unit>();
                if (unit != null)
                {
                    SelectUnit(unit);
                    return;
                }
            }

            if (_selectedUnit != null)
            {

                if (Physics.Raycast(ray, out RaycastHit hitTarget, 500f, _targetMask))
                {
                    var target = hitTarget.collider.GetComponentInParent<CombatObject>();
                    if (target != null && target != _selectedUnit)
                    {     
                        _selectedUnit.SetTarget(target);
                        return;
                    }
                }

                if (Physics.Raycast(ray, out RaycastHit groundHit, 500f, _groundMask))
                {
                    _selectedUnit.MoveTo(groundHit.point);
                }
            }
        }

        private void SelectUnit(Unit unit)
        {
            if (_selectedUnit == unit) return;

            if (_selectedUnit != null)
                _selectedUnit.Deselect();

            _selectedUnit = unit;
            _selectedUnit.Select();
        }
    }
}