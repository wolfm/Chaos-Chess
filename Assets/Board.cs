using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public enum GameState {OTHERS_TURN, CHOOSE_PIECE, HIGHLIGHT_MOVES, };

    public GameState state;

    //public Material boardMaterial;
    public GameObject referenceTile;
    public int rows = 8;
    public int cols = 8;
    public Color[] tileColors = { Color.black, Color.white };

    public float scaleFactor;

    Tile[,] tiles;

    // Draw the board, centered on the transform
    void DrawBoard()
    {
        // Set up reference tile as a quad with input material
        //GameObject referenceTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
        //referenceTile.GetComponent<Renderer>().material = boardMaterial;

        tiles = new Tile[rows, cols];

        // Generate board with input number of rows and columns, centered at center of screen
        for (int r = 0; r < rows; ++r)
        {
            for (int c = 0; c < cols; ++c)  
            {
                GameObject tile = Instantiate(referenceTile, transform);
                tile.GetComponent<Renderer>().material.color = tileColors[(r + c) % 2];

                tiles[r, c] = tile.GetComponent<Tile>();
                tiles[r, c].row = r;
                tiles[r, c].col = c;
                tiles[r, c].board = this;

                float posX = (c + 0.5f) - (cols / 2.0f);
                float posY = (r + 0.5f) - (rows / 2.0f);

                tile.transform.position = new Vector2(posX, posY);
            }
        }

        // Move board into view
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = 2 * Camera.main.ViewportToWorldPoint(topRightCorner);

        bool portrait = edgeVector.x < edgeVector.y;
        float shortestScreenDim = Mathf.Min(edgeVector.x, edgeVector.y);
        int numTilesInDim = portrait ? cols : rows;

        // Scale factor = (size we want the board to be) / (size the board is)
        scaleFactor = (shortestScreenDim) / (numTilesInDim  + 2);

        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1.0f);

        Destroy(referenceTile);
    }

	// Use this for initialization
	void Start () {
        DrawBoard();

        UnityEngine.Object pawnPrefab = Resources.Load("Pieces/Pawn/WhitePawn");

        // * Default Chess Setup

        // Pawns
        for(int i = 0; i < cols; ++i)
        {
            spawnPiece(1, i, "White", "Pawn");
            //tiles[1, i].Piece = (GameObject) Instantiate(Resources.Load("Pieces/Pawn/WhitePawn"));
            spawnPiece(rows - 2, i, "Black", "Pawn");
            //tiles[rows-2, i].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Pawn/BlackPawn"));
        }

        // White Pieces
        spawnPiece(0, 0, "White", "Rook");
        // tiles[0, 0].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Rook/WhiteRook"));
        spawnPiece(0, 1, "White", "Knight");
        // tiles[0, 1].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Knight/WhiteKnight"));
        spawnPiece(0, 2, "White", "Bishop");
        // tiles[0, 2].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Bishop/WhiteBishop"));
        spawnPiece(0, 3, "White", "Queen");
        // tiles[0, 3].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Queen/WhiteQueen"));
        spawnPiece(0, 4, "White", "King");
        // tiles[0, 4].Piece = (GameObject)Instantiate(Resources.Load("Pieces/King/WhiteKing"));
        spawnPiece(0, 5, "White", "Bishop");
        // tiles[0, 5].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Bishop/WhiteBishop"));
        spawnPiece(0, 6, "White", "Knight");
        // tiles[0, 6].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Knight/WhiteKnight"));
        spawnPiece(0, 7, "White", "Rook");
        // tiles[0, 7].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Rook/WhiteRook"));

        // Black Pieces
        spawnPiece(rows-1, 0, "Black", "Rook");
        // tiles[rows - 1, 0].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Rook/BlackRook"));
        spawnPiece(rows - 1, 1, "Black", "Knight");
        // tiles[rows - 1, 1].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Knight/BlackKnight"));
        spawnPiece(rows - 1, 2, "Black", "Bishop");
        // tiles[rows - 1, 2].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Bishop/BlackBishop"));
        spawnPiece(rows - 1, 3, "Black", "Queen");
        // tiles[rows - 1, 3].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Queen/BlackQueen"));
        spawnPiece(rows - 1, 4, "Black", "King");
        // tiles[rows - 1, 4].Piece = (GameObject)Instantiate(Resources.Load("Pieces/King/BlackKing"));
        spawnPiece(rows - 1, 5, "Black", "Bishop");
        // tiles[rows - 1, 5].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Bishop/BlackBishop"));
        spawnPiece(rows - 1, 6, "Black", "Knight");
        // tiles[rows - 1, 6].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Knight/BlackKnight"));
        spawnPiece(rows - 1, 7, "Black", "Rook");
        // tiles[rows - 1, 7].Piece = (GameObject)Instantiate(Resources.Load("Pieces/Rook/BlackRook"));
    }

    void spawnPiece(int r, int c, string color, string piece)
    {
        string location = "Pieces/" + piece + "/" + color + piece;
        Debug.Log("Attempting to load '" + location + "'");
        tiles[r, c].Piece = (GameObject)Instantiate(Resources.Load(location));
        tiles[r, c].Piece.transform.localScale *= scaleFactor;
    }

    // Update is called once per frame
    void Update () {
		
	}

}
