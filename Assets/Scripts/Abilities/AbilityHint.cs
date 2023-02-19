using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class AbilityHint : MonoBehaviour
    {
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text stats;
        [SerializeField] private TMP_Text description;
        
        [SerializeField] private List<GameObject> contentObjects;

        public void Init(string name, string stats, string description)
        {
            this.name.text = name;
            this.stats.text = stats;
            this.description.text = description;
        }

        public void EnableContent(bool value)
        {
            contentObjects.ForEach(contentObject => contentObject.SetActive(value));
        }
    }
}