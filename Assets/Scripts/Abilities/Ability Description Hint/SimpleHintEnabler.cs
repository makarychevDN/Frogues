using UnityEngine;
using UnityEngine.Localization;

namespace FroguesFramework
{
    public class SimpleHintEnabler : MonoBehaviour
    {
        [SerializeField] private LocalizedString header;
        [SerializeField] private LocalizedString description;
        [SerializeField] private Vector2 pivot;
        [SerializeField] private Vector2 offset;
        [SerializeField] private bool useSmallHint;
        private Hint _hashedHint;


        public void ShowHint()
        {
            _hashedHint = useSmallHint ? EntryPoint.Instance.CommonSmallHint : EntryPoint.Instance.AbilityHint;

            _hashedHint.Init(header.GetLocalizedString(), description.GetLocalizedString(), transform, pivot, offset);
            _hashedHint.EnableContent(true);
        }

        public void HideHint()
        {
            _hashedHint.EnableContent(false);
        }
    }
}