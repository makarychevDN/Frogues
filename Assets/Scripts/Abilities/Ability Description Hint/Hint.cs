using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class Hint : MonoBehaviour
    {
        [SerializeField] private TMP_Text headerLabel;
        [SerializeField] private List<TextBlockSegment> textBlockSegments;

        public void Init(string header, string textBlock, Transform hintedObject, Vector2 pivot, Vector2 positionRelativeToHintedObject)
        {
            Init(header, new List<string> { textBlock }, hintedObject, pivot, positionRelativeToHintedObject);
        }

        public virtual void Init(string header, List<string> textBlocks, Transform button, Vector2 pivot, Vector2 positionRelativeToButton)
        {
            Init(header, textBlocks);
            (transform as RectTransform).pivot = pivot;
            transform.position = button.position.ToVector2() + positionRelativeToButton;
        }

        public void Init(string header, List<string> textBlocks)
        {
            headerLabel.text = header;

            if (textBlocks.Count > textBlockSegments.Count)
            {
                Debug.LogError("need more text block sements!");
                return;
            }

            for (int i = 0; i < textBlockSegments.Count; i++)
            {
                textBlockSegments[i].gameObject.SetActive(i < textBlocks.Count);

                if (i >= textBlocks.Count)
                    continue;

                textBlockSegments[i].Label.text = textBlocks[i];
            }
        }

        public virtual void EnableContent(bool value)
        {
            gameObject.SetActive(value);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }
    }
}