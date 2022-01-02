using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionPointsUI : MonoBehaviour
{
    [SerializeField] private IntContainer currentActionPoints;
    [SerializeField] private IntContainer maxActionPoints;
    [SerializeField] private Transform iconsParent;
    [SerializeField] private List<ActionPointUI> actionPointIconPrefabs;
    [SerializeField] private List<ActionPointUI> actionPoinsIcons = new List<ActionPointUI>();
    [SerializeField] private List<IntContainer> preCosts;
    [SerializeField] private bool generateIconsOnStart;

    private void Start()
    {
        if (!generateIconsOnStart)
            return;

        if (iconsParent == null)
            iconsParent = transform;

        int iconsCount = 0;

        for (int i = 0; i < maxActionPoints.Content; i++)
        {
            var spawnedIcon = Instantiate(actionPointIconPrefabs[iconsCount], iconsParent);
            actionPoinsIcons.Add(spawnedIcon);

            iconsCount++;
            iconsCount = (int)Mathf.Repeat(iconsCount, actionPointIconPrefabs.Count);
        }

        float startXPositon = 0;
        actionPoinsIcons.ForEach(icon => startXPositon -= icon.RadiusOfIcons);
        startXPositon += actionPoinsIcons[0].RadiusOfIcons;

        for (int i = 0; i < maxActionPoints.Content -1; i++)
        {
            actionPoinsIcons[i].transform.localPosition = new Vector3(startXPositon, 0, 0);
            startXPositon += actionPoinsIcons[i].RadiusOfIcons + actionPoinsIcons[i + 1].RadiusOfIcons;
        }

        actionPoinsIcons[actionPoinsIcons.Count - 1].transform.localPosition = new Vector3(startXPositon, 0, 0);


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