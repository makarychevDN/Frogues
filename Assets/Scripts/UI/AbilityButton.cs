using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilityButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Image cooldownEffect;
        [SerializeField] private TMP_Text cooldownCounterField;
        [SerializeField] private IntSpriteFontSegment chargesCounter;
        [SerializeField] private List<TMP_Text> chargesCounterElements;
        [SerializeField] private AbilityHint abilityHint;
        [SerializeField] private AudioSource putInTheSlotSound;
        [SerializeField] private AudioSource putOutOfTheSlotSound;

        private BaseAbility _ability;
        private AbilityButtonSlot _currentButtonSlot;
        private Transform _parentToDragAndDropProcess;

        private bool _draggingNow;        
        private bool _myAbilityIsAbleToReturnIsPrevisualized;
        private bool _myAbilityIsAbleToHaveCooldown;
        private bool _myAbilityIsAbleToCost;
        private int _hashedCooldown;
        private bool _isInteractable;
        private bool _isbuttonIsAbleToDisplayStatesOfAbility;

        private Vector2 _positionOfHintRelativeToButton;
        private Vector2 _pivotOfHintRectTransformWhenHover;

        public UnityEvent<AbleToUseAbility> OnAbilityPicked;
        public UnityEvent<AbilityButton> OnDragButton;
        public UnityEvent<AbilityButton> OnDropButton;

        public BaseAbility Ability => _ability;
        public AbilityButtonSlot AbilityButtonSlot
        {
            get => _currentButtonSlot;
            set => _currentButtonSlot = value;
        }

        public void Init(BaseAbility ability, AbilityButtonSlot abilityButtonSlot, bool isInteractable, bool buttonIsAbleToDisplayStatesOfAbility, Vector2 pivotOfHintRectTransformWhenHover, Vector2 positionOfHintRelativeToButton, Transform parentToDragAndDropProcess = null)
        {
            _isInteractable = isInteractable;
            _parentToDragAndDropProcess = parentToDragAndDropProcess;
            _ability = ability;
            _currentButtonSlot = abilityButtonSlot;
            _pivotOfHintRectTransformWhenHover = pivotOfHintRectTransformWhenHover;
            _positionOfHintRelativeToButton = positionOfHintRelativeToButton;
            _isbuttonIsAbleToDisplayStatesOfAbility = buttonIsAbleToDisplayStatesOfAbility;
            image.material = new Material(ability.GetAbilityDataForButton().Material);

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

            OnDragButton.AddListener(_ => putOutOfTheSlotSound.Play());
            OnDropButton.AddListener(_ => putInTheSlotSound.Play());
        }

        public void PickAbility()
        {
            if (!_isInteractable)
                return;

            if (_draggingNow || _ability is not AbleToUseAbility)
                return;

            var cooldownAbility = _ability as IAbleToHaveCooldown;
            if (cooldownAbility != null && !cooldownAbility.IsEnoughCharges())
                return;

            var ableToCostAbility = _ability as IAbleToCost;
            if (ableToCostAbility != null && !ableToCostAbility.IsResoursePointsEnough())
                return;

            OnAbilityPicked.Invoke(_ability as AbleToUseAbility);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_isInteractable)
                return;

            EntryPoint.Instance.AbilityHint.EnableContent(false);
            OnDragButton.Invoke(this);
            transform.parent = _parentToDragAndDropProcess;
            _draggingNow = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isInteractable)
                return;

            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isInteractable)
                return;

            OnDropButton.Invoke(this);            
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
            if (!_ability.HasOwner || !_isbuttonIsAbleToDisplayStatesOfAbility)
                return;

            bool myAbilityIsCooldowned = true;            
            if (_myAbilityIsAbleToHaveCooldown)
            {
                var abilityWithCooldown = _ability as IAbleToHaveCooldown;
                myAbilityIsCooldowned = abilityWithCooldown.IsEnoughCharges();

                //chargesCounter.gameObject.SetActive(abilityWithCooldown.GetCurrentCharges() > 1);
                //chargesCounter.SetValue(abilityWithCooldown.GetCurrentCharges());

                foreach(var textElement in chargesCounterElements)
                {
                    textElement.gameObject.SetActive(abilityWithCooldown.GetCurrentCharges() > 1);
                    textElement.SetText(abilityWithCooldown.GetCurrentCharges().ToString());
                }

                if (abilityWithCooldown.GetCooldownCounter() != _hashedCooldown)
                {
                    if (abilityWithCooldown.GetCooldownCounter() != 0)
                    {
                        cooldownEffect.gameObject.SetActive(true);
                        cooldownCounterField.gameObject.SetActive(abilityWithCooldown.GetCurrentCharges() == 0);
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            var data = _ability.GetAbilityDataForButton();
            EntryPoint.Instance.AbilityHint.Init(data.AbilityName, data.ShortData, data.Description, transform, _pivotOfHintRectTransformWhenHover, _positionOfHintRelativeToButton);
            EntryPoint.Instance.AbilityHint.EnableContent(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}