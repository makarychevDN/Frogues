using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescription : MonoBehaviour
    {
        [SerializeField] private string unitName;
        [SerializeField, Multiline] private string description;

        public string UnitName => unitName;
        public string Description => description;
    }
}