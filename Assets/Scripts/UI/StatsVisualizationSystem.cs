using System;
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
        [SerializeField] private AbilityDescriptionTag strenghtMechanicDescription;
        [SerializeField] private AbilityDescriptionTag intelligenceMehanicDescription;
        [SerializeField] private AbilityDescriptionTag dexterityMechanicDescription;
        [SerializeField] private AbilityDescriptionTag defenceMechanicDescription;
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

        public void ShowStrenghtHint() => ShowHint("Сила", strenghtMechanicDescription.DescriptionText, strenghtSegment.transform, (int)stats.StrenghtModificatorPersentages);
        public void ShowIntelligenceHint() => ShowHint("Интеллект", intelligenceMehanicDescription.DescriptionText, intelligenceSegment.transform, (int)stats.IntelegenceModificatorPersentages);
        public void ShowDexterityHint() => ShowHint("Ловкость", dexterityMechanicDescription.DescriptionText, dexteritySegment.transform, (int)stats.DexterityeModificatorPersentages);
        public void ShowDefenceHint() => ShowHint("Защита", defenceMechanicDescription.DescriptionText, defenceSegment.transform, (int)stats.DefenceModificatorPersentages);
        public void ShowImmobolizedHint() => ShowHint("Обездвиженность", immobilizedMechanicDescription.DescriptionText, immobilizedSegment.transform, stats.Immobilized);

        private void ShowHint(string header, string descriptionTag, Transform transformOfIcon, int value)
        {
            descriptionTag = descriptionTag.Replace("{value}", value.ToString());            
            EntryPoint.Instance.AbilityHint.Init(header, descriptionTag, "", transformOfIcon, new Vector2(0.5f, 0), Vector2.up * 36);
            EntryPoint.Instance.AbilityHint.EnableContent(true, true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}