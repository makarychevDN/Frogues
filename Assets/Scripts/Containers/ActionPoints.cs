using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class ActionPoints : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private int currentPoints;
        [SerializeField] private int maxPointsCount;
        [SerializeField] private int pointsRegeneration;

        public UnityEvent OnActionPointsEnded;

        private void RegeneratePoints()
        {
            currentPoints += pointsRegeneration;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
        }
        
        public int CurrentActionPoints
        {
            get => currentPoints;
            set => currentPoints = value;
        }
        
        public int RegenActionPoints
        {
            get => pointsRegeneration;
        }

        public bool CheckIsActionPointsEnough(int cost)
        {
            return currentPoints >= cost;
        }

        public void SpendPoints(int cost)
        {
            currentPoints -= cost;

            if (currentPoints <= 0)
                OnActionPointsEnded.Invoke();
        }

        public bool Full => currentPoints >= maxPointsCount;
        
        public void Tick() => RegeneratePoints();
    }
}