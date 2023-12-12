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
        [SerializeField, Multiline] private string stats;
        [SerializeField, Multiline] private string description;
        [SerializeField] private List<AbilityDescriptionTag> statsTags;
        private BaseAbility ability;
        private Dictionary<string, Func<string>> dataByKeyWords = new Dictionary<string, Func<string>>();

        public Material Material => material;
        public string AbilityName => abilityName;
        public string Stats => GetStats();
        public string Description => description;

        private void Awake()
        {
            ability = GetComponent<BaseAbility>();

            dataByKeyWords.Add("{range}", () => (ability as IAbleToReturnRange).ReturnRange().ToString());

            dataByKeyWords.Add("{cooldown after use}", () => (ability as IAbleToHaveCooldown).GetCooldownAfterUse().ToString());
            dataByKeyWords.Add("{cooldown after start}", () => (ability as IAbleToHaveCooldown).GetCooldownAfterStart().ToString());

            dataByKeyWords.Add("{action points cost}", () => (ability as IAbleToCost).GetActionPointsCost().ToString());
            dataByKeyWords.Add("{blood points cost}", () => (ability as IAbleToCost).GetBloodPointsCost().ToString());
            dataByKeyWords.Add("{health points cost}", () => (ability as IAbleToCost).GetHealthCost().ToString());

            dataByKeyWords.Add("{default damage value}", () => (ability as IAbleToDealDamage).GetDefaultDamage().ToString());
            dataByKeyWords.Add("{calculated damage value}", () => (ability as IAbleToDealDamage).CalculateDamage().ToString());
            dataByKeyWords.Add("{damage type}", () => (ability as IAbleToDealDamage).GetDamageType().ToString());
        }

        private string GetStats()
        {
            if(statsTags.Count > 0)
            {
                return GenerateDescription(true);
            }

            return stats;
        }

        private string GenerateDescription(bool thereAreNewLinesBetweenTags)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var tag in statsTags)
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
            }

            return stringBuilder.ToString();
        }
    }
}