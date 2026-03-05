using UnityEngine;
using UnityEngine.UI;
using Scripts.Core;

namespace Scripts.UI
{
    public class HealthBarController : MonoBehaviour
    {
        [SerializeField] private Image _healthFillImage;
        [SerializeField] private CombatObject _combatObject;

        private void Start()
        {
            if (_combatObject == null)
            {
                _combatObject = GetComponentInParent<CombatObject>();
            }

            if (_combatObject != null)
            {
                _combatObject.OnHealthChanged += HandleHealthChanged;
                _combatObject.OnDeath += HandleDeath;
            }
        }

        private void OnDestroy()
        {
            if (_combatObject != null)
            {
                _combatObject.OnHealthChanged -= HandleHealthChanged;
                _combatObject.OnDeath -= HandleDeath;
            }
        }

        private void HandleHealthChanged(int current, int max)
        {
            if (_healthFillImage != null)
            {
                _healthFillImage.fillAmount = (float)current / max;
            }
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }
    }
}