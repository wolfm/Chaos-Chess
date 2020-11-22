using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int row;
    public int col;

    public Board board;
    private GameObject piece;
    public GameObject Piece
    {
        get
        {
            return piece;
        }

        set
        {
            value.transform.position = this.transform.position;
            piece = value;
        }
    }

    void OnMouseDown()
    {
        // If it's your turn
        switch (board.state)
        {
            case Board.GameState.OTHERS_TURN:
                break;

            case Board.GameState.CHOOSE_PIECE:
                if (piece)
                {
                    board.state = Board.GameState.HIGHLIGHT_MOVES;
                }
                else
                {
                    Debug.Log("no piece");
                }
                break;

            case Board.GameState.HIGHLIGHT_MOVES:
                break;
        }

        if(board.state != Board.GameState.OTHERS_TURN)
        {
            
        }
    }
}
