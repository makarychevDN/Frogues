using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Description", menuName = "ScriptableObjects/Description Tag", order = 2)]
    public class DescriptionTag : ScriptableObject
    {
        [SerializeField] private string text;
        public string Text => text;
        [SerializeField] private DescriptionTag descriptionTag;
        public bool hasDescription => descriptionTag != null;
        public Color textColor = Color.white;
    }
}