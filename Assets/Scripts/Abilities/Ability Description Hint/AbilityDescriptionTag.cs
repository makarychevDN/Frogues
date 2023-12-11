using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Ability Description Tag", menuName = "ScriptableObjects/Ability Description Tag", order = 2)]
    public class AbilityDescriptionTag : ScriptableObject
    {
        [SerializeField] private string descriptionText;

        [Header("Black List Setup")]
        [SerializeField] private List<string> blackListTags;
        [SerializeField] private List<string> blackListValues;

        public string DescriptionText => descriptionText;
        public List<string> BlackListTags => blackListTags;
        public List<string> BlackListValues => blackListValues;
    }
}