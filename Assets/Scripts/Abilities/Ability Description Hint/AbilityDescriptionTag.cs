using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Ability Description Tag", menuName = "ScriptableObjects/Ability Description Tag", order = 2)]
    public class AbilityDescriptionTag : ScriptableObject
    {
        [SerializeField] private string descriptionText;
        public string DescriptionText => descriptionText;
    }
}