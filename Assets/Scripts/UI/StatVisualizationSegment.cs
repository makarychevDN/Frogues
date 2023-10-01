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

        [SerializeField] private string statIsPositiveColorCode = "#7B8C1B";
        [SerializeField] private string statIsNegativeColorCode = "#8C511B";

        [SerializeField] private GameObject effectIcon;

        private Color statIncresedColor;
        private Color statDecreasedColor;

        private void Awake()
        {
            ColorUtility.TryParseHtmlString(statIsPositiveColorCode, out statIncresedColor);
            ColorUtility.TryParseHtmlString(statIsNegativeColorCode, out statDecreasedColor);
            upArrow.GetComponent<Image>().color = statIncresedColor;
            downArrow.GetComponent<Image>().color = statDecreasedColor;
        }

        public void SetValue(int value)
        {
            secondDigitsParents.SetActive(false);

            var valueIsNegative = value < 0;
            upArrow.SetActive(!valueIsNegative);
            downArrow.SetActive(valueIsNegative);

            Color currentColor = valueIsNegative ? statDecreasedColor : statIncresedColor;

            var firstDigit = Mathf.Abs(value % 10);
            firstDigits[firstDigit].GetComponent<Image>().color = currentColor;
            for (int i = 0; i < firstDigits.Length; i++)
            {
                firstDigits[i].SetActive(firstDigit == i);
                firstDigits[i].GetComponent<Image>().color = currentColor;
            }

            if (Mathf.Abs(value) < 10)
                return;

            secondDigitsParents.SetActive(true);
            var secondDigit = Mathf.Abs(value / 10);
            secondDigits[firstDigit].GetComponent<Image>().color = currentColor;
            for (int i = 0; i < secondDigits.Length; i++)
            {
                secondDigits[i].SetActive(secondDigit == i);
            }
        }
    }
}