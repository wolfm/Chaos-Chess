using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { OTHERS_TURN, CHOOSE_PIECE, HIGHLIGHT_MOVES, };

public class GameController : MonoBehaviour
{

    [HideInInspector]
    public GameState state;

    private Board board;

    public bool playerOneWhite = true;

    // Start is called before the first frame update
    void Start()
    {
        board = gameObject.GetComponent<Board>();

        board.DrawBoard();

        // Pawns
        for (int i = 0; i < board.cols; ++i)
        {
            board.spawnPiece(1, i, "White", "Pawn");
            board.spawnPiece(board.rows - 2, i, "Black", "Pawn");
        }

        // White Pieces
        board.spawnPiece(0, 0, "White", "Rook");
        board.spawnPiece(0, 1, "White", "Knight");
        board.spawnPiece(0, 2, "White", "Bishop");
        board.spawnPiece(0, 3, "White", "Queen");
        board.spawnPiece(0, 4, "White", "King");
        board.spawnPiece(0, 5, "White", "Bishop");
        board.spawnPiece(0, 6, "White", "Knight");
        board.spawnPiece(0, 7, "White", "Rook");

        // Black Pieces
        board.spawnPiece(board.rows - 1, 0, "Black", "Rook");
        board.spawnPiece(board.rows - 1, 1, "Black", "Knight");
        board.spawnPiece(board.rows - 1, 2, "Black", "Bishop");
        board.spawnPiece(board.rows - 1, 3, "Black", "Queen");
        board.spawnPiece(board.rows - 1, 4, "Black", "King");
        board.spawnPiece(board.rows - 1, 5, "Black", "Bishop");
        board.spawnPiece(board.rows - 1, 6, "Black", "Knight");
        board.spawnPiece(board.rows - 1, 7, "Black", "Rook");
    }
}
