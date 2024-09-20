using UnityEngine;

namespace FroguesFramework
{
    public class RowOfResourceIcons : MonoBehaviour
    {
        [SerializeField] private Transform parentOfIcons;

        public Transform ParentOfIcons => parentOfIcons;

        //public int CountOfIcons => transform.Cast<Transform>().Where(child => child.gameObject.activeSelf).Count();
        public int CountOfIcons => parentOfIcons.childCount;
    }
}