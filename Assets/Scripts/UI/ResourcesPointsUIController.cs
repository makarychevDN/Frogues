using AYellowpaper.SerializedCollections;
using FroguesFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace FroguesFramework
{
    public abstract class ResourcesPointsUIController<T> : MonoBehaviour
    {
        [SerializeField] private ResourcePointUI resourcePointIconPrefab;
        [SerializeField] private GameObject visualSplitterBetweenPointsIcons;
        [SerializeField] private bool needToAddSplitters;
        [SerializeField] private Transform iconsParent;
        [SerializeField] private RectTransform resizableParent;
        [SerializeField] private List<ResourcePointUI> resourcePointIcons;
        [SerializeField] private List<GameObject> splitterObjects;
        [SerializeField] private SerializedDictionary<int, int> widthOfIconBasedOnMaxCountOfAllIcons;

        public UnityEvent OnIconsRedrawed;

        private int _hashedValue;
        private int _hashedMaxValue;

        public abstract void Init(T dataSource);

        public void RedrawIcons(int currentValue, int maxValue, int pretakenValue)
        {
            if (resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().Count < maxValue)
            {
                while (resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().Count < maxValue)
                {
                    GameObject currentSplitter = null;
                    var currentIcon = resourcePointIcons.FirstOrDefault(icon => !icon.gameObject.activeSelf);

                    if (needToAddSplitters)
                        currentSplitter = splitterObjects.FirstOrDefault(icon => !icon.gameObject.activeSelf);

                    if (currentIcon == null)
                    {
                        resourcePointIcons.Add(currentIcon = Instantiate(resourcePointIconPrefab, iconsParent));

                        if (needToAddSplitters)
                            splitterObjects.Add(currentSplitter = Instantiate(visualSplitterBetweenPointsIcons, iconsParent));
                    }

                    currentIcon.gameObject.SetActive(true);

                    if (needToAddSplitters)
                        currentSplitter.gameObject.SetActive(true);
                }

                if (_hashedMaxValue != maxValue)
                {
                    resourcePointIcons.ForEach(resourcePointIcon => (resourcePointIcon.transform as RectTransform)
                        .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
                        widthOfIconBasedOnMaxCountOfAllIcons.FirstOrDefault(width => width.Key > maxValue).Value));
                }

                if (resizableParent != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent);
            }

            if (resourcePointIcons.Count > maxValue)
            {
                while (resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().Count > maxValue)
                {
                    resourcePointIcons.Where(icon => icon.gameObject.activeSelf).ToList().GetLast().gameObject.SetActive(false);

                    if (needToAddSplitters)
                        splitterObjects.Where(splitter => splitter.activeSelf).ToList().GetLast().gameObject.SetActive(false);
                }

                if (resizableParent != null)
                    LayoutRebuilder.ForceRebuildLayoutImmediate(resizableParent);
            }

            for (int i = 0; i < maxValue; i++)
            {
                resourcePointIcons[i].DisablePreCostIcon();
                resourcePointIcons[i].EnableEmptyIcon();

                if (i < currentValue)
                    resourcePointIcons[i].EnableFullIcon();
            }

            if (_hashedValue < currentValue)
            {
                for (int i = _hashedValue; i < currentValue; i++)
                {
                    resourcePointIcons[i].Regen();
                }
            }

            for (int i = Mathf.Clamp(pretakenValue, 0, 10000); i < currentValue; i++)
            {
                resourcePointIcons[i].EnablePreCostIcon();
            }

            _hashedValue = currentValue;
            OnIconsRedrawed.Invoke();
        }
    }
}