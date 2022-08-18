using UnityEngine;
using UnityEngine.Events;

namespace FroguesFramework
{
    public class ActionPoints : MonoBehaviour
    {
        [SerializeField] private IntContainer currentPoints;
        [SerializeField] private IntContainer maxPointsCount;
        [SerializeField] private IntContainer pointsRegeneration;
        [SerializeField] private AbleToSkipTurn skipTurnModule;

        [Header("for Player Only")] [SerializeField]
        private DisableAllSelectedCellsVizualisation selectedCellsVisualizationDisabler;

        public UnityEvent OnActionPointsEnded;

        private void Start()
        {
            if (skipTurnModule != null)
            {
                OnActionPointsEnded.AddListener(skipTurnModule.AutoSkip);
            }

            if (selectedCellsVisualizationDisabler != null)
            {
                OnActionPointsEnded.AddListener(selectedCellsVisualizationDisabler.ApplyEffect);
            }
        }

        public void RegeneratePoints()
        {
            currentPoints.Content += pointsRegeneration.Content;
            currentPoints.Content = Mathf.Clamp(currentPoints.Content, 0, maxPointsCount.Content);
        }
        
        public int CurrentActionPoints
        {
            get => currentPoints.Content;
            set => currentPoints.Content = value;
        }
        
        public int RegenActionPoints
        {
            get => pointsRegeneration.Content;
        }

        public bool CheckIsActionPointsEnough(int cost)
        {
            return currentPoints.Content >= cost;
        }

        public void SpendPoints(int cost)
        {
            currentPoints.Content -= cost;

            if (currentPoints.Content <= 0)
                OnActionPointsEnded.Invoke();
        }

        public bool Full => currentPoints.Content >= maxPointsCount.Content;
    }
}