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

                foreach (var dataByKeyWord in dataByKeyWords)
                {
                    if (tagText.Contains(dataByKeyWord.Key))
                    {
                        tagText = tagText.Replace(dataByKeyWord.Key, dataByKeyWord.Value.Invoke());
                    }
                }

                stringBuilder.AppendLine(tagText);

                if (thereAreNewLinesBetweenTags)
                    stringBuilder.AppendLine("\n");
            }

            return stringBuilder.ToString();
        }
    }
}