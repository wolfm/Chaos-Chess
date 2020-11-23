using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnTargeter : Targeter
{
    public override HashSet<Vector2Int> GetMoves(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved)
    {
        return GetTargets(piece_row, piece_col, tiles, hasMoved);
    }
    public override HashSet<Vector2Int> GetTargets(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved)
    {

        int height = tiles.GetLength(0);
        int width = tiles.GetLength(1);

        if (tiles == null) Debug.Log("tiles is null");
        if (tiles[piece_row,piece_col] == null) Debug.Log($"tiles[{piece_row},{piece_col}] is null");
        if (tiles[piece_row, piece_col].Piece == null) Debug.Log($"tiles[{piece_row},{piece_col}].Piece is null");

        // Direction is up if player one, down if player 2
        int dir = tiles[piece_row, piece_col].Piece.BelongsToPlayerOne() ? 1 : -1;

        HashSet<Vector2Int> targets = new HashSet<Vector2Int>();
        Vector2Int[] directions = new Vector2Int[2]
        {
            new Vector2Int(1, dir),
            new Vector2Int(-1, dir)
        };

        // Check for open space ahead
        if (piece_row + dir >= 0 && piece_col >= 0 && piece_row + dir < height && piece_col < width &&
            !tiles[piece_row + dir, piece_col].Piece)
        {
            targets.Add(new Vector2Int(piece_col, piece_row + dir));
        }

        // If hasn't moved, check two spaces ahead
        if (!hasMoved && piece_row + 2*dir >= 0 && piece_col >= 0 && piece_row + 2*dir < height && piece_col < width &&
            !tiles[piece_row + 2*dir, piece_col].Piece)
        {
            targets.Add(new Vector2Int(piece_col, piece_row + 2*dir));
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
                && tiles[r, c].Piece.team != tiles[piece_row, piece_col].Piece.team)
            {
                targets.Add(new Vector2Int(c, r));
            }

        }

        return targets;
    }
}
