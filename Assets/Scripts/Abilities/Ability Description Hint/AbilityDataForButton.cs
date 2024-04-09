using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FroguesFramework
{
    [RequireComponent(typeof(BaseAbility))]
    public class AbilityDataForButton : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private string abilityName;
        [SerializeField] private List<AbilityDescriptionTag> shortDataTags;
        [SerializeField] private List<AbilityDescriptionTag> descriptionTags;
        private BaseAbility ability;
        private Dictionary<string, Func<string>> dataByKeyWords = new Dictionary<string, Func<string>>();

        public Material Material => material;
        public string AbilityName => abilityName;
        public string ShortData => GetShortData();
        public string Description => GetDescription();

        private void Awake()
        {
            ability = GetComponent<BaseAbility>();

            dataByKeyWords.Add("{value}", () => (ability as IAbleToReturnSingleValue).GetValue().ToString());
            dataByKeyWords.Add("{value_2}", () => (ability as IAbleToReturnSecondSingleValue).GetSecondValue().ToString());

            dataByKeyWords.Add("{max_blood_points_mod}", () => (ability as IAbleToModifyMaxBloodPoints).GetModificatorForMaxBloodPoints().ToString());
            dataByKeyWords.Add("{max_hp_mod}", () => (ability as IAbleToModifyMaxHP).GetModificatorForMaxHP().ToString());

            dataByKeyWords.Add("{range}", () => (ability as IAbleToReturnRange).ReturnRange().ToString());
            dataByKeyWords.Add("{alternative_range}", () => (ability as IAbleToHaveAlternativeRange).GetAlternativeRange().ToString());

            dataByKeyWords.Add("{cooldown_after_use}", () => (ability as IAbleToHaveCooldown).GetCooldownAfterUse().ToString());
            dataByKeyWords.Add("{cooldown_after_start}", () => (ability as IAbleToHaveCooldown).GetCooldownAfterStart().ToString());
            dataByKeyWords.Add("{max_charges}", () => (ability as IAbleToHaveCooldown).GetMaxCharges().ToString());

            dataByKeyWords.Add("{action_points_cost}", () => (ability as IAbleToCost).GetActionPointsCost().ToString());
            dataByKeyWords.Add("{blood_points_cost}", () => (ability as IAbleToCost).GetBloodPointsCost().ToString());
            dataByKeyWords.Add("{health_points_cost}", () => (ability as IAbleToCost).GetHealthCost().ToString());

            dataByKeyWords.Add("{default_damage_value}", () => (ability as IAbleToDealDamage).GetDefaultDamage().ToString());
            dataByKeyWords.Add("{calculated_damage_value}", () => IntToStringByCompareValues((ability as IAbleToDealDamage).CalculateDamage(), (ability as IAbleToDealDamage).GetDefaultDamage()));
            dataByKeyWords.Add("{damage_type}", () => (ability as IAbleToDealDamage).GetDamageType().ToString());

            dataByKeyWords.Add("{alternative_default_damage_value}", () => (ability as IAbleToDealAlternativeDamage).GetDefaultAlternativeDamage().ToString());
            dataByKeyWords.Add("{calculated_alternative_damage_value}", () => IntToStringByCompareValues((ability as IAbleToDealAlternativeDamage).CalculateAlternativeDamage(), (ability as IAbleToDealAlternativeDamage).GetDefaultAlternativeDamage()));
            dataByKeyWords.Add("{alternative_damage_type}", () => (ability as IAbleToDealAlternativeDamage).GetAlternativeDamageType().ToString());

            dataByKeyWords.Add("{current_default_damage_value}", () => (ability as IAbleToReturnCurrentDamage).GetDefaultCurrentDamage().ToString());
            dataByKeyWords.Add("{calculated_current_damage_value}", () => IntToStringByCompareValues((ability as IAbleToReturnCurrentDamage).GetCalculatedCurrentDamage(), (ability as IAbleToReturnCurrentDamage).GetDefaultCurrentDamage()));

            dataByKeyWords.Add("{effect_value}", () => (ability as IAbleToApplyAnyModificator).GetModificatorValue().ToString());
            dataByKeyWords.Add("{effect_delta}", () => (ability as IAbleToApplyAnyModificator).GetDeltaValueForEachTurn().ToString());
            dataByKeyWords.Add("{effect_time}", () => (ability as IAbleToApplyAnyModificator).GetTimeToEndOfEffect().ToString());
            dataByKeyWords.Add("{effect_constantly}", () => (ability as IAbleToApplyAnyModificator).GetEffectIsConstantly().ToString());

            dataByKeyWords.Add("{defence_effect_value}", () => (ability as IAbleToApplyDefenceModificator).GetDefenceModificatorValue().ToString());
            dataByKeyWords.Add("{defence_effect_delta}", () => (ability as IAbleToApplyDefenceModificator).GetdeltaOfDefenceValueForEachTurn().ToString());
            dataByKeyWords.Add("{defence_effect_time}", () => (ability as IAbleToApplyDefenceModificator).GetTimeToEndOfDefenceEffect().ToString());
            dataByKeyWords.Add("{defence_ffect_constantly}", () => (ability as IAbleToApplyDefenceModificator).GetDefenceEffectIsConstantly().ToString());

            dataByKeyWords.Add("{strenght_effect_value}", () => (ability as IAbleToApplyStrenghtModificator).GetStrenghtModificatorValue().ToString());
            dataByKeyWords.Add("{strenght_effect_delta}", () => (ability as IAbleToApplyStrenghtModificator).GetDeltaOfStrenghtValueForEachTurn().ToString());
            dataByKeyWords.Add("{strenght_effect_time}", () => (ability as IAbleToApplyStrenghtModificator).GetTimeToEndOfStrenghtEffect().ToString());
            dataByKeyWords.Add("{strenght_effect_constantly}", () => (ability as IAbleToApplyStrenghtModificator).GetStrenghtEffectIsConstantly().ToString());

            dataByKeyWords.Add("{intelligence_effect_value}", () => (ability as IAbleToApplyIntelligenceModificator).GetIntelligenceModificatorValue().ToString());
            dataByKeyWords.Add("{intelligence_effect_delta}", () => (ability as IAbleToApplyIntelligenceModificator).GetDeltaOfIntelligenceValueForEachTurn().ToString());
            dataByKeyWords.Add("{intelligence_effect_time}", () => (ability as IAbleToApplyIntelligenceModificator).GetTimeToEndOfIntelligenceEffect().ToString());
            dataByKeyWords.Add("{intelligence_effect_constantly}", () => (ability as IAbleToApplyIntelligenceModificator).GetIntelligenceEffectIsConstantly().ToString());

            dataByKeyWords.Add("{dexterity_effect_value}", () => (ability as IAbleToApplyDexterityModificator).GetDexterityModificatorValue().ToString());
            dataByKeyWords.Add("{dexterity_effect_delta}", () => (ability as IAbleToApplyDexterityModificator).GetDeltaOfDexterityValueForEachTurn().ToString());
            dataByKeyWords.Add("{dexterity_effect_time}", () => (ability as IAbleToApplyDexterityModificator).GetTimeToEndOfDexterityEffect().ToString());
            dataByKeyWords.Add("{dexterity_effect_constantly}", () => (ability as IAbleToApplyDexterityModificator).GetDexterityEffectIsConstantly().ToString());

            dataByKeyWords.Add("{spikes_effect_value}", () => (ability as IAbleToApplySpikesModificator).GetSpikesModificatorValue().ToString());
            dataByKeyWords.Add("{spikes_effect_delta}", () => (ability as IAbleToApplySpikesModificator).GetdeltaOfSpikesValueForEachTurn().ToString());
            dataByKeyWords.Add("{spikes_effect_time}", () => (ability as IAbleToApplySpikesModificator).GetTimeToEndOfSpikesEffect().ToString());
            dataByKeyWords.Add("{spikes_effect_constantly}", () => (ability as IAbleToApplySpikesModificator).GetSpikesEffectIsConstantly().ToString());

            dataByKeyWords.Add("{immobilized_effect_time}", () => (ability as IAbleToApplyImmobilizedModificator).GetTimeToEndOfImmpobilizedEffect().ToString());

            dataByKeyWords.Add("{default_block_value}", () => (ability as IAbleToApplyBlock).GetDefaultBlockValue().ToString());
            dataByKeyWords.Add("{calculated_block_value}", () => IntToStringByCompareValues((ability as IAbleToApplyBlock).CalculateBlock(), (ability as IAbleToApplyBlock).GetDefaultBlockValue()));

            dataByKeyWords.Add("{default_armor_value}", () => (ability as IAbleToApplyArmor).GetDefaultArmorValue().ToString());
            dataByKeyWords.Add("{calculated_armor_value}", () => IntToStringByCompareValues((ability as IAbleToApplyArmor).CalculateArmor(), (ability as IAbleToApplyArmor).GetDefaultArmorValue()));

            dataByKeyWords.Add("{delta_value}", () => (ability as IAbleToHaveDelta).GetDeltaValue().ToString());
            dataByKeyWords.Add("{step_value}", () => (ability as IAbleToHaveDelta).GetStepValue().ToString());

            dataByKeyWords.Add("{alternative_delta_value}", () => (ability as IAbleToHaveAlternativeDelta).GetAlternativeDeltaValue().ToString());
            dataByKeyWords.Add("{alternative_step_value}", () => (ability as IAbleToHaveAlternativeDelta).GetAlternativeStepValue().ToString());

            dataByKeyWords.Add("{count}", () => (ability as IAbleToHaveCount).GetCount().ToString());

            dataByKeyWords.Add("{action_point_regeneration_penalty}", () => (ability as IAbleToApplyActionPointsRegenerationPenalty).GetActionPointsRegenerationPenaltyValue().ToString());
        }

        private string GetShortData()
        {
            return GenerateDescription(shortDataTags, true);
        }

        private string GetDescription()
        {
            return GenerateDescription(descriptionTags, false);
        }

        private string GenerateDescription(List<AbilityDescriptionTag> tags, bool thereAreNewLinesBetweenTags)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var tag in tags)
            {
                string tagText = tag.DescriptionText;
                bool ignoreTag = false;

                foreach (var dataByKeyWord in dataByKeyWords)
                {
                    if (tagText.Contains(dataByKeyWord.Key))
                    {
                        string textToReplaceTag = dataByKeyWord.Value.Invoke();

                        for(int i = 0; i < tag.BlackListTags.Count; i++)
                        {
                            if (tag.BlackListTags[i] == dataByKeyWord.Key && tag.BlackListValues[i] == textToReplaceTag)
                            {
                                ignoreTag = true;
                            }
                        }

                        tagText = tagText.Replace(dataByKeyWord.Key, textToReplaceTag);
                    }
                }

                if (ignoreTag)
                    continue;

                stringBuilder.Append(tagText);

                if (thereAreNewLinesBetweenTags)
                    stringBuilder.Append("\n");
                else 
                    stringBuilder.Append(" ");
            }

            return stringBuilder.ToString();
        }

        private string IntToStringByCompareValues(int comparableValue, int targetValueToCompare)
        {
            if(comparableValue == targetValueToCompare)
                return comparableValue.ToString();

            string color = comparableValue > targetValueToCompare ? "#96c620" : "#e05454";
            return $"<color={color}>{comparableValue}</color>";
        }
    }
}