using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTargeter : Targeter
{
    public override HashSet<Vector2Int> GetTargets(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved)
    {

        Debug.Log($"Getting targets for king at {piece_row}, {piece_col}");

        HashSet<Vector2Int> targets = new HashSet<Vector2Int>();
        int height = tiles.GetLength(0);
        int width = tiles.GetLength(1);

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

    public override HashSet<Vector2Int> GetMoves(int piece_row, int piece_col, Tile[,] tiles, bool hasMoved)
    {
        // Castling

        int height = tiles.GetLength(0);
        int width = tiles.GetLength(1);

        HashSet<Vector2Int> targets = new HashSet<Vector2Int>();

        Vector2Int[] directions = new Vector2Int[2]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        // If king is unmoved and not in check

        // TODO fix StackOverflow that occurs when uncommenting this condition
        // Previous Bug: checks the black king,which involves checking the white king, which involves checking the black king, etc.
        // Fix: check if it's this player's - the spots are never really "threatened" - so it doesn't matter if the opposing king
        // can castle or not when we just want to find out if our own king is in check
        if (!hasMoved && !tiles[piece_row, piece_col].Piece.IsChecked())
        {
            Team attackingTeam = tiles[piece_row, piece_col].Piece.team == Team.WHITE ? Team.BLACK : Team.WHITE;

            foreach (Vector2Int direction in directions)
            {
                int c_delta = direction.x;
                int r_delta = direction.y;

                int r = piece_row + r_delta;
                int c = piece_col + c_delta;

                while (r >= 0 && c >= 0 && r < height && c < width)
                {
                    // If the tile contains a piece
                    if (tiles[r, c].Piece)
                    {

                        // if it is a friendly unmoved rook, and the spaces the king would cross and arrive at
                        // are not in check, then add rook's square as target

                        if (tiles[r, c].Piece.type == "Rook" && tiles[r, c].Piece.hasMoved == false
                        && tiles[r, c].Piece.team == tiles[piece_row, piece_col].Piece.team
                        && !tiles[piece_row, piece_col + c_delta].IsChecked(attackingTeam)
                        && !tiles[piece_row, piece_col + 2 * c_delta].IsChecked(attackingTeam))
                        {
                            targets.Add(new Vector2Int(c, r));
                        }

                        break;
                    }
                    r += r_delta;
                    c += c_delta;
                }

            }
        }

        targets.UnionWith(GetTargets(piece_row, piece_col, tiles, hasMoved));

        return targets;
    }
}
