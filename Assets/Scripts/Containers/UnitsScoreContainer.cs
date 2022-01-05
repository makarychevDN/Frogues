using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsScoreContainer : IntContainer
{
    private void OnDestroy()
    {
        FindObjectOfType<Score>().Content += Content;
    }
}
