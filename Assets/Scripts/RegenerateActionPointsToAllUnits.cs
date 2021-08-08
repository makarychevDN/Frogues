using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegenerateActionPointsToAllUnits : MonoBehaviour
{
    public void Regenarete()
    {
        FindObjectsOfType<ActionPoints>().ToList().ForEach(x => x.RegeneratePoints());
    }
}
