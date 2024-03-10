using FroguesFramework;
using UnityEngine;

public class CameraReseter : MonoBehaviour
{
    public void ResetCamera() => EntryPoint.Instance.CameraController.ResetCamera();
}
