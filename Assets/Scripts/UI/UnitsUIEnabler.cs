using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitsUIEnabler : MonoBehaviour
{
    public void AllUnitsUISetActive(bool value)
    {
        Map.Instance.allCells.WithContentOnly().Where(cell => cell.Content.UI != null).ToList().ForEach(cell => cell.Content.UI.SetActive(value));
    }
}
