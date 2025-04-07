using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables.MapCreation.MapData;
public class MapCreator : MonoBehaviour
{
    public MapData mapData;
    public float cellXOffset;
    public float cellZOffset;
    public Vector3 tileScale;

    private void Update()
    {

    }

    private Vector3 GetMouseScreenPosToWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private Vector2Int ConvertWorldMousePositionToTilePosition(Vector3 mouseWorldPos)
    {
        return PositionConvertor.WorldPositionToTilePosition(mouseWorldPos, new Vector2(cellXOffset, cellZOffset));

    }

}
