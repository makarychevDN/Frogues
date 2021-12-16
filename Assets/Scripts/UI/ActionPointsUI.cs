using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] private IntContainer currentActionPoints;
    [SerializeField] private IntContainer maxActionPoints;
    [SerializeField] private float iconsOffset;
    [SerializeField] private Transform iconsParent;
    [SerializeField] private ActionPointUI actionPointIconPrefab;
    [SerializeField] private List<ActionPointUI> actionPoinsIcons = new List<ActionPointUI>();
    [SerializeField] private List<IntContainer> preCosts;
    [SerializeField] private bool generateIconsOnStart;

    private void Start()
    {
        if (!generateIconsOnStart)
            return;

        if (iconsParent == null)
            iconsParent = transform;


        for (int i = 0; i < maxActionPoints.Content; i++)
        {
            var spawnedIcon = Instantiate(actionPointIconPrefab, iconsParent);
            actionPoinsIcons.Add(spawnedIcon);
            spawnedIcon.transform.localPosition = new Vector3((i - ((float)(maxActionPoints.Content - 1) / 2)) * iconsOffset, 0, 0);
        }
    }

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
