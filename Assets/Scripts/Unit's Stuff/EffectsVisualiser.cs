using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class EffectsVisualiser : MonoBehaviour
    {
        [Header("Block Effects")]
        [SerializeField] private GameObject damageSuccessfullyBlockedEffect;
        [SerializeField] private GameObject blockDestroyedByDamageEffect;
        [SerializeField] private GameObject blockDestroyedByRegenerationEffect;
        [SerializeField] private GameObject blockIncreasedEffect;
        [SerializeField] private GameObject blockAura;

        [Header("Armor Effects")]
        [SerializeField] private Animator armorImpactAnimator;
        [SerializeField] private AudioSource armorImpactSound;

        [Header("Stats Effects")]
        [SerializeField] private TMP_Text statEffectPrefab;
        [SerializeField] private List<TMP_Text> statEffectTextFields = new();
        [SerializeField] private Canvas canvas;
        private Unit _unit;
        
        public void Init(Unit unit)
        {
            _unit = unit;

            unit.Health.OnDamageBlocked.AddListener(ShowDamageSuccessfullyBlockedEffect);
            unit.Health.OnBlockIncreased.AddListener(ShowBlockIncreasedEffect);

            unit.Health.OnDamageReducedByArmor.AddListener(ShowArmorImpactEffect);
            unit.Health.OnArmorIncreased.AddListener(ShowArmorImpactEffect);

            unit.Health.OnBlockChargesCountUpdated.AddListener(() => EnableBlockAura(unit.Health.Block > 0));
            unit.Health.OnBlockDestroyedByRegeneration.AddListener(ShowBlockDestroyedByRegenerationEffect);
        }

        private void OnStatUpdated(string type, int delta)
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
            _unit.CurrentRoom.CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideSuccessfullyBlockedEffect), 0.8f);
        }

        private void EnableBlockAura(bool value)
        {
            blockAura.SetActive(value);
        }
        
        private void HideSuccessfullyBlockedEffect()
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Remove(this);
            damageSuccessfullyBlockedEffect.SetActive(false);
        }
        
        private void ShowBlockDestroyedByDamageEffect()
        {
            blockDestroyedByDamageEffect.SetActive(true);
            _unit.CurrentRoom.CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideBlockDestroyedByDamageEffect), 0.8f);
        }
        
        private void HideBlockDestroyedByDamageEffect()
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Remove(this);
            blockDestroyedByDamageEffect.SetActive(false);
        }
        
        private void ShowBlockIncreasedEffect()
        {
            blockIncreasedEffect.SetActive(true);
            _unit.CurrentRoom.CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideBlockIncreasedEffect), 0.8f);
        }
        
        private void HideBlockIncreasedEffect()
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Remove(this);
            blockIncreasedEffect.SetActive(false);
        }

        private void ShowBlockDestroyedByRegenerationEffect()
        {
            blockDestroyedByRegenerationEffect.SetActive(true);
            _unit.CurrentRoom.CurrentlyActiveObjects.Add(this);
            Invoke(nameof(HideBlockDestroyedByRegenerationEffect), 1f);
        }

        private void HideBlockDestroyedByRegenerationEffect()
        {
            _unit.CurrentRoom.CurrentlyActiveObjects.Remove(this);
            blockDestroyedByRegenerationEffect.SetActive(false);
        }

        private void ShowArmorImpactEffect()
        {
            armorImpactAnimator.SetTrigger("impact");
            armorImpactSound.Play();
        }
    }
}