using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables.Towers.AttackTower
{
    [CreateAssetMenu(menuName = "Data/TowerData/AttackTowerData")]
    public class AttackTowerData : TowerData
    {
        public float cooldown;
        public int attackDamage;
    }
}


