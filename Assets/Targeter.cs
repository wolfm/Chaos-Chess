using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour
{
    public abstract List<Vector2Int> GetTargets(int row, int col, Tile[,] tiles);
}
