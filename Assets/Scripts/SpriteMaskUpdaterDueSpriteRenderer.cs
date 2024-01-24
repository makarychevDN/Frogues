using UnityEngine;

public class SpriteMaskUpdaterDueSpriteRenderer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteMask spriteMask;

    void Awake()
    {
        spriteMask.sprite = spriteRenderer.sprite;
        spriteRenderer.RegisterSpriteChangeCallback(GetSpriteFromSpriteRender);
    }

    private void GetSpriteFromSpriteRender(SpriteRenderer spriteRenderer)
    {
        spriteMask.sprite = spriteRenderer.sprite;
    }
}
