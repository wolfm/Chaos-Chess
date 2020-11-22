using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    //public Material boardMaterial;
    public GameObject referenceTile;
    public int rows = 8;
    public int cols = 8;
    public Color[] tileColors = { Color.black, Color.white };

    [HideInInspector]
    public float scaleFactor;

    [HideInInspector]
    public Tile[,] tiles;

    private List<Vector2Int> highlightedTargets;

    private void Awake()
    {
        highlightedTargets = new List<Vector2Int>();
    }

    // Draw the board, centered on the transform
    public void DrawBoard()
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
                tile.GetComponent<SpriteRenderer>().color = tileColors[(r + c) % 2];

                tiles[r, c] = tile.GetComponent<Tile>();
                tiles[r, c].row = r;
                tiles[r, c].col = c;
                tiles[r, c].board = this;
                tiles[r, c].normalColor = tileColors[(r + c) % 2];

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

    public void spawnPiece(int r, int c, string color, string piece)
    {
        string location = "Pieces/" + piece + "/" + color + piece;
        Piece newpiece = ((GameObject)Instantiate(Resources.Load(location))).GetComponent<Piece>();
        tiles[r, c].Piece = newpiece;
        tiles[r, c].Piece.transform.localScale *= scaleFactor;
        tiles[r, c].Piece.isWhite = (color == "White");
        tiles[r, c].Piece.Tile = tiles[r, c];
    }

    public void HighlightTargets(Targeter targeter, Tile tile)
    {
        Debug.Log("HighlightTargets Called");

        // Un-highlight already highlighted targets
        foreach (Vector2Int target in highlightedTargets)
        {
            tiles[target.y, target.x].Highlighted = false;
        }

        // Get the next array of targets
        List<Vector2Int> targets = targeter.GetTargets(tile.row, tile.col, tiles);

        //Highlight the next wave of targets
        foreach(Vector2Int target in targets)
        {
            tiles[target.y, target.x].Highlighted = true;
        }

        // Set the highlighted targets list to be the new highlighted targets list
        highlightedTargets = targets;
    }
}
