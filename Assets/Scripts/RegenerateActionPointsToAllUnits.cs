using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class RegenerateActionPointsToAllUnits : MonoBehaviour
    {
        public void Regenarete()
        {
            FindObjectsOfType<ActionPoints>().ToList().ForEach(x => x.RegeneratePoints());
        }
    }
}