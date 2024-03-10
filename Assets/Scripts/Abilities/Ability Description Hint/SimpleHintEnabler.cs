using UnityEngine;

namespace FroguesFramework
{
    public class SimpleHintEnabler : MonoBehaviour
    {
        [SerializeField] private string header;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Vector2 pivot;
        [SerializeField] private Vector2 offcet;


        public void ShowHint()
        {
            EntryPoint.Instance.AbilityHint.Init(header, description, "", transform, pivot, offcet);
            EntryPoint.Instance.AbilityHint.EnableContent(true, true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}