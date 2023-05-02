using UnityEngine;

namespace FroguesFramework
{
    public class EffectsVisualiser : MonoBehaviour
    {
        [SerializeField] private GameObject damageSuccessfullyBlockedEffect;
        [SerializeField] private GameObject blockDestroyedEffect;
        [SerializeField] private GameObject temporaryBlockIncreasedEffect;
        [SerializeField] private GameObject permanentBlockIncreasedEffect;
        
        public void Init(Unit unit)
        {
            unit.Health.OnDamageBlockedSuccessfully.AddListener(ShowDamageSuccessfullyBlockedEffect);
            unit.Health.OnBlockDestroyed.AddListener(ShowBlockDestroyedEffect);
            unit.Health.OnBlockIncreased.AddListener(ShowTemporaryBlockIncreasedEffect);
            //unit.Health.OnPermanentBlockIncreased.AddListener(ShowPermanentBlockIncreasedEffect);
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
        
        private void ShowTemporaryBlockIncreasedEffect()
        {
            temporaryBlockIncreasedEffect.SetActive(true);
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideTemporaryBlockIncreasedEffect), 0.8f);
        }
        
        private void HideTemporaryBlockIncreasedEffect()
        {
            CurrentlyActiveObjects.Remove(this);
            temporaryBlockIncreasedEffect.SetActive(false);
        }
        
        private void ShowPermanentBlockIncreasedEffect()
        {
            permanentBlockIncreasedEffect.SetActive(true);
            CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HidePermanentBlockIncreasedEffect), 1.1f);
        }
        
        private void HidePermanentBlockIncreasedEffect()
        {
            CurrentlyActiveObjects.Remove(this);
            permanentBlockIncreasedEffect.SetActive(false);
        }
    }
}