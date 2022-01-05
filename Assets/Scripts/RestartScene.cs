
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CurrentlyActiveObjects.Clear();
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
