using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilityButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public PlayerAbilityButtonSlot slot;
        [SerializeField] private Ability ability;
        [SerializeField] private bool usingNow;

        public UnityEvent onDragEvent;
        public UnityEvent onDropEvent;

        private PlayerInput _playerInput;
        private Material _material;
        private bool _dragNow;
        private float maxDistanceToClamp = 64;
        private ActionPointsIconsShaker _actionPointsIconsShaker;

        private void Awake()
        {
            _material = GetComponent<Image>().material;
            _playerInput = FindObjectOfType<PlayerInput>();
            _actionPointsIconsShaker = FindObjectOfType<ActionPointsIconsShaker>();
        }

        public void SetAbility(Ability ability)
        {
            this.ability = ability;
        }

        private void Update()
        {
            if (_playerInput == null)
                return;

            //_material.SetInt("_AbilityUsingNow", (_playerInput.currentAbility == ability) ? 1 : 0);
        }

        public void PickAbility()
        {
            if (_dragNow)
            {
                _dragNow = false;
                return;
            }

            if (ability.IsActionPointsEnough()){}
                //_playerInput.currentAbility = ability;
            else
            {
                _actionPointsIconsShaker.Shake();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onDragEvent.Invoke();
            _dragNow = true;
            transform.parent = slot.transform.parent;
            slot.Content = null;
            //_playerInput.currentAbility = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            PlayerAbilityButtonSlot closestSlot = AbilitiesPanel.Instance.abilitySlots[0];
            onDropEvent.Invoke();

            foreach (var temp in AbilitiesPanel.Instance.abilitySlots)
            {
                if (Vector3.Distance(closestSlot.transform.position, transform.position) >
                    Vector3.Distance(temp.transform.position, transform.position))
                    closestSlot = temp;
            }

            if (Vector3.Distance(closestSlot.transform.position, transform.position) < maxDistanceToClamp)
            {
                closestSlot.Content = this;
            }
            else
            {
                slot.Content = this;
            }
        }
    }
}