using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbleToSkipTurn : MonoBehaviour
{
    public void AutoSkip()
    {
        UnitsQueue.Instance.ActivateNext();
    }

    public void SkipTurnWithAnimation()
    {
        UnitsQueue.Instance.ActivateNext();
    }
}
