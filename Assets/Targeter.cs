using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targeter : MonoBehaviour
{
    // Get the spaces that this Piece can move to and attack
    public abstract HashSet<Vector2Int> GetTargets(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved);

    // Get the spaces that this piece can move to (will often be identical to GetTargets)
    public abstract HashSet<Vector2Int> GetMoves(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved);
}
