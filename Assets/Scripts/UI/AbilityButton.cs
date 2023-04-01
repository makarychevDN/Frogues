using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilityButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private AbilityHint abilityHint;

        private bool _dragNow;
        private float maxDistanceToClamp = 64;
        private IAbility _ability;
        private AbilitiesPanel _abilitiesPanel;
        private bool _chosen;
        private Transform _currentButtonSlot;
        private bool _draggingNow;

        public IAbility Ability => _ability;
        
        public void Init(AbilitiesPanel abilitiesPanel, IAbility ability, IAbleToDrawAbilityButton ableToDrawAbilityButton)
        {
            _abilitiesPanel = abilitiesPanel;
            _ability = ability;
            image.material = ableToDrawAbilityButton.GetAbilityDataForButton().Material;

            if((ability as MonoBehaviour).GetComponent<MarkToAddAbilityInTheEndOfAbilitesPanel>() == null)
                _currentButtonSlot = abilitiesPanel.FirstEmptySlot();
            else
                _currentButtonSlot = abilitiesPanel.LastEmptySlot();

            transform.parent = _currentButtonSlot;
            transform.localPosition = Vector3.zero;
            abilityHint.Init(ableToDrawAbilityButton.GetAbilityDataForButton().AbilityName,
                ableToDrawAbilityButton.GetAbilityDataForButton().Stats,
                ableToDrawAbilityButton.GetAbilityDataForButton().Description);
            abilityHint.EnableContent(false);
        }
        
        public void PickAbility()
        {
            if(_draggingNow)
                return;
            
            if (_abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.GetCurrentAbility() == _ability)
            {
                _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
            }
            else
            {
                _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.SetCurrentAbility(_ability);
            }
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
            image.material.SetInt("_AbilityUsingNow", (_abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.GetCurrentAbility() == _ability).ToInt());
        }

        private void SetSlot(Transform slot)
        {
            _currentButtonSlot = slot;
            transform.parent = slot;
            transform.localPosition = Vector3.zero;
        }
    }
}