using UnityEngine;
using Scripts.Core;

namespace Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        private Camera cam;
        [SerializeField] private LayerMask unitMask;

        private Unit _selectedUnit;

        void Awake()
        {
            cam = Camera.main;
        }

        void Update()
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0)) return;

            Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 500f, unitMask))
            {
                var unit = hit.collider.GetComponentInParent<Unit>();
                if (unit != null)
                    SelectUnit(unit);
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