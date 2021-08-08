using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbleToDie : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private UnityEvent OnDeath;

    public void Die()
    {
        _unit._currentCell.Content = null;
        UnitsQueue.Instance.Remove(_unit);
        Destroy(_unit.gameObject);
        OnDeath.Invoke();
    }

}
