using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class AbilityResourcePoints : MonoBehaviour, IRoundTickable, IAbleToDisablePreVisualization, IAbleToCalculateHashFunctionOfPrevisualisation
    {
        [SerializeField] private int currentPoints;
        [SerializeField] private int tempraryPoints;
        [SerializeField] private int maxPointsCount;
        [SerializeField] private int pointsRegeneration;
        [SerializeField] private int penaltyForRegeneration;
        private int _preTakenCurrentPoints;
        private int _preTakenTemporaryPoints;

        public UnityEvent OnAnyPointsIncreased;
        public UnityEvent OnDefaultPointsIncreased;
        public UnityEvent OnTemporaryPointsIncreased;
        public UnityEvent OnPointsRegenerated;
        public UnityEvent OnPickUpPoints;
        public UnityEvent OnPointsSpended;
        public UnityEvent OnPointsEnded;
        public UnityEvent<int> OnTemporaryPointsReseted;

        public void Init(Unit unit)
        {
            AddMySelfToEntryPoint();
            unit.AbleToSkipTurn.OnSkipTurn.AddListener(RegeneratePoints);
        }

        private void RegeneratePoints()
        {
            var hashedPoints = currentPoints;
            currentPoints += pointsRegeneration - penaltyForRegeneration;
            penaltyForRegeneration = 0;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
            _preTakenCurrentPoints = currentPoints;
            OnTemporaryPointsReseted.Invoke(tempraryPoints);
            tempraryPoints = 0;
            _preTakenTemporaryPoints = tempraryPoints;
            OnPointsRegenerated.Invoke();

            if (hashedPoints < currentPoints)
            {
                OnAnyPointsIncreased.Invoke();
                OnDefaultPointsIncreased.Invoke();
            }
        }

        #region GetSet

        public int MaxPointsCount
        {
            get => maxPointsCount;
        }

        public int CurrentPoints
        {
            get => currentPoints;
        }

        public int AvailablePoints
        {
            get => currentPoints + tempraryPoints;
        }

        public int TemporaryPoints
        {
            get => tempraryPoints;
        }

        public int PreTakenCurrentPoints
        {
            get => _preTakenCurrentPoints;
        }

        public int PreTakenTemporaryPoints
        {
            get => _preTakenTemporaryPoints;
        }

        #endregion

        public int PointsRegeneration
        {
            get => pointsRegeneration;
        }

        public void IncreasePenaltyToRegeneration(int value)
        {
            penaltyForRegeneration += value;
        }

        public bool IsPointsEnough(int cost)
        {
            return currentPoints + tempraryPoints >= cost;
        }

        public void SetCurrentPoints(int value)
        {
            currentPoints = value;
        }

        public void SetTemporaryPoints(int value)
        {
            currentPoints = value;
        }

        public void IncreasePoints(int value)
        {
            var hashedPoints = currentPoints;
            currentPoints += value;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
            _preTakenCurrentPoints = currentPoints;

            if(hashedPoints < currentPoints)
            {
                OnAnyPointsIncreased.Invoke();
                OnDefaultPointsIncreased.Invoke();
            }
        }

        public void IncreaseMaxPoints(int value)
        {
            maxPointsCount += value;
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPointsCount);
        }

        public void PickupPoints(int value)
        {
            OnPickUpPoints.Invoke();
            IncreasePoints(value);
        }

        public void PickupTemporaryPoints(int value)
        {
            OnPickUpPoints.Invoke();
            IncreaseTemporaryPoints(value);
        }

        public void IncreaseTemporaryPoints(int value)
        {
            tempraryPoints += value;
            _preTakenTemporaryPoints = tempraryPoints;
            OnAnyPointsIncreased.Invoke();
            OnTemporaryPointsIncreased.Invoke();
        }

        public void SpendPoints(int cost)
        {
            CalculateCost(ref currentPoints, ref tempraryPoints, cost);
            OnPointsSpended.Invoke();

            if (currentPoints <= 0)
                OnPointsEnded.Invoke();
        }
        
        public void PreSpendPoints(int preCost)
        {
            CalculateCost(ref _preTakenCurrentPoints, ref _preTakenTemporaryPoints, preCost);
        }

        private void CalculateCost(ref int points, ref int temporarypPoints, int cost)
        {
            int spendedTemporaryCost = Mathf.Clamp(cost, 0, temporarypPoints);
            cost -= spendedTemporaryCost;
            temporarypPoints -= spendedTemporaryCost;
            points -= cost;
        }

        public bool Full => currentPoints >= maxPointsCount;

        public void TickAfterEnemiesTurn()
        { 
            //if (_isEnemy)
                //RegeneratePoints();
        }
        
        public void TickAfterPlayerTurn()
        {
            //if (!_isEnemy)
               // RegeneratePoints();
        }

        public void DisablePreVisualization()
        {
            _preTakenCurrentPoints = currentPoints;
            _preTakenTemporaryPoints = tempraryPoints;
        }

        public void AddMySelfToEntryPoint() =>
            EntryPoint.Instance.AddAbleToDisablePreVisualizationToCollection(this);

        public void RemoveMySelfFromEntryPoint() =>
            EntryPoint.Instance.RemoveAbleToDisablePreVisualizationToCollection(this);

        private void OnDestroy()
        {
            RemoveMySelfFromEntryPoint();
        }

        public int CalculateHashFunctionOfPrevisualisation()
        {
            return maxPointsCount * 1 + currentPoints * 10 + tempraryPoints * 100 + _preTakenCurrentPoints * 1000 + _preTakenTemporaryPoints * 10000;
        }
    }
}