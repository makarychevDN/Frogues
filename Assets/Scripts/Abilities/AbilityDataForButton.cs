using UnityEngine;

namespace FroguesFramework
{
    public class AbilityDataForButton : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField] private string name;
        [SerializeField, Multiline] private string stats;
        [SerializeField, Multiline] private string description;
        public Material Material => material;
        public string AbilityName => name;
        public string Stats => stats;
        public string Description => description;
    }
}