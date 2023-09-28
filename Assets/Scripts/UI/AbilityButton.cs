using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace FroguesFramework
{
    public class AbilityButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Image cooldownEffect;
        [SerializeField] private AbilityHint abilityHint;
        [SerializeField] private TMP_Text cooldownCounterField;

        private float maxDistanceToClamp = 64;
        private BaseAbility _ability;
        private AbilitiesPanel _abilitiesPanel;
        private Transform _currentButtonSlot;
        private bool _draggingNow;
        private bool _myAbilityIsAbleToReturnIsPrevisualized;
        private bool _myAbilityIsAbleToHaveCooldown;
        private bool _myAbilityIsAbleToCost;
        private int _hashedCooldown;

        public BaseAbility Ability => _ability;
        
        public void Init(BaseAbility ability)
        {
            _ability = ability;
            image.material = ability.GetAbilityDataForButton().Material;

            transform.parent = _currentButtonSlot;
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;

            abilityHint.Init(ability.GetAbilityDataForButton().AbilityName,
                ability.GetAbilityDataForButton().Stats,
                ability.GetAbilityDataForButton().Description,
                ability is PassiveAbility);
            abilityHint.EnableContent(false);

            _myAbilityIsAbleToReturnIsPrevisualized = _ability is IAbleToReturnIsPrevisualized;
            _myAbilityIsAbleToHaveCooldown = _ability is IAbleToHaveCooldown;
            _myAbilityIsAbleToCost = _ability is IAbleToCost;

            if(_myAbilityIsAbleToHaveCooldown)
                _hashedCooldown = (_ability as IAbleToHaveCooldown).GetCooldownCounter();
        }

        public void Init(AbilitiesPanel abilitiesPanel, BaseAbility ability)
        {
            _abilitiesPanel = abilitiesPanel;

            if(ability is PassiveAbility)
            {
                _currentButtonSlot = abilitiesPanel.AddTopAbilitySlot();
            }
            else
            {
                _currentButtonSlot = abilitiesPanel.FirstEmptySlot();
            }

            if(ability is IAbleToHighlightAbilityButton)
            {
                ((IAbleToHighlightAbilityButton)ability).GetHighlightEvent().AddListener(EnableHighlight);
            }

            Init(ability);
        }

        private void EnableHighlight(bool value)
        {
            image.material.SetInt("_NeedToHighlight", value.ToInt());
        }

        public void PickAbility()
        {
            if(_draggingNow || _ability is not AbleToUseAbility)
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
            transform.parent = _currentButtonSlot.parent.parent;
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

            var abilitiesSlots = _ability is PassiveAbility ? _abilitiesPanel.PassiveAbilitySlots : _abilitiesPanel.ActiveAbilitySlots;
            foreach (var temp in abilitiesSlots)
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
            if (!_ability.HasOwner)
                return;

            bool myAbilityIsCooldowned = true;            
            if (_myAbilityIsAbleToHaveCooldown)
            {
                var abilityWithCooldown = _ability as IAbleToHaveCooldown;
                myAbilityIsCooldowned = abilityWithCooldown.IsCooldowned();

                if (abilityWithCooldown.GetCooldownCounter() != _hashedCooldown)
                {
                    if (abilityWithCooldown.GetCurrentCooldown() != 0)
                    {
                        cooldownEffect.gameObject.SetActive(true);
                        cooldownCounterField.text = abilityWithCooldown.GetCooldownCounter().ToString();
                        cooldownCounterField.enabled = abilityWithCooldown.GetCooldownCounter() != 0;
                        cooldownEffect.fillAmount = (float)abilityWithCooldown.GetCooldownCounter() / abilityWithCooldown.GetCurrentCooldown();
                        _hashedCooldown = abilityWithCooldown.GetCooldownCounter();
                    }
                    else
                    {
                        cooldownEffect.gameObject.SetActive(false);
                    }
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

        public void ResetButtonMaterial()
        {
            image.material.SetInt("_AbilityUsingNow", false.ToInt());
            image.material.SetInt("_AbleToUse", true.ToInt());
            image.material.SetInt("_NeedToHighlight", false.ToInt());
        }

        private void SetSlot(Transform slot)
        {
            _currentButtonSlot = slot;
            transform.parent = slot;
            transform.localPosition = Vector3.zero;
        }
    }
}