using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ResourcePointsUI : MonoBehaviour
    {
        [SerializeField] private IntContainer currentResourcePointsContainer;
        [SerializeField] private IntContainer maxActionPointsContainer;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private List<ResourcePointUI> resourcePointIconPrefabs;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons = new();
        [SerializeField] private List<IntContainer> preCosts;
        [SerializeField] private bool generateIconsOnStart;

        private void Start()
        {
            if (!generateIconsOnStart)
                return;

            if (iconsParent == null)
                iconsParent = transform;

            int iconsCount = 0;

            for (int i = 0; i < maxActionPointsContainer.Content; i++)
            {
                var spawnedIcon = Instantiate(resourcePointIconPrefabs[iconsCount], iconsParent);
                resourcePointIcons.Add(spawnedIcon);

                iconsCount++;
                iconsCount = (int) Mathf.Repeat(iconsCount, resourcePointIconPrefabs.Count);
            }
        }

        private void Update()
        {
            resourcePointIcons.ForEach(icon => icon.EnableEmptyIcon());

            int preCostsSum = preCosts.Sum(preCost => preCost.Content);

            if (preCostsSum > currentResourcePointsContainer.Content)
            {
                for (int i = 0; i < currentResourcePointsContainer.Content; i++)
                {
                    resourcePointIcons[i].EnableNotEnoughPointsIcon();
                }

                return;
            }

            for (int i = 0; i < currentResourcePointsContainer.Content; i++)
            {
                resourcePointIcons[i].EnableFullIcon();
            }

            for (int i = currentResourcePointsContainer.Content - 1; i > currentResourcePointsContainer.Content - 1 - preCostsSum; i--)
            {
                resourcePointIcons[i].EnablePreCostIcon();
            }
        }
    }
}