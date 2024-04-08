using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Ability Description Tag", menuName = "ScriptableObjects/Ability Description Tag", order = 2)]
    public class AbilityDescriptionTag : ScriptableObject
    {
        [SerializeField, TextArea] private string descriptionText;
        [SerializeField] private LocalizedString test;

        [Header("Black List Setup")]
        [SerializeField] private List<string> blackListTags;
        [SerializeField] private List<string> blackListValues;

        public string DescriptionText => descriptionText;
        //public string DescriptionText => test.GetLocalizedString();
        public List<string> BlackListTags => blackListTags;
        public List<string> BlackListValues => blackListValues;
    }
}