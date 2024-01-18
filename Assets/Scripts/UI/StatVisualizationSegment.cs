using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class StatVisualizationSegment : MonoBehaviour
    {
        [SerializeField] private bool ignoreArrows;
        [SerializeField] private GameObject upArrow;
        [SerializeField] private GameObject downArrow;
        [SerializeField] private GameObject iconPrefab;
        [SerializeField] private Transform parentForSpwanedIcons;

        [SerializeField] private string statIsPositiveColorCode = "#7B8C1B";
        [SerializeField] private string statIsNegativeColorCode = "#8C511B";

        private Color statIncresedColor;
        private Color statDecreasedColor;
        private List<Image> statIcons = new();

        private void Awake()
        {
            ColorUtility.TryParseHtmlString(statIsPositiveColorCode, out statIncresedColor);
            ColorUtility.TryParseHtmlString(statIsNegativeColorCode, out statDecreasedColor);
            upArrow.GetComponent<Image>().color = statIncresedColor;
            downArrow.GetComponent<Image>().color = statDecreasedColor;
        }

        public void SetValue(int value)
        {
            var valueIsNegative = value < 0;
            Color currentColor = valueIsNegative ? statDecreasedColor : statIncresedColor;
            value = Mathf.Abs(value);

            if (!ignoreArrows)
            {
                upArrow.SetActive(!valueIsNegative);
                downArrow.SetActive(valueIsNegative);
            }

            if (statIcons.Count < value)
            {
                while(statIcons.Count < value)
                {
                    var newIcon = Instantiate(iconPrefab, parentForSpwanedIcons);
                    statIcons.Add(newIcon.GetComponentInChildren<Image>());
                }
            }

            for (int i = 0; i < statIcons.Count; i++)
            {
                statIcons[i].color = currentColor;
                statIcons[i].transform.parent.gameObject.SetActive(i < value);
            }
        }
    }
}