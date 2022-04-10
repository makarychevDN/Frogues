using UnityEngine;

namespace FroguesFramework
{
    public abstract class CostsActionPointsBehaviour : MonoBehaviour
    {
        [SerializeField] protected ActionPoints actionPoints;
        [SerializeField] protected IntContainer defaultActionPointsCost;

        public int CurrentActionCost => defaultActionPointsCost.Content;

        public bool IsActionPointsEnough()
        {
            return IsActionPointsEnough(defaultActionPointsCost.Content);
        }

        public bool IsActionPointsEnough(int actionPointsCost)
        {
            return actionPoints.CheckIsActionPointsEnough(actionPointsCost);
        }

        public void SpendActionPoints()
        {
            actionPoints.SpendPoints(defaultActionPointsCost.Content);
        }

        public void SpendActionPoints(int actionPointsCost)
        {
            actionPoints.SpendPoints(actionPointsCost);
        }
    }
}