using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnTargeter : Targeter
{
    public override List<Vector2Int> GetTargets(int piece_row, int piece_col, Tile[,] tiles)
    {
        int height = tiles.GetLength(0);
        int width = tiles.GetLength(1);

        // Direction is up if player one, down if player 2
        int dir = tiles[piece_row, piece_col].Piece.belongsToPlayerOne() ? 1 : -1;

        List<Vector2Int> targets = new List<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[2]
        {
            new Vector2Int(dir, 1),
            new Vector2Int(dir, -1)
        };

        // Check for open space ahead
        if (piece_row + dir >= 0 && piece_col >= 0 && piece_row + dir < height && piece_col < width &&
            !tiles[piece_row + dir, piece_col].Piece)
        {
            targets.Add(new Vector2Int(piece_col, piece_row + dir));
        }

        // Check for attackable spaces forward to the left and right
        foreach (Vector2Int direction in directions)
        {
            int c_delta = direction.x;
            int r_delta = direction.y;

            int r = piece_row + r_delta;
            int c = piece_col + c_delta;

            // Add this tile to targets if it's on the board a there's an enemy piece on it
            if (r >= 0 && c >= 0 && r < height && c < width &&
                tiles[r, c].Piece
                && tiles[r, c].Piece.isWhite != tiles[piece_row, piece_col].Piece.isWhite)
            {
                targets.Add(new Vector2Int(c, r));
            }

        }

        return targets;
    }
}
