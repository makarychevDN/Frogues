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
        [SerializeField] private bool showHintUnderButton;        
        [SerializeField] private List<GameObject> contentObjects;

        public void Init(string name, string stats, string description, bool showHintUnderButton)
        {
            this.name.text = name;
            this.stats.text = stats;
            this.description.text = description;
            this.showHintUnderButton = showHintUnderButton;
        }

        public void EnableContent(bool value)
        {
            contentObjects.ForEach(contentObject => contentObject.SetActive(value));

            if (!showHintUnderButton)
                return;

            transform.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            transform.localPosition = new Vector3(0, -38, 0);
        }
    }
}