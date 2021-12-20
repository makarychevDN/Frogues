using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecreaseAllTImers : MonoBehaviour
{
    public void Decrease()
    {
        FindObjectsOfType<RoundTimerEvent>().ToList().ForEach(x => x.DecreaseTimer());
    }
}
