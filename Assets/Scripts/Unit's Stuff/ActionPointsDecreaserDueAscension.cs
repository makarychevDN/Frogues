using UnityEngine;

namespace FroguesFramework
{
    public class ActionPointsDecreaserDueAscension : MonoBehaviour
    {
        [SerializeField] private AbilityResourcePoints actionPoints;
        [SerializeField] private int maxPointsValue;
        [SerializeField] private int regenerationValue;
        [SerializeField] private int expectedAscensionIndexToStopDecreaseActionPoints;

        public void Start ()
        {
            if(EntryPoint.Instance.AscensionSetup.index < expectedAscensionIndexToStopDecreaseActionPoints)
            {
                actionPoints.IncreaseMaxPoints(maxPointsValue);
                actionPoints.IncreaseoRegeneration(regenerationValue);
            }
        }
    }
}