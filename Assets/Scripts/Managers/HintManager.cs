using UnityEngine;

namespace FroguesFramework
{
    public class HintManager : MonoBehaviour
    {
        public static HintManager Instance;
        [SerializeField] private Hint abilityHint;
        [SerializeField] private Hint commonSmallHint;

        public Hint AbilityHint => abilityHint;
        public Hint CommonSmallHint => commonSmallHint;

        private void Awake()
        {
            Instance = this;
        }
    }
}