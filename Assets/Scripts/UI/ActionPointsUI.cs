using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ActionPointsUI : MonoBehaviour
    {
        [SerializeField] private IntContainer currentActionPoints;
        [SerializeField] private IntContainer maxActionPoints;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private List<ActionPointUI> actionPointIconPrefabs;
        [SerializeField] private List<ActionPointUI> actionPointIcons = new List<ActionPointUI>();
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
                actionPointIcons.Add(spawnedIcon);

                iconsCount++;
                iconsCount = (int) Mathf.Repeat(iconsCount, actionPointIconPrefabs.Count);
            }

            float startXPosition = 0;
            actionPointIcons.ForEach(icon => startXPosition -= icon.RadiusOfIcons);
            startXPosition += actionPointIcons[0].RadiusOfIcons;

            for (int i = 0; i < maxActionPoints.Content - 1; i++)
            {
                actionPointIcons[i].transform.localPosition = new Vector3(startXPosition, 0, 0);
                startXPosition += actionPointIcons[i].RadiusOfIcons + actionPointIcons[i + 1].RadiusOfIcons;
            }

            actionPointIcons[actionPointIcons.Count - 1].transform.localPosition = new Vector3(startXPosition, 0, 0);


        }

        private void Update()
        {
            actionPointIcons.ForEach(icon => icon.EnableEmptyIcon());

            int preCostsSum = preCosts.Sum(preCost => preCost.Content);

            if (preCostsSum > currentActionPoints.Content)
            {
                for (int i = 0; i < currentActionPoints.Content; i++)
                {
                    actionPointIcons[i].EnableNotEnoghPointsIcon();
                }

                return;
            }

            for (int i = 0; i < currentActionPoints.Content; i++)
            {
                actionPointIcons[i].EnableFullIcon();
            }

            for (int i = currentActionPoints.Content - 1; i > currentActionPoints.Content - 1 - preCostsSum; i--)
            {
                actionPointIcons[i].EnablePreCostIcon();
            }
        }
    }
}