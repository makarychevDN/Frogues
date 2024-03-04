using FroguesFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReseter : MonoBehaviour
{
    public void ResetCamera() => EntryPoint.Instance.CameraController.ResetCamera();
}
