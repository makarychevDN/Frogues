using UnityEngine;

namespace FroguesFramework
{
    public class AnimatedWall : MonoBehaviour
    {
        [SerializeField] private bool showed = true;
        [SerializeField] private Animator animator;
        [SerializeField] Transform thikcnessPointsOnTheFrontSide;

        public Transform ThikcnessPointsOnTheFrontSide => thikcnessPointsOnTheFrontSide;

        public bool Showed
        {
            get => showed;
            set 
            {
                if (showed == value)
                    return;

                showed = value;
                animator.SetBool("IsShowing", value);
            }
        }
    }
}