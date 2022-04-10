using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace FroguesFramework
{
    [CreateAssetMenu(fileName = "Description", menuName = "ScriptableObjects/Description", order = 1)]
    public class UnitsDescription : ScriptableObject
    {
        public List<DescriptionTag> descriptionTags;

        [MultilineAttribute, SerializeField, ReadOnly]
        private string debugValue;

        private void OnValidate()
        {
            StringBuilder sb = new StringBuilder();
            descriptionTags.ForEach(tag => sb.Append(tag.Text).Append("\n "));
            debugValue = sb.ToString();
        }
    }
}