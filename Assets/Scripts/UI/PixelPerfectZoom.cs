using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class PixelPerfectZoom : MonoBehaviour
{
    [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;
    [SerializeField] private Slider _slider;

    [SerializeField, ReadOnly]
    private int[] xResolutions = new int[]
    {
        336, 420, 560, 840, 2200
    };

    public void UpdateResolution()
    {
        _pixelPerfectCamera.refResolutionX = xResolutions[(int)_slider.value];
    }
}
