using System;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class PrePushVisualization : MonoBehaviour
    {
        [SerializeField] private List<TargetWithDirection> arrows;
        [SerializeField] private HexDirContainer prePushValueContainer;

        void Update()
        {
            arrows.ForEach(arrow => arrow.target.SetActive(arrow.direction == prePushValueContainer.Content));
        }

        [Serializable]
        public struct TargetWithDirection
        {
            public GameObject target;
            public HexDir direction;
        }
    }
}
