using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilityButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Image cooldownEffect;
        [SerializeField] private AbilityHint abilityHint;
        [SerializeField] private TMP_Text cooldownCounterField;

        private bool _dragNow;
        private float maxDistanceToClamp = 64;
        private BaseAbility _ability;
        private AbilitiesPanel _abilitiesPanel;
        private bool _chosen;
        private Transform _currentButtonSlot;
        private bool _draggingNow;
        private bool _myAbilityIsAbleToReturnIsPrevisualized;
        private bool _myAbilityIsAbleToHaveCooldown;
        private bool _myAbilityIsAbleToCost;
        private int _hashedCooldown;

        public BaseAbility Ability => _ability;
        
        public void Init(AbilitiesPanel abilitiesPanel, BaseAbility ability)
        {
            _abilitiesPanel = abilitiesPanel;
            _ability = ability;
            image.material = ability.GetAbilityDataForButton().Material;

            if((ability).GetComponent<MarkToAddAbilityInTheEndOfAbilitesPanel>() == null)
                _currentButtonSlot = abilitiesPanel.FirstEmptySlot();
            else
                _currentButtonSlot = abilitiesPanel.LastEmptySlot();

            transform.parent = _currentButtonSlot;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            abilityHint.Init(ability.GetAbilityDataForButton().AbilityName,
                ability.GetAbilityDataForButton().Stats,
                ability.GetAbilityDataForButton().Description);
            abilityHint.EnableContent(false);

            _myAbilityIsAbleToReturnIsPrevisualized = _ability is IAbleToReturnIsPrevisualized;
            _myAbilityIsAbleToHaveCooldown = _ability is IAbleToHaveCooldown;
            _myAbilityIsAbleToCost = _ability is IAbleToCost;
            _hashedCooldown = (_ability as IAbleToHaveCooldown).GetCooldownCounter();
        }
        
        public void PickAbility()
        {
            if(_draggingNow)
                return;
            
            if (_abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.GetCurrentAbility() == _ability)
            {
                _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
                return;
            }

            var cooldownAbility = _ability as IAbleToHaveCooldown;
            if (cooldownAbility != null && !cooldownAbility.IsCooldowned())
                return;

            var ableToCostAbility = _ability as IAbleToCost;
            if (ableToCostAbility != null && !ableToCostAbility.IsResoursePointsEnough())
                return;

            _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.SetCurrentAbility(_ability);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
            transform.parent = _currentButtonSlot.parent;
            _draggingNow = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
            
            var closestSlot = _currentButtonSlot;

            foreach (var temp in _abilitiesPanel.AbilitySlots)
            {
                if (Vector3.Distance(closestSlot.transform.position, transform.position) >
                    Vector3.Distance(temp.transform.position, transform.position))
                    closestSlot = temp;
            }
            
            if (Vector3.Distance(closestSlot.transform.position, transform.position) < maxDistanceToClamp)
            {
                if (closestSlot.childCount != 0)
                {
                    closestSlot.GetComponentInChildren<AbilityButton>().SetSlot(_currentButtonSlot);
                }
                
                _currentButtonSlot = closestSlot;
            }
            
            transform.parent = _currentButtonSlot;
            transform.localPosition = Vector3.zero;
            _draggingNow = false;
        }

        private void Update()
        {
            bool myAbilityIsCooldowned = true;            
            if (_myAbilityIsAbleToHaveCooldown)
            {
                var abilityWithCooldown = _ability as IAbleToHaveCooldown;
                myAbilityIsCooldowned = abilityWithCooldown.IsCooldowned();

                if (abilityWithCooldown.GetCooldownCounter() != _hashedCooldown)
                {
                    cooldownCounterField.text = abilityWithCooldown.GetCooldownCounter().ToString();
                    cooldownCounterField.enabled = abilityWithCooldown.GetCooldownCounter() != 0;
                    cooldownEffect.fillAmount = (float)abilityWithCooldown.GetCooldownCounter() / abilityWithCooldown.GetCooldownAfterStart();
                    _hashedCooldown = abilityWithCooldown.GetCooldownCounter();
                }
            }

            bool myAbilityIsHasEnoughResourses = true;
            if (_myAbilityIsAbleToCost)
                myAbilityIsHasEnoughResourses = (_ability as IAbleToCost).IsResoursePointsEnough();

            image.material.SetInt("_AbleToUse",
                (myAbilityIsCooldowned && myAbilityIsHasEnoughResourses).ToInt());

            if (_myAbilityIsAbleToReturnIsPrevisualized)
            {
                image.material.SetInt("_AbilityUsingNow",
                    (_ability as IAbleToReturnIsPrevisualized).IsPrevisualizedNow().ToInt());
            }
        }

        private void SetSlot(Transform slot)
        {
            _currentButtonSlot = slot;
            transform.parent = slot;
            transform.localPosition = Vector3.zero;
        }
    }
}