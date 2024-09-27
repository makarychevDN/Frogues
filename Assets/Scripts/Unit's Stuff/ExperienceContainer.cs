using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class ExperienceContainer : MonoBehaviour
    {
        [SerializeField] private int ownersLevel;
        [SerializeField] private int currentExperience;
        [SerializeField] private int expirienceRequiredToLvlUp;
        [SerializeField] private int skillPoints;
        [SerializeField] private int additionalSkillPointsPerLevel;

        public UnityEvent OnLevelUp;

        public void Init(Unit owner)
        {
            skillPoints = ownersLevel * additionalSkillPointsPerLevel;
        }

        public void AddExpirience(int experience)
        {
            currentExperience += experience;
        }
    }
}