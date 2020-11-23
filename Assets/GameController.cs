using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { CHOOSE_PIECE, HIGHLIGHT_MOVES, BETWEEN_TURNS };
public enum Team { WHITE, BLACK };

public class GameController : MonoBehaviour
{

    public GameState state = GameState.CHOOSE_PIECE;
    public Team playerOneTeam = Team.WHITE;
    public Team currentTeam = Team.WHITE;

    private Board board;

    public void endTurn()
    {
        // * End this turn
        state = GameState.BETWEEN_TURNS;
        board.UnhighlightTiles();

        bool gameOver = board.CheckEndOfTurnConditions(currentTeam);

        if (gameOver) Debug.Log("Game Over!");

        // * Start next turn

        // Switch current team
        currentTeam = currentTeam == Team.WHITE ? Team.BLACK : Team.WHITE;

        state = GameState.CHOOSE_PIECE;
    }


    // Start is called before the first frame update
    void Start()
    {
        board = gameObject.GetComponent<Board>();

        board.DrawBoard();

        // Pawns
        for (int i = 0; i < board.cols; ++i)
        {
            board.spawnPiece(1, i, Team.WHITE, "Pawn");
            board.spawnPiece(board.rows - 2, i, Team.BLACK, "Pawn");
        }

        // White Pieces
        board.spawnPiece(0, 0, Team.WHITE, "Rook");
        board.spawnPiece(0, 1, Team.WHITE, "Knight");
        board.spawnPiece(0, 2, Team.WHITE, "Bishop");
        board.spawnPiece(0, 3, Team.WHITE, "Queen");
        board.spawnPiece(0, 4, Team.WHITE, "King");
        board.spawnPiece(0, 5, Team.WHITE, "Bishop");
        board.spawnPiece(0, 6, Team.WHITE, "Knight");
        board.spawnPiece(0, 7, Team.WHITE, "Rook");

        // Black Pieces
        board.spawnPiece(board.rows - 1, 0, Team.BLACK, "Rook");
        board.spawnPiece(board.rows - 1, 1, Team.BLACK, "Knight");
        board.spawnPiece(board.rows - 1, 2, Team.BLACK, "Bishop");
        board.spawnPiece(board.rows - 1, 3, Team.BLACK, "Queen");
        board.spawnPiece(board.rows - 1, 4, Team.BLACK, "King");
        board.spawnPiece(board.rows - 1, 5, Team.BLACK, "Bishop");
        board.spawnPiece(board.rows - 1, 6, Team.BLACK, "Knight");
        board.spawnPiece(board.rows - 1, 7, Team.BLACK, "Rook");
    }
}
