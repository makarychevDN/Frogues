using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FroguesFramework
{
    public class UnitsUIEnabler : MonoBehaviour
    {
        public static HashSet<UnitsUI> UIOfAllUnits = new HashSet<UnitsUI>();

        public void AllUnitsUISetActive(bool value)
        {
            UIOfAllUnits.Where(ui => ui != null).ToList().ForEach(ui => ui.SetActiveForAllVisualizationObjects(value));
        }
    }
}