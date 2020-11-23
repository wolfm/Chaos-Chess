using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookTargeter : Targeter
{

    public override HashSet<Vector2Int> GetTargets(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved)
    {
        int height = tiles.GetLength(0);
        int width = tiles.GetLength(1);

        HashSet<Vector2Int> targets = new HashSet<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[4]
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1)
        };

        foreach (Vector2Int direction in directions)
        {
            int c_delta = direction.x;
            int r_delta = direction.y;

            int r = piece_row + r_delta;
            int c = piece_col + c_delta;

            while (r >= 0 && c >= 0 && r < height && c < width)
            {
                // If the tile contains a piece, break out of the while loop
                if (tiles[r, c].Piece)
                {
                    // If the piece on this tile is the opposing player's, add it to the targets HashSet before breaking
                    if (tiles[r, c].Piece.team != tiles[piece_row, piece_col].Piece.team)
                    {
                        targets.Add(new Vector2Int(c, r));
                    }
                    break;
                }
                targets.Add(new Vector2Int(c, r));
                r += r_delta;
                c += c_delta;
            }

        }

        return targets;
    }
}
