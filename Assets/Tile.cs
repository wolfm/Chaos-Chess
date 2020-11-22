using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    
    public GameController game;
    public Board board;
    public int row;
    public int col;

    public Color normalColor;

    private bool highlighted = false;
    public bool Highlighted
    {
        get
        {
            return highlighted;
        }
        
        set
        {
            if (value == true)
                GetComponent<SpriteRenderer>().color = highlightColor;
            else
            {
                GetComponent<SpriteRenderer>().color = normalColor;
            }
        }
    }

    public Color highlightColor;

    [SerializeField]
    private Piece piece;
    
    public Piece Piece
    {
        get
        {
            return piece;
        }

        set
        {

            if (value)
            {
                if(value.Tile) value.Tile.Piece = null;
                value.transform.position = this.transform.position;
            }
            piece = value;
        }
    }

    void highlight()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("Mouse down");
        if(piece)
        {
            Debug.Log("Piece here");
            board.HighlightTargets(piece.GetComponent<Targeter>(), this);
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
