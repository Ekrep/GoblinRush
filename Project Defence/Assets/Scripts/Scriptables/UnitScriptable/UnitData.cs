using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Scriptables.UnitData
{
    [CreateAssetMenu(menuName = "Data/UnitData")]
    public class UnitData : SerializedScriptableObject
    {
        [Header("Render Values")]
        public Material[] materials;

        [Header("Stats")]
        public int health;
        public int attackDamage;
        public int range;
        public float movementSpeed;
    }

}

