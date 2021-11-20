using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderInLayerUpdater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private int defaultOrderValue;
    private float roundingAccuracyMod = 10;

    private void Start()
    {
        defaultOrderValue = spriteRenderer.sortingOrder;
    }

    void Update()
    {
        spriteRenderer.sortingOrder = defaultOrderValue - Mathf.RoundToInt(spriteRenderer.transform.position.y * roundingAccuracyMod);
    }
}
