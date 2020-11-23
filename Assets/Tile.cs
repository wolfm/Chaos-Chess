using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState { NONE, SELECTED, TARGETED };
public class Tile : MonoBehaviour
{    
    public GameController game;
    public Board board;
    public int row;
    public int col;

    public Color normalColor;
    public Color targetedColor;
    public Color selectedColor;

    [SerializeField]
    private Piece piece;

    private TileState state = TileState.NONE;
    public TileState State
    {
        get
        {
            return state;
        }
        
        set
        {
            if (value == TileState.TARGETED)
            {
                GetComponent<SpriteRenderer>().color = targetedColor;
            }
            else if (value == TileState.SELECTED)
            {
                GetComponent<SpriteRenderer>().color = selectedColor;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = normalColor;
            }

            state = value;
        }
    }
    
    public Piece Piece
    {
        get
        {
            return piece;
        }

        set
        {
            // If setting this tile's piece to a piece, not null
            piece = value;
        }
    }

    public bool IsChecked(Team attackingTeam)
    {

        // Debug.Log($"Checking position {row}, {col} for check");

        board.CalculateThreatened(attackingTeam == Team.WHITE ? board.whitePieces : board.blackPieces);


        if (board.threatened.Contains(new Vector2Int(col, row)))
        {
            //Debug.Log($"Check at {row}, {col}");
            return true;
        }

        return false;
    }
    void OnMouseDown()
    {
        switch(game.state)
        {
            case GameState.CHOOSE_PIECE:
                if (Piece && Piece.team == game.currentTeam)
                {
                    board.HighlightTiles(Piece);
                    game.state = GameState.HIGHLIGHT_MOVES;
                }
                break;
            case GameState.HIGHLIGHT_MOVES:

                // Check if targeted first, so castling works (when friendly rook targeted)
                if(State == TileState.TARGETED)
                {
                    // Move the selected piece here
                    board.selectedTile.Piece.moveToTile(this);


                    // Invoke end of turn (deselect / unhighlight all)
                    game.endTurn();
                }
                else if (Piece && Piece.team == game.currentTeam)
                {
                    board.HighlightTiles(Piece);
                }
                break;
        }
    }

    private void Awake()
    {
        game = FindObjectOfType<GameController>();
    }

    private void OnValidate()
    {
        Piece = piece;
    }
}
