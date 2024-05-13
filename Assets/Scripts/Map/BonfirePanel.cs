using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class BonfirePanel : MonoBehaviour
    {
        [SerializeField] private List<BonfireButtonSetup> bonfireButtons;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private LocalizedString defaultEffectLabelValue;
        [SerializeField] private LocalizedString localizedScorePointsString;
        [SerializeField] private LocalizedString localizedHealthPointsString;

        public void Init()
        {
            for(int i = 0; i < bonfireButtons.Count; i++)
            {
                bonfireButtons[i].button.onClick.AddListener(bonfireButtons[i].ApplyHealingAndScore);                
            }

            EntryPoint.Instance.MetaPlayer.Health.OnHpHealed.AddListener(RecalculateFactHealingValues);
        }

        private void OnEnable()
        {
            RecalculateFactHealingValues();
            descriptionLabel.text = defaultEffectLabelValue.GetLocalizedString();
        }

        public void RecalculateFactHealingValues()
        {
            for (int i = 0; i < bonfireButtons.Count; i++)
            {
                int bonfireHealingValue = (bonfireButtons[i].defaultPersentagesHealingValue * EntryPoint.Instance.MetaPlayer.Health.MaxHp * 0.01f).RoundWithGameRules(true);
                bonfireButtons[i].factHealingValue = bonfireHealingValue > 0 ? bonfireHealingValue + EntryPoint.Instance.AdditionalHealingValue : bonfireHealingValue;
            }
        }

        public void SetTextOfDescroptionLabelByIndex(int index)
        {
            descriptionLabel.text = bonfireButtons[index].GetDescription(localizedScorePointsString, localizedHealthPointsString);
        }
    }

    [Serializable]
    public class BonfireButtonSetup
    {
        public Button button;
        public float defaultPersentagesHealingValue;
        public int factHealingValue;
        public int additionalScoreValue;

        public void ApplyHealingAndScore()
        {
            if(factHealingValue > 0)
                EntryPoint.Instance.MetaPlayer.Health.TakeHealing(factHealingValue);

            if (factHealingValue < 0)
                EntryPoint.Instance.MetaPlayer.Health.TakeDamage(-factHealingValue, true, null);

            EntryPoint.Instance.IncreaseScore(additionalScoreValue, true);
            EntryPoint.Instance.EnableBonfireRestPanel(false);
        }

        public string GetDescription(LocalizedString localizedScorePointsString, LocalizedString localizedHealthPointsString)
        {
            string description = "";

            if(factHealingValue != 0)
            {
                string signModificator = factHealingValue > 0 ? "+" : "";
                description += $"{signModificator}{factHealingValue} {localizedHealthPointsString.GetLocalizedString()} ";
            }

            if(additionalScoreValue != 0)
            {                
                description += $"+{additionalScoreValue} {localizedScorePointsString.GetLocalizedString()} ";
            }

            return description;
        }
    }
}