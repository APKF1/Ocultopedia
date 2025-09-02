using UnityEngine;

public class LiquidPiece : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public void SetColor(Color color)
    {
        if (spriteRenderer != null)
            spriteRenderer.color = color;
    }
}
