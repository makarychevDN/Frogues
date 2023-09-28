using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class ResourcePointsUI : MonoBehaviour
    {
        [SerializeField] private AbilityResourcePoints resourcePoints;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private ResourcePointUI resourcePointIconPrefab;
        [SerializeField] private ResourcePointUI temporaryResourcePointIconPrefab;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons = new();
        [SerializeField] private List<ResourcePointUI> temporaryResourcePointIcons = new();
        [SerializeField] private bool generateIconsOnStart;
        private int hashedResourcePointsCount;
        private int hashedPrevisualization;

        private void Start()
        {
            resourcePoints.OnPointsRegenerated.AddListener(() => 
            RedrawIcons(resourcePoints.CurrentPoints, resourcePoints.MaxPointsCount, resourcePoints.PreTakenCurrentPoints, resourcePointIcons, resourcePointIconPrefab, ref hashedResourcePointsCount));            
        }

        private void Update()
        {
            if(hashedPrevisualization != resourcePoints.CalculateHashFunctionOfPrevisualisation())
            {
                RedrawIcons(resourcePoints.CurrentPoints, resourcePoints.MaxPointsCount, resourcePoints.PreTakenCurrentPoints, resourcePointIcons, resourcePointIconPrefab, ref hashedResourcePointsCount);
            }

            hashedPrevisualization = resourcePoints.CalculateHashFunctionOfPrevisualisation();
        }

        private void RedrawIcons(int currentValue, int maxValue, int pretakenValue, List<ResourcePointUI> iconsList, ResourcePointUI iconPrefab, ref int hashedValue)
        {
            if(iconsList.Count < maxValue)
            {
                while (iconsList.Where(icon => icon.gameObject.activeSelf).ToList().Count < maxValue)
                {
                    var currentIcon = iconsList.FirstOrDefault(icon => !icon.gameObject.activeSelf);

                    if (currentIcon == null)
                        iconsList.Add(currentIcon = Instantiate(iconPrefab, iconsParent));

                    currentIcon.gameObject.SetActive(true);
                }
            }

            if(iconsList.Count > maxValue)
            {
                while(iconsList.Where(icon => icon.gameObject.activeSelf).ToList().Count > maxValue)
                {
                    iconsList.Where(icon => icon.gameObject.activeSelf).ToList().GetLast().gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < maxValue; i++)
            {
                iconsList[i].DisablePreCostIcon();
                iconsList[i].EnableEmptyIcon();

                if (i < currentValue)
                    iconsList[i].EnableFullIcon();
            }

            if (hashedValue < currentValue)
            {
                for (int i = hashedValue; i < currentValue; i++)
                {
                    iconsList[i].Regen();
                }
            }

            for (int i = pretakenValue; i < currentValue; i++)
            {
                iconsList[i].EnablePreCostIcon();
            }

            hashedValue = currentValue;
        }
    }
}