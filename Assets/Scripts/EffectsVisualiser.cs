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
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideSuccessfullyBlockedEffect), 0.8f);
        }
        
        private void HideSuccessfullyBlockedEffect()
        {
            CurrentlyActiveObjects.Remove(this);
            damageSuccessfullyBlockedEffect.SetActive(false);
        }
        
        private void ShowBlockDestroyedEffect()
        {
            blockDestroyedEffect.SetActive(true);
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideBlockDestroyedEffect), 0.8f);
        }
        
        private void HideBlockDestroyedEffect()
        {
            CurrentlyActiveObjects.Remove(this);
            blockDestroyedEffect.SetActive(false);
        }
    }
}