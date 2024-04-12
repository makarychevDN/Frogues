using UnityEngine;
using UnityEngine.Localization;

namespace FroguesFramework
{
    public class SimpleHintEnabler : MonoBehaviour
    {
        [SerializeField] private LocalizedString header;
        [SerializeField] private LocalizedString description;
        [SerializeField] private Vector2 pivot;
        [SerializeField] private Vector2 offcet;


        public void ShowHint()
        {
            EntryPoint.Instance.AbilityHint.Init(header.GetLocalizedString(), description.GetLocalizedString(), "", transform, pivot, offcet);
            EntryPoint.Instance.AbilityHint.EnableContent(true, true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }
    }
}