using UnityEngine;

namespace FroguesFramework
{
    public class AbilityDataForButton : MonoBehaviour
    {
        [SerializeField] private Sprite sprite;
        public Sprite Sprite => sprite;
    }
}