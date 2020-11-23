using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Tile tile;
    public Targeter targeter;
    public bool isKingPiece = false;

    private Piece destroyedInSimulation;
    private Tile movedFrom;

    public string type;

    public Tile Tile
    {
        get
        {
            return tile;
        }
        set
        {
            tile = value;
        }
    }

    public Team team;

    public void moveToTile(Tile moveTarget)
    {
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
        // If on a tile, set its piece reference to null
        if (tile) tile.Piece = null;
        
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


    public bool belongsToPlayerOne()
    {
        return tile.game.playerOneTeam == team;
    }

    public HashSet<Vector2Int> GetTargets()
    {
        return targeter.GetTargets(tile.row, tile.col, tile.board.tiles);
    }

    private void Awake()
    {
        targeter = GetComponent<Targeter>();
    }
}
