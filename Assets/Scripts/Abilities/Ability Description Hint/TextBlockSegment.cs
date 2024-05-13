using TMPro;
using UnityEngine;

namespace FroguesFramework
{
    public class TextBlockSegment : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        public TMP_Text Label => label;
    }
}