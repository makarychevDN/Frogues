using System;
using UnityEngine;

namespace FroguesFramework
{
    public class RoundsCounter : MonoBehaviour, IRoundTickable
    {
        [SerializeField] private int count;
        public int Count => count;
        
        public void Tick()
        {
            count++;
        }
    }
}
