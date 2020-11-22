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
            if (value)
            {
                value.transform.position = this.transform.position;
            }
            piece = value;
        }
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

                if (Piece && Piece.team == game.currentTeam)
                {
                    board.HighlightTiles(Piece);
                }
                else if(State == TileState.TARGETED)
                {
                    if (Piece)
                    {
                        // Kill the piece on this tile
                        Destroy(Piece.gameObject);
                    }

                    // Move the selected piece here
                    Piece = board.selectedTile.Piece;
                    board.selectedTile.Piece = null;
                    Piece.Tile = this;

                    // Invoke end of turn (deselect / unhighlight all)
                    game.endTurn();
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
