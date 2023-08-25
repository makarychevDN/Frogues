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
        [SerializeField] private Animator animator;
        private List<GameObject> allIcons;

        private void Start()
        {
            InitAllIcons();
        }

        private void InitAllIcons()
        {
            allIcons = new List<GameObject>
            {
                fullResourcePointIcon,
                preCostedResourcePoint,
                emptyResourcePoint,
                notEnoughResourcePointsIcon
            };
            allIcons.RemoveAll(icon => icon == null);
        }

        public void EnableFullIcon()
        {
            if (animator != null)
                animator.SetBool("Full", true);
        }

        public void EnablePreCostIcon()
        {
            if (animator != null)
            {
                animator.SetBool("PreCost", true);
                animator.SetTrigger("ResetPreCostBlinking");
            }
        }

        public void DisablePreCostIcon()
        {
            if (animator != null)
                animator.SetBool("PreCost", false);
        }

        public void EnableEmptyIcon()
        {
            if (animator != null)
                animator.SetBool("Full", false);
        }

        public void Regen()
        {
            if(animator != null)
                animator.SetTrigger("Regen");
        }

        private void DisableAllIcons() => allIcons?.ForEach(icon => icon.SetActive(false));
    }
}