using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class ResourcePointUI : MonoBehaviour
    {
        [SerializeField] private GameObject fullResourcePointIcon;
        [SerializeField] private GameObject preCostedResourcePoint;
        [SerializeField] private GameObject emptyResourcePoint;
        [SerializeField] private GameObject notEnoughResourcePointsIcon;
        private List<GameObject> allIcons;

        private void Start()
        {
            InitAllIcons();
        }

        private void InitAllIcons()
        {
            allIcons = new List<GameObject>();
            allIcons.Add(fullResourcePointIcon);
            allIcons.Add(preCostedResourcePoint);
            allIcons.Add(emptyResourcePoint);
            allIcons.Add(notEnoughResourcePointsIcon);
            allIcons.RemoveAll(icon => icon == null);
        }

        public void EnableFullIcon()
        {
            DisableAllIcons();
            fullResourcePointIcon.SetActive(true);
        }

        public void EnablePreCostIcon()
        {
            DisableAllIcons();
            preCostedResourcePoint.SetActive(true);
        }

        public void EnableEmptyIcon()
        {
            DisableAllIcons();
            emptyResourcePoint.SetActive(true);
        }

        public void EnableNotEnoughPointsIcon()
        {
            DisableAllIcons();
            notEnoughResourcePointsIcon.SetActive(true);
        }

        private void DisableAllIcons() => allIcons?.ForEach(icon => icon.SetActive(false));
    }
}