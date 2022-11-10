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
        

        public void Init(AbilitiesPanel abilitiesPanel,IAbility ability, IAbleToDrawAbilityButton ableToDrawAbilityButton)
        {
            _abilitiesPanel = abilitiesPanel;
            _ability = ability;
            image.sprite = ableToDrawAbilityButton.GetAbilityDataForButton().Sprite;
            transform.parent = abilitiesPanel.FirstEmptySlot();
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
            /*PlayerAbilityButtonSlot closestSlot = AbilitiesPanel.Instance.abilitySlots[0];
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
            }*/
        }
    }
}