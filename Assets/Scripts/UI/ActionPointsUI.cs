using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] private IntContainer currentActionPoints;
    [SerializeField] private List<ActionPointUI> actionPoinsIcons;
    [SerializeField] private List<IntContainer> preCosts;

    private void Update()
    {
        actionPoinsIcons.ForEach(icon => icon.EnableEmptyIcon());

        int preCostsSum = preCosts.Sum(preCost => preCost.Content);

        if (preCostsSum > currentActionPoints.Content)
        {
            for (int i = 0; i < currentActionPoints.Content; i++)
            {
                actionPoinsIcons[i].EnableNotEnoghPointsIcon();
            }

            return;
        }

        for (int i = 0; i < currentActionPoints.Content; i++)
        {
            actionPoinsIcons[i].EnableFullIcon();
        }

        for (int i = currentActionPoints.Content - 1; i > currentActionPoints.Content - 1 - preCostsSum; i--)
        {
            actionPoinsIcons[i].EnablePreCostIcon();
        }
    }
}
