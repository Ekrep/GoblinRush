using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables.UnitData;

public class Unit : MonoBehaviour
{
    public Vector2Int currentTilePosition;
    [SerializeField] private UnitData unitData;
    public UnitData UnitData => unitData;
    #region Stats
    [HideInInspector] public int health;
    [HideInInspector] public int attackDamage;
    [HideInInspector] public int range;
    [HideInInspector] public float movementSpeed;
    #endregion
    #region Render
    private MeshRenderer[] renderers;
    private MeshFilter[] filters;
    private Material[] materials;

    #endregion
    public void SetUnitDataAndInitialize(UnitData unitData)
    {
        this.unitData = unitData;
        InitializeUnitStats();
        InitializeUnitRenderData();
    }

    public void InitializeUnitStats()
    {
        health = unitData.health;
        attackDamage = unitData.attackDamage;
        range = unitData.range;
        movementSpeed = unitData.movementSpeed;
    }
    public void InitializeUnitRenderData()
    {
        materials = new Material[unitData.materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = unitData.materials[i];
        }
    }
    protected void Move(Vector3 worldPos)
    {
        MoveWithAnim(worldPos);
    }
    protected void Move(Vector2Int tilePos)
    {
        MoveWithAnim(GridMap.Instance.TilePositionToWorldPosition(tilePos));
    }
    protected void Move(BaseTile tile)
    {
        MoveWithAnim(tile.worldPosition);
    }
    protected void MoveRandomPositionInTile(Vector2Int tilePos)
    {
        MoveWithAnim(GridMap.Instance.GetRandomPointInsideTheTile(tilePos));
    }
    private void MoveWithAnim(Vector3 position)
    {
        StartCoroutine(MoveWithAnimCorot(position));
    }

    private IEnumerator MoveWithAnimCorot(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, movementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        currentTilePosition = GridMap.Instance.WorldPositionToTilePosition(position);
    }



}
