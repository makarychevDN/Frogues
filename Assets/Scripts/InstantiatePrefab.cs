using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public void Run()
    {
        Instantiate(prefab);
    }
}
