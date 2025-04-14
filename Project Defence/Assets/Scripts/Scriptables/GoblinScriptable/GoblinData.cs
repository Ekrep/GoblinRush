using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptables.UnitData;
using static StaticHelpers.Util.Utils;

namespace Scriptables.UnitData.GoblinData
{
    [CreateAssetMenu(menuName = "Data/GoblinData")]
    public class GoblinData : UnitData
    {
        public GoblinType goblinType;
        public Sprite icon;
        public int trainCost;

    }
}

