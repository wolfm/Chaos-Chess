using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTargeter : Targeter
{
    public override HashSet<Vector2Int> GetTargets(int piece_row, int piece_col, Tile[,] tiles)
    {
        int height = tiles.GetLength(0);
        int width = tiles.GetLength(1);

        HashSet<Vector2Int> targets = new HashSet<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[8]
        {
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1)
        };

        foreach (Vector2Int direction in directions)
        {
            int c_delta = direction.x;
            int r_delta = direction.y;

            int r = piece_row + r_delta;
            int c = piece_col + c_delta;

            // Add this tile to targets unless it's off the board there's a friendly piece on it
            if (r >= 0 && c >= 0 && r < height && c < width &&
                !(tiles[r, c].Piece 
                && tiles[r, c].Piece.team == tiles[piece_row, piece_col].Piece.team))
            {
                targets.Add(new Vector2Int(c, r));
            }

        }

        return targets;
    }
}
