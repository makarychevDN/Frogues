using UnityEngine;

namespace FroguesFramework
{
    public class TargetAsGameObjectIsActiveContainer : BoolContainer
    {
        [SerializeField] private GameObject target;
        public override bool Content => target.activeSelf;
    }
}
