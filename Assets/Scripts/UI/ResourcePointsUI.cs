using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ResourcePointsUI : MonoBehaviour
    {
        [SerializeField] private AbilityResourcePoints resourcePoints;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private List<ResourcePointUI> resourcePointIconPrefabs;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons = new();
        [SerializeField] private bool generateIconsOnStart;
        private int hashedResourcePointsCount;
        private int hashedPrevisualization;

        private void Start()
        {
            hashedResourcePointsCount = resourcePoints.CurrentPoints;

            if (!generateIconsOnStart)
                return;

            if (iconsParent == null)
                iconsParent = transform;

            int iconsCount = 0;

            for (int i = 0; i < resourcePoints.MaxPointsCount; i++)
            {
                var spawnedIcon = Instantiate(resourcePointIconPrefabs[iconsCount], iconsParent);
                resourcePointIcons.Add(spawnedIcon);

                iconsCount++;
                iconsCount = (int) Mathf.Repeat(iconsCount, resourcePointIconPrefabs.Count);
            }

            resourcePoints.OnPointsRegenerated.AddListener(RedrawIcon);
        }

        private void Update()
        {
            /*resourcePointIcons.ForEach(icon => icon.EnableEmptyIcon());

            int preCostsValue = ResourcePoints.CurrentActionPoints - ResourcePoints.PreTakenCurrentPoints;
            
            if (preCostsValue > ResourcePoints.CurrentActionPoints)
            {
                for (int i = 0; i < ResourcePoints.CurrentActionPoints; i++)
                {
                    resourcePointIcons[i].EnableNotEnoughPointsIcon();
                }

                return;
            }

            for (int i = 0; i < ResourcePoints.CurrentActionPoints; i++)
            {
                resourcePointIcons[i].EnableFullIcon();
            }

            for (int i = ResourcePoints.CurrentActionPoints - 1; i > ResourcePoints.CurrentActionPoints - 1 - preCostsValue; i--)
            {
                resourcePointIcons[i].EnablePreCostIcon();
            }*/

            if(hashedPrevisualization != resourcePoints.CalculateHashFunctionOfPrevisualisation())
            {
                RedrawIcon();
            }

            hashedPrevisualization = resourcePoints.CalculateHashFunctionOfPrevisualisation();
        }

        private void RedrawIcon()
        {
            for (int i = 0; i < resourcePoints.MaxPointsCount; i++)
            {
                resourcePointIcons[i].DisablePreCostIcon();
                resourcePointIcons[i].EnableEmptyIcon();

                if (i < resourcePoints.CurrentPoints)
                    resourcePointIcons[i].EnableFullIcon();
            }

            if (hashedResourcePointsCount < resourcePoints.CurrentPoints)
            {
                for (int i = hashedResourcePointsCount; i < resourcePoints.CurrentPoints; i++)
                {
                    resourcePointIcons[i].Regen();
                }
            }

            for (int i = resourcePoints.PreTakenCurrentPoints; i < resourcePoints.CurrentPoints; i++)
            {
                resourcePointIcons[i].EnablePreCostIcon();
            }

            hashedResourcePointsCount = resourcePoints.CurrentPoints;
        }
    }
}