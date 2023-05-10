using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float tickRate;
    private float timer;
    private string fpsCounter;

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), fpsCounter);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > tickRate) 
        {
            fpsCounter = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
            timer = 0;
        }

    }
}
