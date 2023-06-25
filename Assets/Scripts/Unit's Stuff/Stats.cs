using UnityEngine;

namespace FroguesFramework
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] private int strenght;
        [SerializeField] private int intelegence;
        [SerializeField] private float strengtModificatorStep;
        [SerializeField] private float intelegenceModificatorStep;

        public int Strenght => strenght;
        public int Intelegence => intelegence;

        public float StrenghtModificator => (1 + strenght * strengtModificatorStep);
        public float IntelegenceModificator => (1 + intelegence * intelegenceModificatorStep);
    }
}