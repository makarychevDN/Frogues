using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActionPoints : MonoBehaviour
{
    [SerializeField] private IntContainer currentPoints;
    [SerializeField] private IntContainer maxPointsCount;
    [SerializeField] private IntContainer pointsRegeneration;
    [SerializeField] private AbleToSkipTurn skipTurnModule;

    [Header("for Player Only")]
    [SerializeField] private DisableAllSelectedCellsVizualisation seletedCellsVizualisationDisabler;

    public UnityEvent OnActionPointsEnded;

    private void Start()
    {
        OnActionPointsEnded.AddListener(skipTurnModule.AutoSkip);

        if (seletedCellsVizualisationDisabler != null)
        {
            OnActionPointsEnded.AddListener(seletedCellsVizualisationDisabler.ApplyEffect);
        }
    }

    public void RegeneratePoints()
    {
        currentPoints.Content += pointsRegeneration.Content;
        currentPoints.Content = Mathf.Clamp(currentPoints.Content, 0, maxPointsCount.Content);
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
