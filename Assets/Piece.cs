using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private Tile tile;

    public Tile Tile
    {
        get
        {
            return tile;
        }
        // Preserve the invariant of one-to-one mapping pieces to tiles
        set
        {
            if(tile) tile.Piece = null;
            value.Piece = this;
            tile = value;

        }
    }

    public bool isWhite;

    public bool belongsToPlayerOne()
    {
        return !(tile.game.playerOneWhite ^ isWhite);
    }
}
