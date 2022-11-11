using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class AbilityButton : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Image image;

        private bool _dragNow;
        private float maxDistanceToClamp = 64;
        private IAbility _ability;
        private AbilitiesPanel _abilitiesPanel;
        private bool _chosen;
        private Transform _currentButtonSlot;
        

        public void Init(AbilitiesPanel abilitiesPanel,IAbility ability, IAbleToDrawAbilityButton ableToDrawAbilityButton)
        {
            _abilitiesPanel = abilitiesPanel;
            _ability = ability;
            image.sprite = ableToDrawAbilityButton.GetAbilityDataForButton().Sprite;
            _currentButtonSlot = abilitiesPanel.FirstEmptySlot();
            transform.parent = _currentButtonSlot;
            transform.localPosition = Vector3.zero;
        }
        
        public void PickAbility()
        {
            /*if (_dragNow)
            {
                _dragNow = false;
                return;
            }*/
            
            if(_abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.GetCurrentAbility() == _ability)
                _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
            
            else
                _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.SetCurrentAbility(_ability);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _abilitiesPanel.AbilitiesManager.AbleToHaveCurrentAbility.ClearCurrentAbility();
            transform.parent = _currentButtonSlot.parent;
            /*onDragEvent.Invoke();
            _dragNow = true;
            transform.parent = slot.transform.parent;
            slot.Content = null;
            //_playerInput.currentAbility = null;*/
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
                transform.parent = closestSlot;
            }
            
            transform.localPosition = Vector3.zero;
        }

        private void SetSlot(Transform slot)
        {
            _currentButtonSlot = slot;
            transform.parent = slot;
            transform.localPosition = Vector3.zero;
        }
    }
}