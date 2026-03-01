using UnityEngine;

namespace Scripts.Core
{
    public class Unit : CombatObject, ISelectable
    {
        [Header("Visuals")]
        [SerializeField] private MeshRenderer unitRenderer;
        [SerializeField] private Material selectedMaterial;
        [SerializeField] private Material unselectedMaterial;

        private void Start()
        {
            Deselect();
        }

        public void Select()
        {
            if (unitRenderer != null && selectedMaterial != null)
                unitRenderer.material = selectedMaterial;
        }

        public void Deselect()
        {
            if (unitRenderer != null && unselectedMaterial != null)
                unitRenderer.material = unselectedMaterial;
        }
    }
}