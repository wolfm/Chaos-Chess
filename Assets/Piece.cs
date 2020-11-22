using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Tile tile;
    public Targeter targeter;

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
