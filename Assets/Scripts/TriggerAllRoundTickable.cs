using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class TriggerAllRoundTickable : MonoBehaviour
    {
        public void RoundTick()
        {
            FindObjectsOfType<MonoBehaviour>().OfType<IRoundTickable>().ToList().ForEach(x => x.Tick());
        }
    }
}