using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ResourcePointsUI : MonoBehaviour
    {
        [SerializeField] private ActionPoints actionPoints;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private List<ResourcePointUI> resourcePointIconPrefabs;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons = new();
        [SerializeField] private bool generateIconsOnStart;

        private void Start()
        {
            if (!generateIconsOnStart)
                return;

            if (iconsParent == null)
                iconsParent = transform;

            int iconsCount = 0;

            for (int i = 0; i < actionPoints.MaxPointsCount; i++)
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

            int preCostsValue = actionPoints.CurrentActionPoints - actionPoints.PreTakenCurrentPoints;
            
            if (preCostsValue > actionPoints.CurrentActionPoints)
            {
                for (int i = 0; i < actionPoints.CurrentActionPoints; i++)
                {
                    resourcePointIcons[i].EnableNotEnoughPointsIcon();
                }

                return;
            }

            for (int i = 0; i < actionPoints.CurrentActionPoints; i++)
            {
                resourcePointIcons[i].EnableFullIcon();
            }

            for (int i = actionPoints.CurrentActionPoints - 1; i > actionPoints.CurrentActionPoints - 1 - preCostsValue; i--)
            {
                resourcePointIcons[i].EnablePreCostIcon();
            }

        }
    }
}