using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace FroguesFramework
{
    public class ResourcePointsUI : MonoBehaviour
    {
        [SerializeField] private AbilityResourcePoints currentResourcePoints;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private ResourcePointUI resourcePointIconPrefab;
        [SerializeField] private ResourcePointUI temporaryResourcePointIconPrefab;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons = new();
        [SerializeField] private List<ResourcePointUI> temporaryResourcePointIcons = new();
        [SerializeField] private bool generateIconsOnStart;
        [SerializeField] private RectTransform resizableParent;

        [Header("hint")]
        [SerializeField] private string header;

        private int _hashedResourcePointsCount;
        private int _hashedTemporaryResourcePointsCount;
        private int hashedPrevisualization;

        private void Start()
        {
            if (currentResourcePoints != null)
                Init(currentResourcePoints);
        }

        public void Init(AbilityResourcePoints resourcePoints)
        {
            currentResourcePoints?.OnDefaultPointsIncreased.RemoveListener(RedrawCurrentActionPointsIcons);
            currentResourcePoints?.OnTemporaryPointsIncreased.RemoveListener(RedrawTemporaryActionPointsIcons);

            currentResourcePoints = resourcePoints;

            currentResourcePoints.OnDefaultPointsIncreased.AddListener(RedrawCurrentActionPointsIcons);
            currentResourcePoints.OnTemporaryPointsIncreased.AddListener(RedrawTemporaryActionPointsIcons);
        }

        private void RedrawCurrentActionPointsIcons() =>
            RedrawIcons(currentResourcePoints.CurrentPoints,
            currentResourcePoints.MaxPointsCount,
            currentResourcePoints.PreTakenCurrentPoints,
            resourcePointIcons,
            resourcePointIconPrefab,
            ref _hashedResourcePointsCount);

        private void RedrawTemporaryActionPointsIcons() =>
            RedrawIcons(currentResourcePoints.TemporaryPoints,
            currentResourcePoints.TemporaryPoints,
            currentResourcePoints.PreTakenTemporaryPoints,
            temporaryResourcePointIcons,
            temporaryResourcePointIconPrefab,
            ref _hashedTemporaryResourcePointsCount);

        private void Update()
        {
            if (hashedPrevisualization != currentResourcePoints.CalculateHashFunctionOfPrevisualisation())
            {
                RedrawIcons(currentResourcePoints.CurrentPoints, currentResourcePoints.MaxPointsCount, currentResourcePoints.PreTakenCurrentPoints, resourcePointIcons, resourcePointIconPrefab, ref _hashedResourcePointsCount);
                RedrawIcons(currentResourcePoints.TemporaryPoints, currentResourcePoints.TemporaryPoints, currentResourcePoints.PreTakenTemporaryPoints, temporaryResourcePointIcons, temporaryResourcePointIconPrefab, ref _hashedTemporaryResourcePointsCount);
            }

            hashedPrevisualization = currentResourcePoints.CalculateHashFunctionOfPrevisualisation();
        }

        private void RedrawIcons(int currentValue, int maxValue, int pretakenValue, List<ResourcePointUI> iconsList, ResourcePointUI iconPrefab, ref int hashedValue)
        {
            if (iconsList.Where(icon => icon.gameObject.activeSelf).ToList().Count < maxValue)
            {
                while (iconsList.Where(icon => icon.gameObject.activeSelf).ToList().Count < maxValue)
                {
                    var currentIcon = iconsList.FirstOrDefault(icon => !icon.gameObject.activeSelf);

                    if (currentIcon == null)
                        iconsList.Add(currentIcon = Instantiate(iconPrefab, iconsParent));

                    currentIcon.gameObject.SetActive(true);
                }

                if (resizableParent != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent);
            }

            if (iconsList.Count > maxValue)
            {
                while (iconsList.Where(icon => icon.gameObject.activeSelf).ToList().Count > maxValue)
                {
                    iconsList.Where(icon => icon.gameObject.activeSelf).ToList().GetLast().gameObject.SetActive(false);
                }

                if (resizableParent != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent);
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

        public void ShowHint()
        {
            EntryPoint.Instance.AbilityHint.Init(header, GenerateStatsString(), "", transform, new Vector2(0.5f, 0), Vector2.up * 32);
            EntryPoint.Instance.AbilityHint.EnableContent(true, true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.AbilityHint.EnableContent(false);
        }

        private string GenerateStatsString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Текущий запас: {currentResourcePoints.CurrentPoints}")
                .AppendLine($"Максимальный запас: {currentResourcePoints.MaxPointsCount}")
                .AppendLine($"Регенерация: {currentResourcePoints.PointsRegeneration}")
                .AppendLine($"Временные очки: {currentResourcePoints.TemporaryPoints}");

            return sb.ToString();
        }
    }
}