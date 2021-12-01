using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitToContainer : MonoBehaviour
{
    [SerializeField] private UnitContainer unitContainer;

    void Start()
    {
        unitContainer.Content = FindObjectOfType<PlayerInput>().GetComponentInParent<Unit>();
    }
}
