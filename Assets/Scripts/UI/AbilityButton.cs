using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

        private BaseAbility _ability;
        private AbilityButtonSlot _currentButtonSlot;

        private bool _draggingNow;
        
        private bool _myAbilityIsAbleToReturnIsPrevisualized;
        private bool _myAbilityIsAbleToHaveCooldown;
        private bool _myAbilityIsAbleToCost;
        private int _hashedCooldown;

        public UnityEvent<AbleToUseAbility> OnAbilityPicked;
        public UnityEvent<AbilityButton> OnDragButton;
        public UnityEvent<AbilityButton> OnDropButton;

        public BaseAbility Ability => _ability;
        public AbilityButtonSlot AbilityButtonSlot
        {
            get => _currentButtonSlot;
            set => _currentButtonSlot = value;
        }

        public void Init(BaseAbility ability, AbilityButtonSlot abilityButtonSlot)
        {
            _ability = ability;
            _currentButtonSlot = abilityButtonSlot;
            image.material = ability.GetAbilityDataForButton().Material;

            _currentButtonSlot.AddButton(this);

            if (ability is IAbleToHighlightAbilityButton)
            {
                ((IAbleToHighlightAbilityButton)ability).GetHighlightEvent().AddListener(EnableHighlight);
            }

            _myAbilityIsAbleToReturnIsPrevisualized = _ability is IAbleToReturnIsPrevisualized;
            _myAbilityIsAbleToHaveCooldown = _ability is IAbleToHaveCooldown;
            _myAbilityIsAbleToCost = _ability is IAbleToCost;

            if (_myAbilityIsAbleToHaveCooldown)
                _hashedCooldown = (_ability as IAbleToHaveCooldown).GetCooldownCounter();

            abilityHint.Init(ability.GetAbilityDataForButton().AbilityName,
                ability.GetAbilityDataForButton().Stats,
                ability.GetAbilityDataForButton().Description,
                ability is PassiveAbility);
            abilityHint.EnableContent(false);
        }

        public void PickAbility()
        {
            if (_draggingNow || _ability is not AbleToUseAbility)
                return;

            var cooldownAbility = _ability as IAbleToHaveCooldown;
            if (cooldownAbility != null && !cooldownAbility.IsCooldowned())
                return;

            var ableToCostAbility = _ability as IAbleToCost;
            if (ableToCostAbility != null && !ableToCostAbility.IsResoursePointsEnough())
                return;

            OnAbilityPicked.Invoke(_ability as AbleToUseAbility);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDragButton.Invoke(this);
            transform.parent = _currentButtonSlot.transform.parent.parent;
            _draggingNow = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDropButton.Invoke(this);            
            /*var closestSlot = _currentButtonSlot;

            var abilitiesSlots = _ability is PassiveAbility ? _abilitiesPanel.PassiveAbilitySlots : _abilitiesPanel.ActiveAbilitySlots;
            foreach (var temp in abilitiesSlots)
            {
                if (Vector3.Distance(closestSlot.transform.position, transform.position) >
                    Vector3.Distance(temp.transform.position, transform.position))
                    closestSlot = temp;
            }
            
            if (Vector3.Distance(closestSlot.transform.position, transform.position) < maxDistanceToClamp)
            {
                _currentButtonSlot.Clear();

                if (!closestSlot.Empty)
                {
                    closestSlot.AbilityButton.SetSlot(_currentButtonSlot);
                }
                
                _currentButtonSlot = closestSlot;
            }            

            _currentButtonSlot.AddButton(this);*/
            _draggingNow = false;
        }

        public void SetSlot(AbilityButtonSlot slot)
        {
            _currentButtonSlot.Clear();
            _currentButtonSlot = slot;
            slot.AddButton(this);
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
                    if (abilityWithCooldown.GetCooldownCounter() != 0)
                    {
                        cooldownEffect.gameObject.SetActive(true);
                        cooldownCounterField.text = abilityWithCooldown.GetCooldownCounter().ToString();
                        cooldownCounterField.enabled = abilityWithCooldown.GetCooldownCounter() != 0;
                        cooldownEffect.fillAmount = (float)abilityWithCooldown.GetCooldownCounter() / abilityWithCooldown.GetCurrentCooldown();
                    }
                    else
                    {
                        cooldownEffect.gameObject.SetActive(false);
                    }
                }

                _hashedCooldown = abilityWithCooldown.GetCooldownCounter();
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

        private void EnableHighlight(bool value)
        {
            image.material.SetInt("_NeedToHighlight", value.ToInt());
        }
    }
}