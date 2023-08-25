using UnityEngine;

namespace FroguesFramework
{
    public class ResourcePointUI : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void EnableFullIcon()
        {
            animator.SetBool("Full", true);
        }

        public void EnablePreCostIcon()
        {
            animator.SetBool("PreCost", true);
            animator.SetTrigger("ResetPreCostBlinking");
        }

        public void DisablePreCostIcon()
        {
            animator.SetBool("PreCost", false);
        }

        public void EnableEmptyIcon()
        {
            animator.SetBool("Full", false);
        }

        public void Regen()
        {
            animator.SetTrigger("Regen");
        }
    }
}