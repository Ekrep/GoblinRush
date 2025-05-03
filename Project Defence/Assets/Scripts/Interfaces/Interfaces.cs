using UnityEngine;
public interface IAssignableUnit
{
    public void PassAssignedIndex(int index, Vector2Int tilePos);
    public int GetInstanceId();
    public Vector3 GetWorldPosition();
    public bool IsAlive();
    public void Damage(int damageAmount);
    public void Death();
}
public interface IGoblin : IAssignableUnit
{
    public void ApplyDebuff();
    public void RemoveDebuff();
}
public interface INonGoblin : IAssignableUnit
{
}
public interface IDamageableBuilding
{
    public void Damage(int damageAmount);
    public void DestroyBuilding();
}
namespace PoolSystem.Poolable
{
    public interface IPoolable
    {
        public void OnCreatedForPool();
        public void OnAssignPool();
        public void OnEnqueuePool();
        public void OnDequeuePool();
        public void OnDeletePool();

    }

}


