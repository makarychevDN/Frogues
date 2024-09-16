using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FroguesFramework
{
    public abstract class ResourcesPointsUIController<T> : MonoBehaviour
    {
        [Header("Resizing Setup")]
        [SerializeField] private RectTransform resizableParent;

        [Header("Icons setup")]
        [SerializeField] private ResourcePointUI resourcePointIconPrefab;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons;
        [SerializeField] private SerializedDictionary<int, int> widthOfIconBasedOnMaxCountOfAllIcons;

        [Header("Rows setup")]
        [SerializeField] private int maxPossibleCountOfIconsInRow;
        [SerializeField] private Transform parentOfIconRows;
        [SerializeField] private RowOfResourceIcons iconsParentPrefab;
        [SerializeField] private List<RowOfResourceIcons> iconsParents;

        public UnityEvent OnIconsRedrawed;

        private int _hashedValue;
        private int _hashedMaxValue;
        private int _hashedRowsCount;

        public abstract void Init(T dataSource);

        public void RedrawIcons(int currentValue, int maxValue, int pretakenValue)
        {
            if(_hashedMaxValue != maxValue)
            {
                int countOfDefaultRows = maxValue / maxPossibleCountOfIconsInRow;
                int countOfAdditionalRows = maxValue % maxPossibleCountOfIconsInRow == 0 ? 0 : 1;
                int countOfRows = countOfDefaultRows + countOfAdditionalRows;
                int sizeOfDefaultRow = (int)Math.Ceiling((float)maxValue / countOfRows);

                TryToAddNewRows(countOfRows);
                TryToDisableExtraRows(countOfRows);

                TryToAddNewIcons(maxValue);
                TryToRemoveExtraIcons(maxValue);

                if(_hashedRowsCount != countOfRows)
                    TryToUpdateTransformsOfIcons(sizeOfDefaultRow, countOfRows);

                if (resizableParent != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent);

                _hashedRowsCount = countOfRows;
            }

            DrawFullIcons(currentValue, maxValue);
            TryToDrawRegenedIcons(currentValue);
            DrawPrecostedIcons(currentValue, pretakenValue);

            _hashedValue = currentValue;
            _hashedMaxValue = maxValue;
            OnIconsRedrawed.Invoke();
        }

        private void TryToAddNewRows(int rowsCount)
        {
            while(rowsCount > iconsParents.Where(iconParent => iconParent.gameObject.activeSelf).ToList().Count)
            {
                var currentParent = iconsParents.FirstOrDefault(icon => !icon.gameObject.activeSelf);

                if (currentParent == null)
                    iconsParents.Add(currentParent = Instantiate(iconsParentPrefab, parentOfIconRows));

                currentParent.gameObject.SetActive(true);
            }
        }

        private void TryToDisableExtraRows(int rowsCount)
        {
            while (iconsParents.Where(icon => icon.gameObject.activeSelf).ToList().Count > rowsCount)
            {
                iconsParents.Where(icon => icon.gameObject.activeSelf).ToList().GetLast().gameObject.SetActive(false);
            }
        }

        private void TryToAddNewIcons(int maxValue)
        {
            while (resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().Count < maxValue)
            {
                var currentIcon = resourcePointIcons.FirstOrDefault(icon => !icon.gameObject.activeSelf);

                if (currentIcon == null)
                    resourcePointIcons.Add(currentIcon = Instantiate(resourcePointIconPrefab, iconsParents.Last(iconsParent => iconsParent.gameObject.activeInHierarchy).transform));

                currentIcon.gameObject.SetActive(true);
            }
        }

        private void TryToRemoveExtraIcons(int maxValue)
        {
            while (resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().Count > maxValue)
            {
                resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().GetLast().gameObject.SetActive(false);
            }
        }

        private void TryToUpdateTransformsOfIcons(int sizeOfDefaultRow, int rowsCount)
        {
            if (_hashedRowsCount == rowsCount)
                return;

            int idOfCurrentParent = 0;
            var activeResourcePoints = resourcePointIcons.Where(resourcePoint => resourcePoint.gameObject.activeInHierarchy);

            foreach (var resourcePointIcon in resourcePointIcons)
            {
                resourcePointIcon.transform.parent = null;
            }

            foreach (var resourcePointIcon in resourcePointIcons)
            {
                resourcePointIcon.transform.SetParent(iconsParents[idOfCurrentParent].ParentOfIcons);
                resourcePointIcon.SetWidthOfResizableElement(widthOfIconBasedOnMaxCountOfAllIcons.FirstOrDefault(width => width.Key > sizeOfDefaultRow).Value);
                resourcePointIcon.transform.localEulerAngles = Vector3.zero;
                resourcePointIcon.transform.localScale = Vector3.one;
                resourcePointIcon.transform.localPosition = Vector3.zero;

                if(iconsParents[idOfCurrentParent].CountOfIcons == sizeOfDefaultRow)
                {
                    idOfCurrentParent++;
                    idOfCurrentParent = Mathf.Clamp(idOfCurrentParent, 0, iconsParents.Count - 1);
                }
            }
        }

        private void DrawFullIcons(int currentValue, int maxValue)
        {
            for (int i = 0; i < maxValue; i++)
            {
                resourcePointIcons[i].DisablePreCostIcon();
                resourcePointIcons[i].EnableEmptyIcon();

                if (i < currentValue)
                    resourcePointIcons[i].EnableFullIcon();
            }
        }

        private void TryToDrawRegenedIcons(int currentValue)
        {
            if (_hashedValue < currentValue)
            {
                for (int i = _hashedValue; i < currentValue; i++)
                {
                    resourcePointIcons[i].Regen();
                }
            }
        }

        private void DrawPrecostedIcons(int currentValue, int pretakenValue)
        {
            for (int i = Mathf.Clamp(pretakenValue, 0, 10000); i < currentValue; i++)
            {
                resourcePointIcons[i].EnablePreCostIcon();
            }
        }
    }
}