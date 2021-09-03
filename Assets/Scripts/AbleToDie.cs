using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbleToDie : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private UnityEvent OnDeath;

    public void Die()
    {
        unit.currentCell.Content = null;
        UnitsQueue.Instance.Remove(unit);
        Destroy(unit.gameObject);
        OnDeath.Invoke();
    }

}
