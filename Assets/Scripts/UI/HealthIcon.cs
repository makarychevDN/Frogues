using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class HealthIcon : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textField;
        [SerializeField] private IntContainer currentHealthContainer;
        [SerializeField] private IntContainer preTakenDamageContainer;
        [SerializeField] private IntContainer armorContainer;
        [SerializeField] private IntContainer blockContainer; //coming soon
        [SerializeField] private GameObject armoredHPIcon;

        void Update()
        {
            textField.text = currentHealthContainer.Content.ToString();
            textField.color = Color.white;
            armoredHPIcon.SetActive(false);

            if (preTakenDamageContainer != null && currentHealthContainer.Content != preTakenDamageContainer.Content)
            {
                textField.text = preTakenDamageContainer.Content.ToString();
                textField.color = Color.green;
            }

            armoredHPIcon.SetActive(armorContainer.Content > 0);
        }
    }
}
