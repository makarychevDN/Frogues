using UnityEngine;

namespace FroguesFramework
{
    public class EffectsVisualiser : MonoBehaviour
    {
        [SerializeField] private GameObject damageSuccessfullyBlockedEffect;
        [SerializeField] private GameObject blockDestroyedEffect;

        public void Init(Unit unit)
        {
            unit.Health.OnDamageBlockedSuccessfully.AddListener(ShowDamageSuccessfullyBlockedEffect);
            unit.Health.OnBlockDestroyed.AddListener(ShowBlockDestroyedEffect);
        }

        private void ShowDamageSuccessfullyBlockedEffect()
        {
            damageSuccessfullyBlockedEffect.SetActive(true);
            Invoke(nameof(HideSuccessfullyBlockedEffect), 0.2f);
        }
        
        private void HideSuccessfullyBlockedEffect()
        {
            damageSuccessfullyBlockedEffect.SetActive(false);
        }
        
        private void ShowBlockDestroyedEffect()
        {
            blockDestroyedEffect.SetActive(true);
            Invoke(nameof(HideBlockDestroyedEffect), 0.2f);
        }
        
        private void HideBlockDestroyedEffect()
        {
            blockDestroyedEffect.SetActive(false);
        }
    }
}