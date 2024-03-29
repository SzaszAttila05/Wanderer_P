using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    [SerializeField] private Texture2D baseTexture;
    [SerializeField] private Texture2D offsetTexture;

    public override void Init(int x, int y)
    {
        var isOffset = (x + y) % 2 == 1;
        Renderer.sprite = isOffset ? Sprite.Create(offsetTexture, new Rect(0, 0, offsetTexture.width, offsetTexture.height), Vector2.one * 0.5f) : 
                                      Sprite.Create(baseTexture, new Rect(0, 0, baseTexture.width, baseTexture.height), Vector2.one * 0.5f);
    }
}
