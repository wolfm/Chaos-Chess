using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Tile tile;
    public Targeter targeter;
    public bool isKingPiece;
    public bool hasMoved;

    private Piece destroyedInSimulation;
    private Tile movedFrom;

    public string type;

    public Team team;

    public void moveToTile(Tile moveTarget)
    {

        // Castling
        if(type == "King" && moveTarget.Piece && moveTarget.Piece.type == "Rook" && team == moveTarget.Piece.team)
        {
            int kingCol = tile.col;
            int rookCol = moveTarget.Piece.tile.col;
            int kingDir = (rookCol - kingCol) / System.Math.Abs(rookCol - kingCol);
            moveTarget.Piece.moveToTile(tile.board.tiles[tile.row, rookCol + (kingDir == 1 ? -2 : 3)]);
            moveToTile(tile.board.tiles[tile.row, kingCol + 2*kingDir]);
            return;
        }

        if(tile) Debug.Log($"Moving {team} {type} at r{tile.row} c{tile.col} to r{moveTarget.row} c{moveTarget.col}");
        
        if (moveTarget.Piece)
        {
            // Kill the piece on this tile
            if(moveTarget.Piece.team == Team.WHITE)
            {
                tile.board.whitePieces.Remove(moveTarget.Piece);
            }
            else {
                tile.board.blackPieces.Remove(moveTarget.Piece);
            }
            Destroy(moveTarget.Piece.gameObject);
        }
        // If moving from a tile, set its piece reference to null
        if (tile)
        {
            tile.Piece = null;

            // Mark piece as moved
            hasMoved = true;
        }
        
        
        // Set tile reference to the new tile
        tile = moveTarget;

        // Set the new tile's piece reference to this piece
        tile.Piece = this;

        // Visualy move the piece
        transform.position = moveTarget.transform.position;

    }

    public void simulateMove(Tile moveTarget)
    {
        if (tile) Debug.Log($"Simulating move of {team} {type} at r{tile.row} c{tile.col} to r{moveTarget.row} c{moveTarget.col}");

        if (moveTarget.Piece)
        {
            // Kill the piece on this tile
            destroyedInSimulation = moveTarget.Piece;
            Debug.Log("Simulated kill");
        }
        // If on a tile, set its piece reference to null
        if (tile) tile.Piece = null;

        // Store tile before move
        movedFrom = tile;

        // Set tile reference to the new tile
        tile = moveTarget;

        // Set the new tile's piece reference to this piece
        tile.Piece = this;
    }

    public void rewindSimulatedMove()
    {
        // If no active simulated move, do nothing
        if(!movedFrom)
        {
            Debug.Log("rewindSimulatedMove() called with no stored simulated move");
            return;
        }

        Debug.Log($"rewinding simulated move of {team} {type}");

        // If on a tile, set its piece reference to null
        if (destroyedInSimulation)
        {
            tile.Piece = destroyedInSimulation;
            destroyedInSimulation = null;
        }
        else
        {
            tile.Piece = null;
        }

        // Set tile reference to the new tile
        tile = movedFrom;
        movedFrom = null;

        // Set the new tile's piece reference to this piece
        tile.Piece = this;

    }

    public bool IsChecked()
    {
        Debug.Log($"Checking if {team} {type} is checked");
        Team attackingTeam = team == Team.WHITE ? Team.BLACK : Team.WHITE;
        return tile.IsChecked(attackingTeam);
    }

    public bool BelongsToPlayerOne()
    {
        return tile.game.playerOneTeam == team;
    }

    public HashSet<Vector2Int> GetTargets()
    {
        return targeter.GetTargets(tile.row, tile.col, tile.board.tiles, hasMoved);
    }
    public HashSet<Vector2Int> GetMoves()
    {
        return targeter.GetMoves(tile.row, tile.col, tile.board.tiles, hasMoved);
    }

    private void Awake()
    {
        targeter = GetComponent<Targeter>();
        hasMoved = false;
        isKingPiece = false;
    }
}
