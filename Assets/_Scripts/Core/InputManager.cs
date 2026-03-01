using UnityEngine;
using Scripts.Core;

namespace Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        [Header("Layer Masks")]
        [SerializeField] private LayerMask unitLayer;

        private ISelectable currentSelectedUnit;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, unitLayer))
                {
                    ISelectable clickedUnit = hit.collider.GetComponent<ISelectable>();

                    if (clickedUnit != null)
                    {
                        if (currentSelectedUnit != null && currentSelectedUnit != clickedUnit)
                        {
                            Debug.Log("a");
                            currentSelectedUnit.Deselect();
                        }

                        currentSelectedUnit = clickedUnit;
                        currentSelectedUnit.Select();
                    }
                }
            }
        }
    }
}