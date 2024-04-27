using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class AnimatedWall : MonoBehaviour
    {
        [SerializeField] private bool showed = true;
        [SerializeField] private List<Animator> animators;
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
                animators.ForEach(animator => animator.SetBool("IsShowing", value));
            }
        }
    }
}