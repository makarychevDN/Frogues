using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointUI : MonoBehaviour
{
    [SerializeField] private GameObject fullActionPoint;
    [SerializeField] private GameObject preCostedActionPoint;
    [SerializeField] private GameObject emptyActionPoint;
    [SerializeField] private GameObject notEnoughActionPointsIcon;

    public void EnableFullIcon()
    {
        fullActionPoint.SetActive(true);
        preCostedActionPoint.SetActive(false);
        emptyActionPoint.SetActive(false);
        notEnoughActionPointsIcon.SetActive(false);
    }

    public void EnablePreCostIcon()
    {
        fullActionPoint.SetActive(false);
        preCostedActionPoint.SetActive(true);
        emptyActionPoint.SetActive(false);
        notEnoughActionPointsIcon.SetActive(false);
    }

    public void EnableEmptyIcon()
    {
        fullActionPoint.SetActive(false);
        preCostedActionPoint.SetActive(false);
        emptyActionPoint.SetActive(true);
        notEnoughActionPointsIcon.SetActive(false);
    }

    public void EnableNotEnoghPointsIcon()
    {
        fullActionPoint.SetActive(false);
        preCostedActionPoint.SetActive(false);
        emptyActionPoint.SetActive(false);
        notEnoughActionPointsIcon.SetActive(true);
    }
}
