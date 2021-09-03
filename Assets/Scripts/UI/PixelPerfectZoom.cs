using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PixelPerfectZoom : MonoBehaviour
{
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;
    [SerializeField] private Slider slider;
    private int[] _xResolutions = new int[] { 336, 420, 560, 840, 2200 };

    public void UpdateResolution()
    {
        pixelPerfectCamera.refResolutionX = _xResolutions[(int)slider.value];
    }
}
