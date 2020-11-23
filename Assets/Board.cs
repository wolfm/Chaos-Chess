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

    private HashSet<Vector2Int> highlightedTargets;
    public Tile selectedTile;

    public HashSet<Vector2Int> threatened;
    private List<Piece> whitePieces;
    private List<Piece> blackPieces;

    private Piece whiteKing;
    private Piece blackKing;

    public GameController game;

    private void Awake()
    {
        highlightedTargets = new HashSet<Vector2Int>();
        game = FindObjectOfType<GameController>();
        whitePieces = new List<Piece>();
        blackPieces = new List<Piece>();
        threatened = new HashSet<Vector2Int>();
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
    }

    public void spawnPiece(int r, int c, Team team, string piece)
    {
        string teamColor = team == Team.WHITE ? "White" : "Black";
        string location = "Pieces/" + piece + "/" + teamColor + piece;
        Piece newpiece = ((GameObject)Instantiate(Resources.Load(location))).GetComponent<Piece>();

        if (team == Team.WHITE)
        {
            whitePieces.Add(newpiece);
            if (piece == "King")
            {
                newpiece.isKingPiece = (piece == "King");
                whiteKing = newpiece;
            }
        }
        else
        {
            blackPieces.Add(newpiece);
            if (piece == "King")
            {
                newpiece.isKingPiece = (piece == "King");
                blackKing = newpiece;
            }
        }

        tiles[r, c].Piece = newpiece;
        newpiece.transform.localScale *= scaleFactor;
        newpiece.team = team;
        newpiece.Tile = tiles[r, c];
    }

    public void HighlightTiles(Piece piece)
    {
        UnhighlightTiles();

        // Select newly selected tile
        piece.tile.State = TileState.SELECTED;
        selectedTile = piece.tile;

        // Get the next array of targets
        HashSet<Vector2Int> targets = piece.GetTargets();

        //Highlight the next wave of targets
        foreach (Vector2Int target in targets)
        {
            tiles[target.y, target.x].State = TileState.TARGETED;
        }

        // Set the highlighted targets list to be the new highlighted targets list
        highlightedTargets = targets;
    }

    public void UnhighlightTiles()
    {
        // Un-highlight already highlighted targets
        foreach (Vector2Int target in highlightedTargets)
        {
            tiles[target.y, target.x].State = TileState.NONE;
        }

        // Unselect previous tile
        if (selectedTile) selectedTile.State = TileState.NONE;
    }
    public bool CheckForCheck(Team attackingTeam)
    {

        Piece king = attackingTeam == Team.WHITE ? blackKing : whiteKing;
        CalculateThreatened(attackingTeam == Team.WHITE ? whitePieces : blackPieces);

        Debug.Log($"Checking king at {king.tile.row}, {king.tile.col} for check");

        if (threatened.Contains(new Vector2Int(king.tile.col, king.tile.row)))
        {
            Debug.Log("Check");
            return true;
        }
        return false;
    }
    public void CalculateThreatened(List<Piece> attackers)
    {
        threatened.Clear();

        foreach(Piece piece in attackers)
        {
            threatened.UnionWith(piece.GetTargets());
        }
    }
}
