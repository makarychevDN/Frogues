using UnityEngine;

namespace FroguesFramework
{
    public class BottomPanelSizeController : MonoBehaviour
    {
        [SerializeField] private RectTransform abilityButtonsSlotsPanel;
        [SerializeField] private int sizeDelta;
        [SerializeField] private int slotsInTheRowQuantity;        

        public int SlotsInTheRowQuantity
        {
            get => slotsInTheRowQuantity;

            set
            {
                slotsInTheRowQuantity = value;
                abilityButtonsSlotsPanel.sizeDelta = new Vector2(CalculateWidth(), abilityButtonsSlotsPanel.sizeDelta.y);
            }
        }

        public void IncreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity += 1;

        public void DecreaseSlotsInTheRowQuantity() => SlotsInTheRowQuantity -= 1;

        private int CalculateWidth() => sizeDelta * slotsInTheRowQuantity;
    }
}