using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class StatVisualizationSegment : MonoBehaviour
    {
        [SerializeField] private GameObject upArrow;
        [SerializeField] private GameObject downArrow;

        [SerializeField] private GameObject[] firstDigits;
        [SerializeField] private GameObject[] secondDigits;
        [SerializeField] private GameObject secondDigitsParents;

        [SerializeField] private GameObject effectIcon;

        private Color statIncresedColor = new Color(123, 140, 27);
        private Color statDecreasedColor = new Color(140, 81, 27);

        private void Awake()
        {
            upArrow.GetComponent<Image>().color = statIncresedColor;
            downArrow.GetComponent<Image>().color = statDecreasedColor;
        }

        public void SetValue(int value)
        {
            secondDigitsParents.SetActive(false);

            var valueIsNegative = value < 0;
            upArrow.SetActive(!valueIsNegative);
            downArrow.SetActive(valueIsNegative);

            var firstDigit = value % 10;
            firstDigits[firstDigit].GetComponent<Image>().color = valueIsNegative ? statDecreasedColor : statIncresedColor;
            for (int i = 0; i < firstDigits.Length; i++)
            {
                firstDigits[i].SetActive(firstDigit == i);
            }

            if (value < 10)
                return;

            secondDigitsParents.SetActive(true);
            var secondDigit = value / 10;
            firstDigits[firstDigit].GetComponent<Image>().color = valueIsNegative ? statDecreasedColor : statIncresedColor;
            for (int i = 0; i < secondDigits.Length; i++)
            {
                firstDigits[i].SetActive(secondDigit == i);
            }
        }
    }
}