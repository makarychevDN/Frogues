using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitDescription : MonoBehaviour
    {
        [SerializeField, Multiline] private string description;
        public string Description => description;
    }
}