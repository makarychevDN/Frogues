using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class EffectsVisualiser : MonoBehaviour
    {
        [SerializeField] private GameObject damageSuccessfullyBlockedEffect;
        [SerializeField] private GameObject blockDestroyedEffect;
        [SerializeField] private GameObject temporaryBlockIncreasedEffect;
        [SerializeField] private GameObject permanentBlockIncreasedEffect;
        [SerializeField] private TMP_Text statEffectPrefab;
        [SerializeField] private List<TMP_Text> statEffectTextFields = new();
        [SerializeField] private Canvas canvas;
        
        public void Init(Unit unit)
        {
            unit.Health.OnDamageBlockedSuccessfully.AddListener(ShowDamageSuccessfullyBlockedEffect);
            unit.Health.OnBlockDestroyed.AddListener(ShowBlockDestroyedEffect);
            unit.Health.OnBlockIncreased.AddListener(ShowTemporaryBlockIncreasedEffect);
            unit.Stats.OnStrenghtUpdated.AddListener(OnStatUpdated);
            unit.Stats.OnIntelegenceUpdated.AddListener(OnStatUpdated);
            unit.Stats.OnDexterityUpdated.AddListener(OnStatUpdated);
            unit.Stats.OnDefenceUpdated.AddListener(OnStatUpdated);
            unit.Stats.OnSpikesUpdated.AddListener(OnStatUpdated);
            unit.Stats.OnImmobilizedUpdated.AddListener(OnStatUpdated);
        }

        private void OnStatUpdated(StatEffectTypes type, int delta)
        {
            var currentTextEffect = statEffectTextFields.FirstOrDefault(textField => !textField.gameObject.activeSelf);

            if(currentTextEffect == null)
                statEffectTextFields.Add(currentTextEffect = Instantiate(statEffectPrefab, canvas.transform));

            currentTextEffect.gameObject.SetActive(true);
            currentTextEffect.text = delta > 0 ? $"<color=green>+{delta} {type}</color=green>" : $"<color=red>{delta} {type}</color=red>";
            StartCoroutine(HideEffect(currentTextEffect.gameObject));
        }

        private IEnumerator HideEffect(GameObject effect)
        {
            yield return new WaitForSeconds(1f);
            effect.SetActive(false);
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