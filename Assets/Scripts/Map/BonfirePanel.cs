using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class BonfirePanel : MonoBehaviour
    {
        [SerializeField] private List<BonfireButtonSetup> bonfireButtons;
        [SerializeField] private TMP_Text descriptionLabel;

        public void Init()
        {
            for(int i = 0; i < bonfireButtons.Count; i++)
            {
                bonfireButtons[i].button.onClick.AddListener(bonfireButtons[i].ApplyHealingAndScore);                
            }
        }

        private void OnEnable()
        {
            RecalculateFactHealingValues();
            descriptionLabel.text = "Здесь будет описан эффект вашего выбора";
        }

        public void RecalculateFactHealingValues()
        {
            for (int i = 0; i < bonfireButtons.Count; i++)
            {
                bonfireButtons[i].factHealingValue = (bonfireButtons[i].defaultHealingValue * EntryPoint.Instance.BonfireHealingMultiplierValue).RoundWithGameRules(true);
            }
        }

        public void SetTextOfDescroptionLabelByIndex(int index)
        {
            descriptionLabel.text = bonfireButtons[index].GetDescription();
        }
    }

    [Serializable]
    public class BonfireButtonSetup
    {
        public Button button;
        public int defaultHealingValue;
        public int factHealingValue;
        public int additionalScoreValue;

        public void ApplyHealingAndScore()
        {
            if(factHealingValue > 0)
                EntryPoint.Instance.MetaPlayer.Health.TakeHealing(factHealingValue);

            if (factHealingValue < 0)
                EntryPoint.Instance.MetaPlayer.Health.TakeDamage(-factHealingValue, null);

            EntryPoint.Instance.IncreaseScore(additionalScoreValue);
            EntryPoint.Instance.EnableBonfireRestPanel(false);
        }

        public string GetDescription()
        {
            string description = "";

            if(factHealingValue != 0)
            {
                string signModificator = factHealingValue > 0 ? "+" : "";
                description += $"{signModificator}{factHealingValue} хп ";
            }

            if(additionalScoreValue != 0)
            {
                description += $"{additionalScoreValue} очков ";
            }

            return description;
        }
    }
}