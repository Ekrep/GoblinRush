using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Scriptables.UnitData
{
    [CreateAssetMenu(menuName = "Data/UnitData")]
    public class UnitData : SerializedScriptableObject
    {
        [Header("UI")]
        public Sprite icon;
        public string unitName;
        [Header("Stats")]
        public int health;
        public int attackDamage;
        public int range;
        public float movementSpeed;
        public int trainCost;
        public int deathIncome;
    }

}

