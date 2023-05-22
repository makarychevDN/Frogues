using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilityResourcePoints : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization
    {
        [SerializeField] private int currentPoints;
        [SerializeField] private int maxPointsCount;
        [SerializeField] private int pointsRegeneration;
        private int _preTakenCurrentPoints;
        private bool _isEnemy;

        public UnityEvent OnActionPointsEnded;

        public void Init(Unit unit)
        {
            _isEnemy = unit.IsEnemy;
            AddMySelfToEntryPoint();
        }

        private void RegeneratePoints()
        {
            currentPoints += pointsRegeneration;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
        }

        #region GetSet

        public int MaxPointsCount
        {
            get => maxPointsCount;
        }
        
        public int CurrentActionPoints
        {
            get => currentPoints;
        }
        
        public int PreTakenCurrentPoints
        {
            get => _preTakenCurrentPoints;
        }
        
        #endregion
        
        public int RegenActionPoints
        {
            get => pointsRegeneration;
        }

        public bool IsPointsEnough(int cost)
        {
            return currentPoints >= cost;
        }

        public void SetPoints(int value)
        {
            currentPoints = value;
        }

        public void SpendPoints(int cost)
        {
            CalculateCost(ref currentPoints, cost);

            if (currentPoints <= 0)
                OnActionPointsEnded.Invoke();
        }
        
        public void PreSpendPoints(int preCost)
        {
            CalculateCost(ref _preTakenCurrentPoints, preCost);
        }

        private void CalculateCost(ref int points, int cost)
        {
            points -= cost;
        }

        public bool Full => currentPoints >= maxPointsCount;

        public void TickBeforePlayerTurn()
        { 
            if (_isEnemy)
                RegeneratePoints();
        }
        
        public void TickBeforeEnemiesTurn()
        {
            if (!_isEnemy)
                RegeneratePoints();
        }

        public void DisablePreVisualization()
        {
            _preTakenCurrentPoints = currentPoints;
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

    }
}