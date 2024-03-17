using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class StatsVisualizationSystem : MonoBehaviour
    {
        [SerializeField] private Stats stats;
        [SerializeField] protected List<RectTransform> resizableParents;
        [SerializeField] private StatVisualizationSegment strenghtSegment;
        [SerializeField] private StatVisualizationSegment intelligenceSegment;
        [SerializeField] private StatVisualizationSegment dexteritySegment;
        [SerializeField] private StatVisualizationSegment defenceSegment;
        [SerializeField] private StatVisualizationSegment immobilizedSegment;

        [Header("description tags")]
        [SerializeField] private AbilityDescriptionTag strenghtMechanicDescriptionPositive;
        [SerializeField] private AbilityDescriptionTag strenghtMechanicDescriptionNegative;

        [SerializeField] private AbilityDescriptionTag intelligenceMehanicDescriptionPositive;
        [SerializeField] private AbilityDescriptionTag intelligenceMehanicDescriptionNegative;

        [SerializeField] private AbilityDescriptionTag dexterityMechanicDescriptionPositive;
        [SerializeField] private AbilityDescriptionTag dexterityMechanicDescriptionNegative;

        [SerializeField] private AbilityDescriptionTag defenceMechanicDescriptionPositive;
        [SerializeField] private AbilityDescriptionTag defenceMechanicDescriptionNegative;

        [SerializeField] private AbilityDescriptionTag immobilizedMechanicDescription;

        private int lastStatsHash;

        public void SetStats(Stats stats)
        {
            this.stats = stats;
        }

        void Update()
        {
            if(lastStatsHash != stats.CalculateHashFunctionOfPrevisualisation()) 
            {
                RedrawIcons();
            }

            lastStatsHash = stats.CalculateHashFunctionOfPrevisualisation();
        }

        private void RedrawIcons()
        {
            strenghtSegment.gameObject.SetActive(stats.Strenght != 0);
            strenghtSegment.SetValue(stats.Strenght);

            intelligenceSegment.gameObject.SetActive(stats.Intelegence != 0);
            intelligenceSegment.SetValue(stats.Intelegence);

            dexteritySegment.gameObject.SetActive(stats.Dexterity != 0);
            dexteritySegment.SetValue(stats.Dexterity);

            defenceSegment.gameObject.SetActive(stats.Defence != 0);
            defenceSegment.SetValue(stats.Defence);

            immobilizedSegment.gameObject.SetActive(stats.Immobilized != 0);
            immobilizedSegment.SetValue(stats.Immobilized);

            resizableParents.ForEach(parent => LayoutRebuilder.ForceRebuildLayoutImmediate(parent));
        }

        public void ShowStrenghtHint() => ShowHint("Сила", strenghtMechanicDescriptionPositive.DescriptionText, strenghtMechanicDescriptionNegative.DescriptionText, strenghtSegment.transform, (int)stats.StrenghtModificatorPersentages, stats.Strenght);
        public void ShowIntelligenceHint() => ShowHint("Интеллект", intelligenceMehanicDescriptionPositive.DescriptionText, intelligenceMehanicDescriptionNegative.DescriptionText, intelligenceSegment.transform, (int)stats.IntelegenceModificatorPersentages, stats.Intelegence);
        public void ShowDexterityHint() => ShowHint("Ловкость", dexterityMechanicDescriptionPositive.DescriptionText, dexterityMechanicDescriptionNegative.DescriptionText, dexteritySegment.transform, (int)stats.DexterityeModificatorPersentages, stats.Dexterity);
        public void ShowDefenceHint() => ShowHint("Защита", defenceMechanicDescriptionPositive.DescriptionText, defenceMechanicDescriptionNegative.DescriptionText, defenceSegment.transform, (int)stats.DefenceModificatorPersentages, stats.Defence);
        public void ShowImmobolizedHint() => ShowHint("Обездвиженность", immobilizedMechanicDescription.DescriptionText, immobilizedMechanicDescription.DescriptionText, immobilizedSegment.transform, stats.Immobilized, 0);

        private void ShowHint(string header, string positiveDescriptionTag, string negativeDescriptionTag, Transform transformOfIcon, int modificatorStepValue, int statValue)
        {
            var text = statValue > 0 ? positiveDescriptionTag : negativeDescriptionTag;

            text = text.Replace("{step value}", modificatorStepValue.ToString());
            text = text.Replace("{value}", statValue.ToString());
            text = text.Replace("{sum value}", Mathf.Abs(statValue * modificatorStepValue).ToString());            

            EntryPoint.Instance.AbilityHint.Init(header, text, "", transformOfIcon, new Vector2(0.5f, 0), Vector2.up * 36);
            EntryPoint.Instance.AbilityHint.EnableContent(true, true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}