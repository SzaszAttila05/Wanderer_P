using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedTile : Tile
{
    [SerializeField] private Texture2D[] blockTextures;
    [SerializeField] private Texture2D dirtTexture; // Dirt text�ra hozz�ad�sa

    public override void Init(int x, int y)
    {
        base.Init(x, y); // H�vjuk meg az �soszt�ly Init met�dus�t

        // V�letlenszer�en v�lasszunk ki egy k�pet a blockTextures t�mbb�l
        var randomIndex = Random.Range(0, blockTextures.Length);
        var selectedTexture = blockTextures[randomIndex];

        // A kiv�lasztott k�p m�retez�se a Tile m�ret�hez
        var rect = new Rect(0, 0, selectedTexture.width, selectedTexture.height);

        // Sprite l�trehoz�sa a kiv�lasztott k�pb�l
        Renderer.sprite = Sprite.Create(selectedTexture, rect, new Vector2(0.5f, 0.5f));

        // Dirt text�ra hozz�ad�sa a h�tt�rhez
        AddDirtTexture();
    }

    private void AddDirtTexture()
    {
        // Ellen�rizz�k, hogy van-e dirt text�ra
        if (dirtTexture != null)
        {
            // Dirt Sprite l�trehoz�sa
            var dirtRect = new Rect(0, 0, dirtTexture.width, dirtTexture.height);
            var dirtSprite = Sprite.Create(dirtTexture, dirtRect, new Vector2(0.5f, 0.5f));

            // Dirt Sprite hozz�ad�sa a h�tt�rhez
            var dirtRenderer = new GameObject("DirtSprite").AddComponent<SpriteRenderer>();
            dirtRenderer.sprite = dirtSprite;
            dirtRenderer.transform.parent = transform; // Dirt Sprite a BlockedTile gyermek�v� tesz�se
            dirtRenderer.transform.localPosition = Vector3.zero; // Poz�cion�l�s a Tile k�zep�re
        }
    }
}
