using UnityEngine;

namespace FroguesFramework
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] private int strenght;
        [SerializeField] private int intelegence;
        [SerializeField] private int dexterity;
        [SerializeField] private int defence;
        [SerializeField] private int spikes;
        [SerializeField] private float strengtModificatorStep;
        [SerializeField] private float intelegenceModificatorStep;
        [SerializeField] private float dexterityModificatorStep;
        [SerializeField] private float defenceModificatorStep;

        public int Strenght => strenght;
        public int Intelegence => intelegence;
        public int Dexterity => dexterity;
        public int Defence => defence;
        public int Spikes => spikes;

        public float StrenghtModificator => (1 + strenght * strengtModificatorStep);
        public float IntelegenceModificator => (1 + intelegence * intelegenceModificatorStep);
        public float DexterityeModificator => (1 + dexterity * dexterityModificatorStep);
        public float DefenceModificator => (1 + defence * defenceModificatorStep);
    }
}