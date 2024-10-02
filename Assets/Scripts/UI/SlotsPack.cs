using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class SlotsPack : MonoBehaviour
    {
        [SerializeField] private AbilityButtonSlot slotPrefab;
        [SerializeField] private List<AbilityButtonSlot> slots = new();
        [SerializeField] private Transform slotsParent;
        private AbilitiesManager abilitiesManager;

        public void Init(AbilitiesManager abilitiesManager)
        {
            print(abilitiesManager);
            this.abilitiesManager = abilitiesManager;
            abilitiesManager.OnAbilityHasBeenAdded.AddListener(UpdateSlotsQuantity);
            abilitiesManager.OnAbilityHasBeenRemoved.AddListener(UpdateSlotsQuantity);
        }

        public void UpdateSlotsQuantity(BaseAbility baseAbility)
        {
            print(abilitiesManager);
            print(slots.ObjectsWithActiveGameObjects());

            while(slots.ObjectsWithActiveGameObjects().Count() < abilitiesManager.Abilities.Count) 
            {
                AddSlot();
            }

            while(slots.ObjectsWithActiveGameObjects().Count() > abilitiesManager.Abilities.Count) 
            {
                RemoveSlot();
            }
        }

        private void AddSlot()
        {
            var slotToAdd = slots.FirstOrDefault(icon => !icon.gameObject.activeSelf);

            if (slotToAdd == null)
                slots.Add(slotToAdd = Instantiate(slotPrefab, slotsParent));

            slotToAdd.gameObject.SetActive(true);
        }

        private void RemoveSlot()
        {
            slots.ObjectsWithActiveGameObjects().ToList().GetLast().gameObject.SetActive(false);
        }
    }
}