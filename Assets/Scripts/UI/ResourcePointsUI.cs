using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
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
        [SerializeField] private LocalizedString header;
        [SerializeField] private AbilityDescriptionTag descriptionTag;

        private int _hashedResourcePointsCount;
        private int _hashedTemporaryResourcePointsCount;
        private int _hashedPrevisualization;
        private Dictionary<string, Func<string>> _dataByKeyWords = new Dictionary<string, Func<string>>();

        private void Start()
        {
            if (currentResourcePoints != null)
                Init(currentResourcePoints);

            _dataByKeyWords.Add("{current_action_points}", () => currentResourcePoints.CurrentPoints.ToString());
            _dataByKeyWords.Add("{max_action_points}", () => currentResourcePoints.MaxPointsCount.ToString());
            _dataByKeyWords.Add("{action_points_regeneration}", () => currentResourcePoints.PointsRegeneration.ToString());
            _dataByKeyWords.Add("{temporary_action_points}", () => currentResourcePoints.TemporaryPoints.ToString());
        }

        public void Init(AbilityResourcePoints resourcePoints)
        {
            currentResourcePoints?.OnDefaultPointsIncreased.RemoveListener(RedrawCurrentActionPointsIcons);
            currentResourcePoints?.OnTemporaryPointsIncreased.RemoveListener(RedrawTemporaryActionPointsIcons);

            currentResourcePoints = resourcePoints;

            currentResourcePoints.OnDefaultPointsIncreased.AddListener(RedrawCurrentActionPointsIcons);
            currentResourcePoints.OnTemporaryPointsIncreased.AddListener(RedrawTemporaryActionPointsIcons);
        }

        private void OnEnable()
        {
            RedrawIcons(currentResourcePoints.CurrentPoints, currentResourcePoints.MaxPointsCount, currentResourcePoints.PreTakenCurrentPoints, resourcePointIcons, resourcePointIconPrefab, ref _hashedResourcePointsCount);
            RedrawIcons(currentResourcePoints.TemporaryPoints, currentResourcePoints.TemporaryPoints, currentResourcePoints.PreTakenTemporaryPoints, temporaryResourcePointIcons, temporaryResourcePointIconPrefab, ref _hashedTemporaryResourcePointsCount);
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
            if (_hashedPrevisualization != currentResourcePoints.CalculateHashFunctionOfPrevisualisation())
            {
                RedrawIcons(currentResourcePoints.CurrentPoints, currentResourcePoints.MaxPointsCount, currentResourcePoints.PreTakenCurrentPoints, resourcePointIcons, resourcePointIconPrefab, ref _hashedResourcePointsCount);
                RedrawIcons(currentResourcePoints.TemporaryPoints, currentResourcePoints.TemporaryPoints, currentResourcePoints.PreTakenTemporaryPoints, temporaryResourcePointIcons, temporaryResourcePointIconPrefab, ref _hashedTemporaryResourcePointsCount);
            }

            _hashedPrevisualization = currentResourcePoints.CalculateHashFunctionOfPrevisualisation();
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
            EntryPoint.Instance.CommonSmallHint.Init(header.GetLocalizedString(), GenerateStatsString(), transform, new Vector2(0.5f, 0), Vector2.up * 32);
            EntryPoint.Instance.CommonSmallHint.EnableContent(true);
        }

        public void HideHint()
        {
            EntryPoint.Instance.CommonSmallHint.EnableContent(false);
        }

        private string GenerateStatsString()
        {
            return Extensions.GenerateDescription(new List<AbilityDescriptionTag> { descriptionTag }, _dataByKeyWords, false);
        }
    }
}