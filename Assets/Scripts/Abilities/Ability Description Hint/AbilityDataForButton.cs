using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Localization;

namespace FroguesFramework
{
    [RequireComponent(typeof(BaseAbility))]
    public class AbilityDataForButton : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private LocalizedString abilityName;
        [SerializeField] private List<AbilityDescriptionTag> shortDataTags;
        [SerializeField] private List<AbilityDescriptionTag> descriptionTags;
        [SerializeField] private SerializedDictionary<DamageType, LocalizedString> localizedDamageTypes;
        private BaseAbility ability;
        private Dictionary<string, Func<string>> _dataByKeyWords = new Dictionary<string, Func<string>>();

        public Material Material => material;
        public string AbilityName => abilityName.GetLocalizedString();
        public string ShortData => GetShortData();
        public string Description => GetDescription();

        private void Awake()
        {
            ability = GetComponent<BaseAbility>();

            _dataByKeyWords.Add("{value}", () => (ability as IAbleToReturnSingleValue).GetValue().ToString());
            _dataByKeyWords.Add("{value_2}", () => (ability as IAbleToReturnSecondSingleValue).GetSecondValue().ToString());

            _dataByKeyWords.Add("{max_blood_points_mod}", () => (ability as IAbleToModifyMaxBloodPoints).GetModificatorForMaxBloodPoints().ToString());
            _dataByKeyWords.Add("{max_hp_mod}", () => (ability as IAbleToModifyMaxHP).GetModificatorForMaxHP().ToString());

            _dataByKeyWords.Add("{range}", () => (ability as IAbleToReturnRange).ReturnRange().ToString());
            _dataByKeyWords.Add("{alternative_range}", () => (ability as IAbleToHaveAlternativeRange).GetAlternativeRange().ToString());

            _dataByKeyWords.Add("{cooldown_after_use}", () => (ability as IAbleToHaveCooldown).GetCooldownAfterUse().ToString());
            _dataByKeyWords.Add("{cooldown_after_start}", () => (ability as IAbleToHaveCooldown).GetCooldownAfterStart().ToString());
            _dataByKeyWords.Add("{max_charges}", () => (ability as IAbleToHaveCooldown).GetMaxCharges().ToString());

            _dataByKeyWords.Add("{action_points_cost}", () => (ability as IAbleToCost).GetActionPointsCost().ToString());
            _dataByKeyWords.Add("{blood_points_cost}", () => (ability as IAbleToCost).GetBloodPointsCost().ToString());
            _dataByKeyWords.Add("{health_points_cost}", () => (ability as IAbleToCost).GetHealthCost().ToString());

            _dataByKeyWords.Add("{default_damage_value}", () => (ability as IAbleToDealDamage).GetDefaultDamage().ToString());
            _dataByKeyWords.Add("{calculated_damage_value}", () => IntToStringByCompareValues((ability as IAbleToDealDamage).CalculateDamage(), (ability as IAbleToDealDamage).GetDefaultDamage()));
            _dataByKeyWords.Add("{damage_type}", () => LocalizeDamageType((ability as IAbleToDealDamage).GetDamageType()).GetLocalizedString());

            _dataByKeyWords.Add("{alternative_default_damage_value}", () => (ability as IAbleToDealAlternativeDamage).GetDefaultAlternativeDamage().ToString());
            _dataByKeyWords.Add("{calculated_alternative_damage_value}", () => IntToStringByCompareValues((ability as IAbleToDealAlternativeDamage).CalculateAlternativeDamage(), (ability as IAbleToDealAlternativeDamage).GetDefaultAlternativeDamage()));
            _dataByKeyWords.Add("{alternative_damage_type}", () => (ability as IAbleToDealAlternativeDamage).GetAlternativeDamageType().ToString());

            _dataByKeyWords.Add("{current_default_damage_value}", () => (ability as IAbleToReturnCurrentDamage).GetDefaultCurrentDamage().ToString());
            _dataByKeyWords.Add("{calculated_current_damage_value}", () => IntToStringByCompareValues((ability as IAbleToReturnCurrentDamage).GetCalculatedCurrentDamage(), (ability as IAbleToReturnCurrentDamage).GetDefaultCurrentDamage()));

            _dataByKeyWords.Add("{effect_value}", () => (ability as IAbleToApplyAnyModificator).GetModificatorValue().ToString());
            _dataByKeyWords.Add("{effect_delta}", () => (ability as IAbleToApplyAnyModificator).GetDeltaValueForEachTurn().ToString());
            _dataByKeyWords.Add("{effect_time}", () => (ability as IAbleToApplyAnyModificator).GetTimeToEndOfEffect().ToString());
            _dataByKeyWords.Add("{effect_constantly}", () => (ability as IAbleToApplyAnyModificator).GetEffectIsConstantly().ToString());

            _dataByKeyWords.Add("{thorns_effect_value}", () => (ability as IAbleToApplySpikesModificator).GetSpikesModificatorValue().ToString());
            _dataByKeyWords.Add("{thorns_effect_delta}", () => (ability as IAbleToApplySpikesModificator).GetdeltaOfSpikesValueForEachTurn().ToString());
            _dataByKeyWords.Add("{thorns_effect_time}", () => (ability as IAbleToApplySpikesModificator).GetTimeToEndOfSpikesEffect().ToString());
            _dataByKeyWords.Add("{thorns_effect_constantly}", () => (ability as IAbleToApplySpikesModificator).GetSpikesEffectIsConstantly().ToString());

            _dataByKeyWords.Add("{immobilized_effect_time}", () => (ability as IAbleToApplyImmobilizedModificator).GetTimeToEndOfImmpobilizedEffect().ToString());

            _dataByKeyWords.Add("{default_block_value}", () => (ability as IAbleToApplyBlock).GetDefaultBlockValue().ToString());
            _dataByKeyWords.Add("{calculated_block_value}", () => IntToStringByCompareValues((ability as IAbleToApplyBlock).CalculateBlock(), (ability as IAbleToApplyBlock).GetDefaultBlockValue()));

            _dataByKeyWords.Add("{default_armor_value}", () => (ability as IAbleToApplyArmor).GetDefaultArmorValue().ToString());
            _dataByKeyWords.Add("{calculated_armor_value}", () => IntToStringByCompareValues((ability as IAbleToApplyArmor).CalculateArmor(), (ability as IAbleToApplyArmor).GetDefaultArmorValue()));

            _dataByKeyWords.Add("{delta_value}", () => (ability as IAbleToHaveDelta).GetDeltaValue().ToString());
            _dataByKeyWords.Add("{step_value}", () => (ability as IAbleToHaveDelta).GetStepValue().ToString());

            _dataByKeyWords.Add("{alternative_delta_value}", () => (ability as IAbleToHaveAlternativeDelta).GetAlternativeDeltaValue().ToString());
            _dataByKeyWords.Add("{alternative_step_value}", () => (ability as IAbleToHaveAlternativeDelta).GetAlternativeStepValue().ToString());

            _dataByKeyWords.Add("{count}", () => (ability as IAbleToHaveCount).GetCount().ToString());

            _dataByKeyWords.Add("{action_point_regeneration_penalty}", () => (ability as IAbleToApplyActionPointsRegenerationPenalty).GetActionPointsRegenerationPenaltyValue().ToString());
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
            return Extensions.GenerateDescription(tags, _dataByKeyWords, thereAreNewLinesBetweenTags);
        }

        private string IntToStringByCompareValues(int comparableValue, int targetValueToCompare)
        {
            if(comparableValue == targetValueToCompare)
                return comparableValue.ToString();

            string color = comparableValue > targetValueToCompare ? "#96c620" : "#e05454";
            return $"<color={color}>{comparableValue}</color>";
        }

        public LocalizedString LocalizeDamageType(DamageType damageType)
        {
            return localizedDamageTypes[damageType];
        }
    }
}