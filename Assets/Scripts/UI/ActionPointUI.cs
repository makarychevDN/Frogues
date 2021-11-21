using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPointUI : MonoBehaviour
{
    [SerializeField] private GameObject fullActionPoint;
    [SerializeField] private GameObject preCostedActionPoint;
    [SerializeField] private GameObject emptyActionPoint;
    [SerializeField] private GameObject notEnoughActionPointsIcon;
    private List<GameObject> allIcons;

    private void Start()
    {
        allIcons = new List<GameObject>();
        allIcons.Add(fullActionPoint);
        allIcons.Add(preCostedActionPoint);
        allIcons.Add(emptyActionPoint);
        allIcons.Add(notEnoughActionPointsIcon);
        allIcons.RemoveAll(icon => icon == null);
    }

    public void EnableFullIcon()
    {
        DisableAllIcons();
        fullActionPoint.SetActive(true);
    }

    public void EnablePreCostIcon()
    {
        DisableAllIcons();
        preCostedActionPoint.SetActive(true);
    }

    public void EnableEmptyIcon()
    {
        DisableAllIcons();
        emptyActionPoint.SetActive(true);
    }

    public void EnableNotEnoghPointsIcon()
    {
        DisableAllIcons();
        notEnoughActionPointsIcon.SetActive(true);
    }

    private void DisableAllIcons() => allIcons.ForEach(icon => icon.SetActive(false));
}
