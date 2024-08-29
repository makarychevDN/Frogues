using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class StatsVisualizationSystem : MonoBehaviour
    {
        [SerializeField] private Stats stats;
        [SerializeField] protected List<RectTransform> resizableParents;
        [SerializeField] private StatVisualizationSegment strengthSegment;
        [SerializeField] private StatVisualizationSegment immobilizedSegment;

        [Header("Localized mechanic names")]
        [SerializeField] private LocalizedString strengthMechanicName;
        [SerializeField] private LocalizedString immobolizedMechanicName;

        [Header("description tags")]
        [SerializeField] private AbilityDescriptionTag strengthMechanicDescriptionPositive;
        [SerializeField] private AbilityDescriptionTag strengthMechanicDescriptionNegative;

        [SerializeField] private AbilityDescriptionTag immobilizedMechanicDescription;

        private int lastStatsHash;

        public void SetStats(Stats stats)
        {
            if(this.stats != null)
                this.stats.OnSomethingUpdated.RemoveListener(RedrawIcons);

            this.stats = stats;
            this.stats.OnSomethingUpdated.AddListener(RedrawIcons);
        }

        void Update()
        {
            if(lastStatsHash != stats.CalculateHashFunctionOfPrevisualisation()) 
            {
                RedrawIcons();
            }

            lastStatsHash = stats.CalculateHashFunctionOfPrevisualisation();
        }

        private void OnEnable()
        {
            RedrawIcons();
        }

        private void RedrawIcons()
        {
            if (stats == null)
                return;

            strengthSegment.gameObject.SetActive(stats.Strenght != 0);
            strengthSegment.SetValue(stats.Strenght);

            immobilizedSegment.gameObject.SetActive(stats.Immobilized != 0);
            immobilizedSegment.SetValue(stats.Immobilized);

            resizableParents.ForEach(parent => LayoutRebuilder.ForceRebuildLayoutImmediate(parent));
        }

        public void ShowStrenghtHint() => ShowHint(strengthMechanicName.GetLocalizedString(), strengthMechanicDescriptionPositive.DescriptionText, strengthMechanicDescriptionNegative.DescriptionText, strengthSegment.transform, (int)stats.StrenghtModificatorPersentages, stats.Strenght);
        public void ShowImmobolizedHint() => ShowHint(immobolizedMechanicName.GetLocalizedString(), immobilizedMechanicDescription.DescriptionText, immobilizedMechanicDescription.DescriptionText, immobilizedSegment.transform, stats.Immobilized, 0);

        private void ShowHint(string header, string positiveDescriptionTag, string negativeDescriptionTag, Transform transformOfIcon, int modificatorStepValue, int statValue)
        {
            var text = statValue > 0 ? positiveDescriptionTag : negativeDescriptionTag;

            text = text.Replace("{step_value}", modificatorStepValue.ToString());
            text = text.Replace("{value}", statValue.ToString());
            text = text.Replace("{sum_value}", Mathf.Abs(statValue * modificatorStepValue).ToString());

            HintManager.Instance.CommonSmallHint.Init(header, text, transformOfIcon, new Vector2(0.5f, 0), Vector2.up * 36);
            HintManager.Instance.CommonSmallHint.EnableContent(true);
        }

        public void HideHint()
        {
            HintManager.Instance.CommonSmallHint.EnableContent(false);
        }
    }
}