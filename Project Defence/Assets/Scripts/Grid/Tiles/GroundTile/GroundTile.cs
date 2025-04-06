using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : BaseTile
{
    public GroundTile parent;
    public int g, h;
    public int f => g + h;
  
}
