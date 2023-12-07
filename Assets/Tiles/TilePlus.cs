using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile Plus", menuName = "Other/Tile Plus")]
public class TilePlus : Tile
{
    public Sprite newSprite;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        if (newSprite != null) tileData.sprite = newSprite;
    }
}
