using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockedTile : Tile
{
    [SerializeField] private Texture2D[] blockTextures;
    [SerializeField] private Texture2D dirtTexture; // Dirt textúra hozzáadása

    public override void Init(int x, int y)
    {
        base.Init(x, y); // Hívjuk meg az õsosztály Init metódusát

        // Véletlenszerûen válasszunk ki egy képet a blockTextures tömbbõl
        var randomIndex = Random.Range(0, blockTextures.Length);
        var selectedTexture = blockTextures[randomIndex];

        // A kiválasztott kép méretezése a Tile méretéhez
        var rect = new Rect(0, 0, selectedTexture.width, selectedTexture.height);

        // Sprite létrehozása a kiválasztott képbõl
        Renderer.sprite = Sprite.Create(selectedTexture, rect, new Vector2(0.5f, 0.5f));

        // Dirt textúra hozzáadása a háttérhez
        AddDirtTexture();
    }

    private void AddDirtTexture()
    {
        // Ellenõrizzük, hogy van-e dirt textúra
        if (dirtTexture != null)
        {
            // Dirt Sprite létrehozása
            var dirtRect = new Rect(0, 0, dirtTexture.width, dirtTexture.height);
            var dirtSprite = Sprite.Create(dirtTexture, dirtRect, new Vector2(0.5f, 0.5f));

            // Dirt Sprite hozzáadása a háttérhez
            var dirtRenderer = new GameObject("DirtSprite").AddComponent<SpriteRenderer>();
            dirtRenderer.sprite = dirtSprite;
            dirtRenderer.transform.parent = transform; // Dirt Sprite a BlockedTile gyermekévé teszése
            dirtRenderer.transform.localPosition = Vector3.zero; // Pozícionálás a Tile közepére
        }
    }
}
