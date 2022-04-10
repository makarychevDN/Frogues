using System.Collections.Generic;
using UnityEngine;

namespace FroguesFramework
{
    public abstract class BaseCellsTaker : MonoBehaviour
    {
        public abstract List<Cell> Take();
    }
}