using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class DecreaseAllTImers : MonoBehaviour
    {
        public void Decrease()
        {
            FindObjectsOfType<RoundTimerEvent>().ToList().ForEach(x => x.DecreaseTimer());
        }
    }
}